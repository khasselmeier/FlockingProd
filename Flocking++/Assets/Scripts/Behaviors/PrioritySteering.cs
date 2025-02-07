using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrioritySteering
{
    float epsilon = 0.1f;

    public BlendedSteering[] groups;

    public SteeringOutput getSteering()
    {
        SteeringOutput steering = new SteeringOutput();
        foreach (BlendedSteering group in groups)
        {
            steering = group.ComputeSteering();

            if (steering.linear.magnitude > epsilon || Mathf.Abs(steering.angular) > epsilon)
            {
                return steering;
            }
        }

        return steering;
    }
}