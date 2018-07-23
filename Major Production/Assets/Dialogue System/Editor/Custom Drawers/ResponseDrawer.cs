using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace Dialogue
{
    [CustomPropertyDrawer(typeof(Response), true)]
    class ResponseDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUILayout.BeginVertical();
            EditorGUILayout.PropertyField(property.FindPropertyRelative("Text"));
            EditorGUILayout.PropertyField(property.FindPropertyRelative("OnChosen"),true);
            EditorGUILayout.PropertyField(property.FindPropertyRelative("Prerequisite"));
            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(property.FindPropertyRelative("transitions"));
            EditorGUI.indentLevel--;
            EditorGUILayout.EndVertical();
        }
    }
}
