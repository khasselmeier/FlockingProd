using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Evade : Seek
{
    float maxPredictionTime = 1f;

    protected override Vector3 getTargetPosition()
    {
        // 1. Calculate how far ahead in time to predict
        Vector3 directionToTarget = target.transform.position - character.transform.position;
        float distanceToTarget = directionToTarget.magnitude;
        float mySpeed = character.linearVelocity.magnitude;
        float predictionTime;

        if (mySpeed <= distanceToTarget / maxPredictionTime)
        {
            // If I'm far enough away, I can use the max prediction time
            predictionTime = maxPredictionTime;
        }
        else
        {
            // If I'm close enough that my current speed will get me to
            // the target before the max prediction time elapses,
            // use a smaller prediction time
            predictionTime = distanceToTarget / mySpeed;
        }

        // 2. Get the current velocity of the target and add an offset based on prediction time
        Kinematic myMovingTarget = target.GetComponent<Kinematic>();
        if (myMovingTarget == null)
        {
            // Default to base Seek behavior for non-kinematic targets
            return base.getTargetPosition();
        }

        // Return a point opposite to the predicted target position
        Vector3 predictedTargetPosition = target.transform.position + myMovingTarget.linearVelocity * predictionTime;
        Vector3 evadeDirection = character.transform.position - predictedTargetPosition;

        return character.transform.position + evadeDirection.normalized * distanceToTarget;
    }
}