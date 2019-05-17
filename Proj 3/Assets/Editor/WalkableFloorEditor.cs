using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PathControl))]
public class WalkableFloorEditor : Editor
{
    private static Tool lastTool = Tool.None;
    private bool isHelpOpen = false;
    public static void StoreTool()
    {
        lastTool = Tools.current;
        Tools.current = Tool.View;
    }

    public static void RestoreTool()
    {
        Tools.current = lastTool;
        lastTool = Tool.None;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        //EditorGUILayout.LabelField("Instructions: ");
        //EditorGUILayout.LabelField("To create a waypoint: ");
        //EditorGUILayout.LabelField("To delete a waypoint: ");

        foreach(Transform waypoint in (target as PathControl).transform)
        {
            if (waypoint.name.StartsWith("Waypoint_"))
            {
                waypoint.Find("Cylinder").gameObject.SetActive(true);
            }
        }
    }

    private void OnDestroy()
    {
        if(target == null)
        {
            return;
        }
        foreach (Transform waypoint in (target as PathControl).transform)
        {
            if (waypoint.name.StartsWith("Waypoint_"))
            {
                waypoint.Find("Cylinder").gameObject.SetActive(false);
            }
        }
    }

    private void OnSceneGUI()
    {
        Event evt = Event.current;

        switch (evt.type)
        {
            case EventType.MouseDown:
                if (evt.button == 0)
                {
                    if (HandleMouseDown(evt.mousePosition)) evt.Use();
                }
                break;
            case EventType.MouseUp:
                if (evt.button == 0)
                {
                    RestoreTool();
                }
                break;
        }
        isHelpOpen = ShowHelp(isHelpOpen);
    }
    public static bool ShowHelp(bool helpOpen)
    {
        Handles.BeginGUI();
        if (helpOpen)
        {
            if (GUI.Button(new Rect(5, 5, 20, 20), "X"))
            {
                helpOpen = false;
            }
            List<string> helpText = new List<string>();
            helpText.Add("Click a walkable surface to add a new waypoint");
            helpText.Add("Click-drag waypoints to move them");
            GUI.Box(new Rect(5, 30, 400, 200), string.Join("\n", helpText));
        }
        else
        {
            if(GUI.Button(new Rect(5, 5, 20, 20), "?"))
            {
                helpOpen = true;
            }
        }
        Handles.EndGUI();
        return helpOpen;
    }

    bool HandleMouseDown(Vector2 mousePosition)
    {
        RaycastHit info;
        if (Physics.Raycast(HandleUtility.GUIPointToWorldRay(mousePosition), out info ))
        {
            if (info.collider.gameObject.GetComponent<WalkableFloor>() != null)
            {
                GameObject waypointTemplate = AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Waypoint.prefab", typeof(GameObject)) as GameObject;                
                GameObject newWaypoint = PrefabUtility.InstantiatePrefab(waypointTemplate) as GameObject;
                newWaypoint.transform.position = info.point;
                newWaypoint.transform.parent = (this.target as PathControl).transform;
                (this.target as PathControl).ResetWaypointNames();
                
                StoreTool();

                GUIUtility.hotControl = GUIUtility.GetControlID(FocusType.Passive);

                Selection.activeObject = newWaypoint;

                return true;
            }

            if (info.transform.parent != null && info.transform.parent.gameObject.tag == "Waypoint")
            {
                StoreTool();
                GUIUtility.hotControl = GUIUtility.GetControlID(FocusType.Passive);

                Selection.activeObject = info.transform.parent.gameObject;
                return true;
            }
        }
        return false;
    }
}
