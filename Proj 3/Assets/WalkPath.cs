using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkPath : MonoBehaviour
{
    public PathControl pathToWalk;
    int waypointIndex = 0;
    KinematicSeek kinematicSeek;

    private void Start()
    {
        kinematicSeek = GetComponent<KinematicSeek>();

        List<Vector3> waypointLocations = pathToWalk.Waypoints;
        for (int i = 1; i < waypointLocations.Count; i++)
        {
            if (Vector3.Distance(transform.position, waypointLocations[i]) < Vector3.Distance(transform.position, waypointLocations[waypointIndex]))
            {
                waypointIndex = i;
            }
        }

        kinematicSeek.destination = waypointLocations[waypointIndex];
    }
    private void Update()
    {
        if (kinematicSeek.isAtTarget)
        {
            waypointIndex = (waypointIndex + 1) % pathToWalk.Waypoints.Count;
            kinematicSeek.destination = pathToWalk.Waypoints[waypointIndex];
        }
    }
}
