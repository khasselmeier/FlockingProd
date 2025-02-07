using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteeringBehaviorWeight
{
    public SteeringBehavior behavior = null;
    public float weightFactor = 0f;
}

public class BlendedSteering
{
    public SteeringBehaviorWeight[] behaviorWeights;

    float maxLinearAcceleration = 1f;
    float maxAngularAcceleration = 5f;

    public SteeringOutput ComputeSteering()
    {
        SteeringOutput finalSteering = new SteeringOutput();

        foreach (SteeringBehaviorWeight bw in behaviorWeights)
        {
            SteeringOutput tempSteering = bw.behavior.getSteering();
            if (tempSteering != null)
            {
                finalSteering.angular += tempSteering.angular * bw.weightFactor;
                finalSteering.linear += tempSteering.linear * bw.weightFactor;
            }
        }

        //normalize and cap linear acceleration
        finalSteering.linear = finalSteering.linear.normalized * maxLinearAcceleration;

        //cap angular acceleration if it exceeds the max limit
        float angularMagnitude = Mathf.Abs(finalSteering.angular);
        if (angularMagnitude > maxAngularAcceleration)
        {
            finalSteering.angular /= angularMagnitude;
            finalSteering.angular *= maxAngularAcceleration;
        }

        return finalSteering;
    }
}