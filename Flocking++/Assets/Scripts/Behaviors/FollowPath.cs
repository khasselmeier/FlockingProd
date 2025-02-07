using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPath : Seek
{
    public GameObject[] path;
    private int currentPathIndex = 0;
    public float targetRadius = 0.5f;

    public override SteeringOutput getSteering()
    {
        // If no path is defined or has no waypoints, return null
        if (path == null || path.Length == 0)
        {
            Debug.LogWarning("Path is not defined or empty.");
            return null;
        }

        if (target == null)
        {
            target = FindNearestWaypoint();
        }

        // Check if reached the current target waypoint
        if (IsTargetReached())
        {
            // Move to the next waypoint in the path
            currentPathIndex = (currentPathIndex + 1) % path.Length; // Loop back to the start
            target = path[currentPathIndex];
        }

        // Delegate the steering to seek
        return base.getSteering();
    }

    private GameObject FindNearestWaypoint()
    {
        int nearestIndex = 0;
        float nearestDistance = float.MaxValue;

        for (int i = 0; i < path.Length; i++)
        {
            float distance = Vector3.Distance(character.transform.position, path[i].transform.position);
            if (distance < nearestDistance)
            {
                nearestIndex = i;
                nearestDistance = distance;
            }
        }

        currentPathIndex = nearestIndex;
        return path[nearestIndex];
    }

    private bool IsTargetReached()
    {
        float distanceToTarget = Vector3.Distance(character.transform.position, target.transform.position);
        return distanceToTarget < targetRadius;
    }
}