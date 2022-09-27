using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using TerrainGeneration;

[CustomEditor(typeof(WorldMeshGenerator))]
public class TerrainManager : Editor
{

    public override void OnInspectorGUI()
    {
        //base.OnInspectorGUI();


        WorldMeshGenerator generator = (WorldMeshGenerator)target;

        if(DrawDefaultInspector() && generator.autoUpdate)
        {
            generator.GenerateNewChuks();
        }

        if(GUILayout.Button("Generate"))
        {
            generator.GenerateNewChuks();
        }   
        
        if(GUILayout.Button("Clear"))
        {
            generator.ClearChunks();
            EditorUtility.SetDirty(generator);
        }   
    }
}
