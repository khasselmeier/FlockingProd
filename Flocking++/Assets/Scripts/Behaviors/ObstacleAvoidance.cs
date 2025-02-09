using UnityEngine;

public class ObstacleAvoidance : SteeringBehavior
{
    public Kinematic character;
    public float avoidDistance = 10f;
    public float lookAhead = 6f;
    public float maxAcceleration = 5f;

    public bool flee = false;  // Determines whether the object moves away from detected obstacles

    public override SteeringOutput getSteering()
    {
        SteeringOutput result = new SteeringOutput();

        Vector3 velocityDirection = character.linearVelocity.normalized;
        RaycastHit hit;

        int obstacleLayerMask = LayerMask.GetMask("obstacles"); // Avoid detecting the floor
        Debug.DrawRay(character.transform.position, character.linearVelocity.normalized * lookAhead, Color.red);

        // Cast a ray in the direction of movement to detect obstacles
        if (Physics.Raycast(character.transform.position, velocityDirection, out hit, lookAhead))
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
                avoidanceForce = Vector3.Cross(velocityDirection, Vector3.up).normalized * maxAcceleration;
            }

            result.linear = avoidanceForce;
            result.angular = 0;

            return result;
        }

        return null; // No obstacle detected

    }
}