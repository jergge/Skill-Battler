using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(WFCTerrainManager))]
public class WFCEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Generate Terrain"))
        {
            WFCTerrainManager manager = (WFCTerrainManager)target;
            manager.GenerateAreas();
        }
    }
}
