using UnityEngine;

public class ObstacleAvoidance : SteeringBehavior
{
    public Kinematic character;
    public float avoidDistance = 5f;
    public float lookAhead = 3f;
    public float maxAcceleration = 2f;

    private float initialY; // Store the bird’s initial height

    public bool flee = false; // Determines whether the object moves away from detected obstacles

    void Start()
    {
        initialY = character.transform.position.y; // Save the starting height
    }

    public override SteeringOutput getSteering()
    {
        SteeringOutput result = new SteeringOutput();
        Vector3 velocityDirection = character.linearVelocity.normalized;
        RaycastHit hit;

        int obstacleLayerMask = LayerMask.GetMask("Obstacles"); // Avoid detecting the floor
        Debug.DrawRay(character.transform.position, velocityDirection * lookAhead, Color.red);

        if (Physics.Raycast(character.transform.position, velocityDirection, out hit, lookAhead, obstacleLayerMask))
        {
            Vector3 avoidanceForce;

            if (flee)
            {
                // Move directly away from the obstacle
                avoidanceForce = (character.transform.position - hit.point).normalized * maxAcceleration;
            }
            else
            {
                // Move to the side to avoid the obstacle
                Vector3 sideDirection = Vector3.Cross(velocityDirection, Vector3.up).normalized;

                // Restrict movement to XZ plane
                avoidanceForce = new Vector3(sideDirection.x, 0, sideDirection.z) * maxAcceleration;
            }

            result.linear = avoidanceForce;
            result.angular = 0;
            return result;
        }

        return null; // No obstacle detected
    }
}