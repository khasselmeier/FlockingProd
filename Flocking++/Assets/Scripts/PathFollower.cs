using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFollower : Kinematic
{
    FollowPath myMoveType;
    LookWhereGoing myRotateType;

    public GameObject[] myPath = new GameObject[4];

    void Start()
    {
        // Initialize LookWhereGoing rotation behavior
        myRotateType = new LookWhereGoing
        {
            character = this,
            target = myTarget
        };

        // Initialize FollowPath movement behavior
        myMoveType = new FollowPath
        {
            character = this,
            path = myPath
        };
    }

    protected override void Update()
    {
        // Calculate movement and rotation steering
        SteeringOutput movementSteering = myMoveType.getSteering();
        SteeringOutput rotationSteering = myRotateType.getSteering();

        // Combine steering behaviors
        steeringUpdate = new SteeringOutput
        {
            linear = movementSteering != null ? movementSteering.linear : Vector3.zero,
            angular = rotationSteering != null ? rotationSteering.angular : 0
        };

        // Apply the steering updates to the character
        base.Update();
    }
}