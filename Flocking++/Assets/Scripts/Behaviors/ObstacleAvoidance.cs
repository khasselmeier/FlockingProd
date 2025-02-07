using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class CollisionAvoidance : SteeringBehavior
{
    public Kinematic character;
    public float maxAcceleration = 10f;
    public Kinematic[] targets;
    public float radius = 10f;
    public Kinematic target;

    public override SteeringOutput getSteering()
    {
        float shortestTime = float.PositiveInfinity;
        Kinematic firstTarget = null;
        float firstMinSeparation = float.PositiveInfinity;
        float firstDistance = float.PositiveInfinity;
        Vector3 firstRelativePos = Vector3.positiveInfinity;
        Vector3 firstRelativeVel = Vector3.zero;

        SteeringOutput result = new SteeringOutput();

        // Check for obstacles
        foreach (Kinematic obstacle in targets)
        {
            Vector3 relativePos = obstacle.transform.position - character.transform.position;
            Vector3 relativeVel = character.linearVelocity - obstacle.linearVelocity;

            float relativeSpeed = relativeVel.magnitude;
            if (relativeSpeed == 0) continue;

            float timeToCollision = -Vector3.Dot(relativePos, relativeVel) / (relativeSpeed * relativeSpeed);
            float distance = relativePos.magnitude;
            float minSeparation = distance - relativeSpeed * timeToCollision;

            if (minSeparation > 2 * radius || timeToCollision < 0)
            {
                continue;
            }

            if (timeToCollision < shortestTime)
            {
                shortestTime = timeToCollision;
                firstTarget = obstacle;
                firstMinSeparation = minSeparation;
                firstDistance = distance;
                firstRelativePos = relativePos;
                firstRelativeVel = relativeVel;
            }
        }

        // Calc the avoidance force if there is an obstacle
        Vector3 avoidanceForce = Vector3.zero;

        if (firstTarget != null)
        {
            Vector3 avoidanceDirection;
            if (firstMinSeparation <= 0 || firstDistance < 2 * radius)
            {
                avoidanceDirection = character.transform.position - firstTarget.transform.position;
            }
            else
            {
                avoidanceDirection = firstRelativePos + firstRelativeVel * shortestTime;
            }

            avoidanceDirection.y = 0;
            avoidanceForce = avoidanceDirection.normalized * maxAcceleration;
        }

        // Move toward the target
        Vector3 targetDirection = Vector3.zero;
        if (target != null)
        {
            targetDirection = (target.transform.position - character.transform.position).normalized * maxAcceleration;
        }

        result.linear = targetDirection + avoidanceForce;
        result.linear = result.linear.normalized * maxAcceleration;
        result.angular = 0;

        return result;
    }
}