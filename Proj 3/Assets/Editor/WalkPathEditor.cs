using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(WalkPath))]
public class WalkPathEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        WalkPath myWalkPath = target as WalkPath;

        if(GUILayout.Button("Create and assign new path"))
        {
            GameObject path = AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Path.prefab", typeof(GameObject)) as GameObject;
            GameObject newPath = PrefabUtility.InstantiatePrefab(path) as GameObject;
            newPath.transform.position = myWalkPath.transform.position;
            myWalkPath.pathToWalk = newPath.GetComponent<PathControl>();
            EditorUtility.SetDirty(target);
        }

        if(myWalkPath.pathToWalk != null)
        {
            if(GUILayout.Button("Select Path"))
            {
                Selection.activeGameObject = myWalkPath.pathToWalk.gameObject;
            }
        }
    }
}
