using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionAvoider : Kinematic
{
    CollisionAvoidance myMoveType;
    public Kinematic[] myTargets;
    public Kinematic myDestination;

    void Start()
    {
        myMoveType = new CollisionAvoidance();
        myMoveType.character = this;
        myMoveType.targets = myTargets;
        myMoveType.target = myDestination;
    }

    protected override void Update()
    {
        SteeringOutput steering = myMoveType.getSteering();

        if (steering != null)
        {
            steeringUpdate = steering;
            linearVelocity += steeringUpdate.linear * Time.deltaTime;
        }

        base.Update();
    }
}