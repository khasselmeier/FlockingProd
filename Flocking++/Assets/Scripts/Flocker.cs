using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flocker : Kinematic
{
    public bool enableObstacleAvoidance = false;
    public GameObject cohesionTarget;

    private float initialY;

    BlendedSteering baseSteering;
    PrioritySteering advancedSteering;
    Kinematic[] nearbyBirds;

    void Start()
    {
        enableObstacleAvoidance = true;
        initialY = transform.position.y;

        //separation behavior
        Separation separationBehavior = new Separation();
        separationBehavior.character = this;

        GameObject[] birdObjects = GameObject.FindGameObjectsWithTag("bird");
        nearbyBirds = new Kinematic[birdObjects.Length - 1];
        int index = 0;
        for (int i = 0; i < birdObjects.Length - 1; i++)
        {
            if (birdObjects[i] == this) continue;
            nearbyBirds[index++] = birdObjects[i].GetComponent<Kinematic>();
        }
        separationBehavior.targets = nearbyBirds;

        //cohesion behavior
        Arrive cohesionBehavior = new Arrive();
        cohesionBehavior.character = this;
        cohesionBehavior.target = cohesionTarget;

        //alignment behavior (look in the flock's direction)
        LookWhereGoing rotationBehavior = new LookWhereGoing();
        rotationBehavior.character = this;

        //base flocking behaviors
        baseSteering = new BlendedSteering();
        baseSteering.behaviorWeights = new SteeringBehaviorWeight[3];
        baseSteering.behaviorWeights[0] = new SteeringBehaviorWeight { behavior = separationBehavior, weightFactor = 1f };
        baseSteering.behaviorWeights[1] = new SteeringBehaviorWeight { behavior = cohesionBehavior, weightFactor = 1f };
        baseSteering.behaviorWeights[2] = new SteeringBehaviorWeight { behavior = rotationBehavior, weightFactor = 1f };

        //obstacle avoidance behavior
        ObstacleAvoidance obstacleAvoidance = new ObstacleAvoidance();
        obstacleAvoidance.character = this;
        obstacleAvoidance.avoidDistance = 3f;
        obstacleAvoidance.lookAhead = 1.5f;
        obstacleAvoidance.flee = true;

        //high-priority obstacle avoidance
        BlendedSteering highPrioritySteering = new BlendedSteering();
        highPrioritySteering.behaviorWeights = new SteeringBehaviorWeight[1];
        highPrioritySteering.behaviorWeights[0] = new SteeringBehaviorWeight { behavior = obstacleAvoidance, weightFactor = 1f };

        //priority-based steering
        advancedSteering = new PrioritySteering();
        advancedSteering.groups = new BlendedSteering[2];
        advancedSteering.groups[0] = highPrioritySteering; // obstacle avoidance has higher priority
        advancedSteering.groups[1] = baseSteering; // regular flocking behavior
    }

    protected override void Update()
    {
        steeringUpdate = new SteeringOutput();

        // Choose steering approach based on whether obstacle avoidance is enabled
        if (!enableObstacleAvoidance)
        {
            steeringUpdate = baseSteering.ComputeSteering();
        }
        else
        {
            steeringUpdate = advancedSteering.getSteering();
        }

        // **Keep the bird at the initial Y height**
        transform.position = new Vector3(transform.position.x, initialY, transform.position.z);

        base.Update();
    }
}