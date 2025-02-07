using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seeker : Kinematic
{
    Seek myMoveType;
    Face mySeekRotateType;
    LookWhereGoing myFleeRotateType;

    public bool flee = false;

    // Start is called before the first frame update
    void Start()
    {
        myMoveType = new Seek();
        //myMoveType = new Wander();
        myMoveType.character = this;
        myMoveType.target = myTarget;
        myMoveType.flee = flee;

        mySeekRotateType = new Face();
        mySeekRotateType.character = this;
        mySeekRotateType.target = myTarget;

        myFleeRotateType = new LookWhereGoing();
        myFleeRotateType.character = this;
        myFleeRotateType.target = myTarget;
    }

    // Update is called once per frame
    protected override void Update()
    {
        steeringUpdate = new SteeringOutput();
        steeringUpdate.linear = myMoveType.getSteering().linear;
        steeringUpdate.angular = flee ? myFleeRotateType.getSteering().angular : mySeekRotateType.getSteering().angular;
        base.Update();
    }
}