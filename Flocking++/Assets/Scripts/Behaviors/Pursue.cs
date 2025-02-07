using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pursue : Seek
{
    // the maximum prediction time
    float maxPredictionTime = 1f;

    // overrides the position seek will aim for
    // assume the target will continue travelling in the same direction and speed
    // pick a point farther along that vector
    protected override Vector3 getTargetPosition()
    {
        // 1. figure out how far ahead in time we should predict
        Vector3 directionToTarget = target.transform.position - character.transform.position;
        float distanceToTarget = directionToTarget.magnitude;
        float mySpeed = character.linearVelocity.magnitude;
        float predictionTime;
        if (mySpeed <= distanceToTarget / maxPredictionTime)
        {
            // if I'm far enough away, I can use the max prediction time
            predictionTime = maxPredictionTime;
        }
        else
        {
            // if I'm close enough that my current speed will get me to 
            // the target before the max prediction time elapses
            // use a smaller prediction time
            predictionTime = distanceToTarget / mySpeed;
        }

        // 2. get the current velocity of our target and add an offset based on our prediction time
        //Kinematic myMovingTarget = target.GetComponent(typeof(Kinematic)) as Kinematic;
        Kinematic myMovingTarget = target.GetComponent<Kinematic>();
        if (myMovingTarget == null)
        {
            // default to seek behavior for non-kinematic targets
            return base.getTargetPosition();
        }

        return target.transform.position + myMovingTarget.linearVelocity * predictionTime;
    }
}