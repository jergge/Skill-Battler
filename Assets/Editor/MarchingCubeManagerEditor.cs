using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using TerrainGeneration;

[CustomEditor(typeof(MarchingCubeManager))]
public class MarchingCubeManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        MarchingCubeManager manager = (MarchingCubeManager)target;

        if (GUILayout.Button("Generate Sample Points"))
        {
            manager.CreateSamplePoints();
            EditorUtility.SetDirty(target);
        }
        if (GUILayout.Button("Generate Sample Cubes"))
        {
            manager.CreateMarchingCubes();
            EditorUtility.SetDirty(target);
        }
        if (GUILayout.Button("Clear Data"))
        {
            manager.ResetData();
            EditorUtility.SetDirty(target);
        }
    }
}
