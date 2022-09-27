using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(UpdateableData), true)]
public class NewBehaviourScript : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        UpdateableData data = (UpdateableData)target;

        if (GUILayout.Button("Update"))
        {
            data.NotifyOfUpdatedValues(); 
        }
    }
}
