using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Waypoint))]
public class WaypointEditor : Editor
{
    private bool helpOpen = false;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        foreach (Transform waypoint in (target as Waypoint).transform.parent)
        {
            if (waypoint.name.StartsWith("Waypoint_"))
            {
                waypoint.Find("Cylinder").gameObject.SetActive(true);
            }
        }
    }

    private void OnDestroy()
    {
        if (target == null) return;
        foreach (Transform waypoint in (target as Waypoint).transform.parent)
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
            case EventType.MouseDrag:
                if (evt.button == 0)
                {
                    if (HandleDrag(evt))
                    {
                        evt.Use();
                    }
                }
                break;
            case EventType.MouseUp:
                if (evt.button == 0)
                {
                    if (HandleMouseUp(evt))
                    {
                        evt.Use();
                    }
                    WalkableFloorEditor.RestoreTool();
                }
                break;
            case EventType.MouseDown:
                if (evt.button == 0)
                {
                    if (HandleMouseDown(evt))
                    {
                        evt.Use();
                    }
                    else
                    {
                        WalkableFloorEditor.RestoreTool();
                    }
                }
                break;
        }
        helpOpen = WalkableFloorEditor.ShowHelp(helpOpen);
    }

    private bool HandleDrag(Event evt)
    {
        if (evt.alt)
        {
            return false;
        }

        Vector2 currentScreen = HandleUtility.WorldToGUIPoint((target as Waypoint).transform.position);
        Vector2 newScreen = currentScreen += evt.delta;

        RaycastHit info;
        if (Physics.Raycast(HandleUtility.GUIPointToWorldRay(newScreen), out info, float.MaxValue, 1 << LayerMask.NameToLayer("WalkableFloor")))
        {
            (target as Waypoint).transform.position = info.point;
            return true;
        }
        return false;
    }

    private bool HandleMouseUp(Event evt)
    {
        return false;
    }

    private bool HandleMouseDown(Event evt)
    {
        RaycastHit info;
        if (Physics.Raycast(HandleUtility.GUIPointToWorldRay(evt.mousePosition), out info, float.MaxValue, 1 << LayerMask.NameToLayer("EditorWaypoint")))
        {
            Waypoint selectedWaypoint = info.transform.parent.GetComponent<Waypoint>();
            if (selectedWaypoint == null)
            {
                selectedWaypoint = info.transform.GetComponent<Waypoint>();
            }

            if (selectedWaypoint == this.target as Waypoint)
            {
                // Nothing?
            }
            else
            {
                Selection.activeObject = selectedWaypoint.gameObject;
            }

            WalkableFloorEditor.StoreTool();
            GUIUtility.hotControl = GUIUtility.GetControlID(FocusType.Passive);
            return true;
        }
        return false;
    }
}
