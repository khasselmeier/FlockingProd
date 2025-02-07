using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Separation : SteeringBehavior
{
    public Kinematic character;
    float maxAcceleration = 1f;

    public Kinematic[] targets;

    float threshold = 5f;
    float decayCoefficient = 100f;

    public override SteeringOutput getSteering()
    {
        SteeringOutput result = new SteeringOutput();

        foreach (Kinematic target in targets)
        {
            Vector3 direction = character.transform.position - target.transform.position;
            float distance = direction.magnitude;

            if (distance < threshold)
            {
                float strength = Mathf.Min(decayCoefficient / (distance * distance), maxAcceleration);
                direction.Normalize();
                result.linear += strength * direction;
            }
        }

        return result;
    }
}