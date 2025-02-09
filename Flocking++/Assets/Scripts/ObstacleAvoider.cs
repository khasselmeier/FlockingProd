using UnityEngine;

public class ObstacleAvoider : Kinematic
{
    ObstacleAvoidance myMoveType;

    void Start()
    {
        myMoveType = new ObstacleAvoidance();
        myMoveType.character = this;
    }

    protected override void Update()
    {
        SteeringOutput steering = myMoveType.getSteering();

        if (steering != null)
        {
            steeringUpdate = steering;
            steeringUpdate.linear.y = 0f;
        }

        base.Update();
    }
}