using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace TerrainGeneration
{
    public class ExposedScriptableObjectAttribute : PropertyAttribute
    {

    }

//     [CustomPropertyDrawer(typeof(ExposedScriptableObjectAttribute))]
//     public class ExposedScriptableObjectAttributeDrawer : PropertyDrawer
//     {
//         private Editor editor = null;

//         public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
//         {
//             //if (Event.current.type == EventType.Repaint)
//             //{
//                 EditorGUI.PropertyField(position, property, label, true);

//                 if (property.objectReferenceValue != null)
//                 {
//                     property.isExpanded = EditorGUI.Foldout(position, property.isExpanded, GUIContent.none);
//                 }

//                 if (property.isExpanded)
//                 {
//                     EditorGUI.indentLevel++;

//                     if (!editor)
//                     {
//                         Editor.CreateCachedEditor(property.objectReferenceValue, null, ref editor);
//                     }
//                     editor.OnInspectorGUI();

//                     EditorGUI.indentLevel--;
//                 }
//             //}
//         }
//     }
}
