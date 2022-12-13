using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace TerrainGeneration
{
    [CustomEditor(typeof(ITerrainGenerator), true)]
    public class iTerrainGeneratorGUI : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            ITerrainGenerator t = (ITerrainGenerator)target;

            if (GUILayout.Button("Generate"))
            {
                t.GenerateAll();
            }
        }
    }
}
