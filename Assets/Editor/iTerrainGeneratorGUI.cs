using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(iTerrainGenerator), true)]
public class iTerrainGeneratorGUI : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        iTerrainGenerator t = (iTerrainGenerator)target;

        if (GUILayout.Button("Generate"))
        {
            t.GenerateAll();
        }
    }
}
