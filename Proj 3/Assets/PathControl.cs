using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathControl : MonoBehaviour
{
    private List<Vector3> waypointLocations = new List<Vector3>();
    public List<Vector3> Waypoints { get { return waypointLocations; } }

    void Start()
    {
        foreach (Transform waypoint in transform)
        {
            if (waypoint.name.StartsWith("Waypoint_"))
            {
                waypointLocations.Add(waypoint.transform.position);
                waypoint.Find("Cylinder").gameObject.SetActive(false);
            }
        }
    }

    public void ResetWaypointNames()
    {
        int ix = 0;
        foreach (Transform waypoint in transform)
        {
            if (waypoint.name.StartsWith("Waypoint"))
            {
                waypoint.name = "Waypoint_" + ix.ToString("0000");
                ix++;
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        DrawPathGizmos();
    }

    public void DrawPathGizmos()
    {
        Color original = Gizmos.color;
        Gizmos.color = Color.green;
        List<Transform> waypoints = new List<Transform>();
        foreach (Transform waypoint in transform)
        {
            if (waypoint.name.StartsWith("Waypoint"))
            {
                waypoints.Add(waypoint);
            }
        }

        for (int i = 0; i < waypoints.Count; i++)
        {
            Vector3 start = waypoints[i].position;
            Vector3 end = waypoints[(i + 1) < waypoints.Count ? i + 1 : 0].position;

            RaycastHit hitInfo;
            if (Physics.Raycast(start + Vector3.up * 0.2f, end - start, out hitInfo))
            {
                if (hitInfo.collider.transform.parent != null && hitInfo.collider.transform.parent.position == end)
                    Gizmos.color = Color.green;
                else
                {
                    Gizmos.color = Color.red;
                }
            }

            Gizmos.DrawLine(start, end);

            Vector3 center = (start + end) / 2;
            Vector3 startToEnd = (end - start).normalized;
            Vector3 lineNormal = new Vector3(startToEnd.z, startToEnd.y, -startToEnd.x); 
            Gizmos.DrawLine(center, center - startToEnd * 0.3f + lineNormal * 0.3f); 
            Gizmos.DrawLine(center, center - startToEnd * 0.3f - lineNormal * 0.3f);
        }
        Gizmos.color = original;
    }
}
