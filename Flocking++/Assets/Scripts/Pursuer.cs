using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pursuer : Kinematic
{
    Pursue myPursueType;   // Handles pursuing
    Evade myEvadeType;     // Handles evading
    LookWhereGoing myRotateType; // Handles rotation alignment

    public bool isEvading = false; // Toggle to switch between pursue and evade

    void Start()
    {
        // Initialize rotation behavior
        myRotateType = new LookWhereGoing
        {
            character = this,
            target = myTarget
        };

        // Initialize pursuing behavior
        myPursueType = new Pursue
        {
            character = this,
            target = myTarget
        };

        // Initialize evading behavior
        myEvadeType = new Evade
        {
            character = this,
            target = myTarget
        };
    }

    protected override void Update()
    {
        SteeringOutput movement;

        // Use pursue or evade based on the toggle
        if (isEvading)
        {
            movement = myEvadeType.getSteering();
        }
        else
        {
            movement = myPursueType.getSteering();
        }

        // Combine linear and angular steering
        steeringUpdate = new SteeringOutput
        {
            linear = movement.linear,
            angular = myRotateType.getSteering().angular
        };

        base.Update();
    }
}