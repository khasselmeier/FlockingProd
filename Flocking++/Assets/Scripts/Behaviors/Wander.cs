using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wander : Seek
{

    float wanderOffset;
    float wanderRadius;

    float wanderRate = 2;
    float wanderOrientation = 0;
    float maxAcceleration = 100f;

    public override SteeringOutput getSteering()
    {
        SteeringOutput result = new SteeringOutput();

        wanderOrientation += Random.insideUnitCircle.x * wanderRate;


        Vector3 targetPosition = getTargetPosition();
        if (targetPosition == Vector3.positiveInfinity)
        {
            return null;
        }

        result.linear = wanderOrientation * character.transform.position;

        result.linear.Normalize();
        result.linear *= maxAcceleration;

        result.angular = 0;
        return result;
    }

}