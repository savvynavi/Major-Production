using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace Dialogue
{
    enum ListActions
    {
        Nothing,
        MoveUp,
        MoveDown,
        Delete
    }

    [CustomPropertyDrawer(typeof(DialogueEntry))]
    class DialogueEntryEditor : PropertyDrawer
    {
        const float ID_LABEL_HEIGHT = 15;
        const float ID_LABEL_WIDTH = 25;
        const float LINE_MARGIN = 2;
        const float MOVE_BUTTON_WIDTH = 20;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return base.GetPropertyHeight(property, label);
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            SerializedProperty id = property.FindPropertyRelative("ID");
            SerializedProperty title = property.FindPropertyRelative("Title");
            SerializedProperty speaker = property.FindPropertyRelative("speakerIndex");
            SerializedProperty isEnd = property.FindPropertyRelative("isEnd");
            SerializedProperty text = property.FindPropertyRelative("Text");
            SerializedProperty onEnter = property.FindPropertyRelative("OnEnter");
			SerializedProperty onExit = property.FindPropertyRelative("OnExit");
            SerializedProperty cutsceneEvents = property.FindPropertyRelative("cutsceneEvents");
            SerializedProperty transitions = property.FindPropertyRelative("transitions");
            SerializedProperty responsesList = property.FindPropertyRelative("Responses");
            SerializedProperty response;
            SerializedObject parent = property.serializedObject;//TODO instead use serialized property, and get conversation from that

            EditorGUILayout.BeginVertical();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("ID: ");
            EditorGUILayout.SelectableLabel(id.intValue.ToString());
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.PropertyField(title);
            // Display possible speakers as popup
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel("Speaker: ");
            speaker.intValue = EditorGUILayout.Popup(speaker.intValue, (parent.targetObject as Conversation).Speakers.ToArray());
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.PropertyField(isEnd);
            EditorGUILayout.PropertyField(text);
            EditorGUILayout.Separator();
            EditorGUILayout.PropertyField(onEnter, true);
            EditorGUILayout.PropertyField(cutsceneEvents, true);
            EditorGUILayout.Separator();
            EditorGUILayout.PropertyField(transitions,true);

            EditorGUILayout.Separator();

            // Responses list area
            int responsesSize = responsesList.arraySize;
            ListActions action = ListActions.Nothing;
            int selectedIndex = -1;
            responsesList.isExpanded = EditorGUILayout.Foldout(responsesList.isExpanded, "Responses", true, EditorStyles.boldLabel);
            if (responsesList.isExpanded)
            {
                // Display each response
                for (int i = 0; i < responsesSize; ++i)
                {
                    response = responsesList.GetArrayElementAtIndex(i);

                    string responseTitle = response.FindPropertyRelative("Text").stringValue;
                    if (string.IsNullOrEmpty(responseTitle))
                    {
                        responseTitle = "Response " + i.ToString();
                    }

                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.BeginVertical();
                    // Display response details if expanded
                    response.isExpanded = EditorGUILayout.Foldout(response.isExpanded, responseTitle);
                    if (response.isExpanded)
                    {
                        EditorGUILayout.PropertyField(response, new GUIContent(responseTitle), true); //TODO get text from response, use as label
                    }
                    EditorGUILayout.EndVertical();
                    // Buttons for moving or deleting response
                    bool moveUp = false, moveDown = false, deleteResponse = false;
                    using (new EditorGUI.DisabledScope(i <= 0))     // Disable if first in array
                    {
                        moveUp = GUILayout.Button("^", GUILayout.Width(MOVE_BUTTON_WIDTH));
                    }
                    using (new EditorGUI.DisabledScope(i + 1 >= responsesSize))
                    {
                        moveDown = GUILayout.Button("v", GUILayout.Width(MOVE_BUTTON_WIDTH));
                    }
                    deleteResponse = GUILayout.Button("X", GUILayout.Width(MOVE_BUTTON_WIDTH));
                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.Separator();
                    // Set button action based on buttons clicked
                    if (moveUp)
                    {
                        action = ListActions.MoveUp;
                        selectedIndex = i;
                    }
                    else if (moveDown)
                    {
                        action = ListActions.MoveDown;
                        selectedIndex = i;
                    } else if (deleteResponse)
                    {
                        action = ListActions.Delete;
                        selectedIndex = i;
                    }
                }
                GUILayout.BeginHorizontal();
                bool addItem = GUILayout.Button("Add Response");
                bool clearItems = GUILayout.Button("Clear Responses");
                GUILayout.EndHorizontal();
                // Perform action based on buttons clicked
                if (addItem)
                {
                    responsesList.InsertArrayElementAtIndex(responsesSize);
                }
                if (clearItems)
                {
                    responsesList.arraySize = 0;
                }
                switch (action)
                {
                    case ListActions.MoveUp:
                        if (selectedIndex > 0)
                        {
                            responsesList.MoveArrayElement(selectedIndex, selectedIndex - 1);
                        }
                        break;
                    case ListActions.MoveDown:
                        if (selectedIndex >= 0 && selectedIndex < (responsesSize - 1))
                        {
                            responsesList.MoveArrayElement(selectedIndex, selectedIndex + 1);
                        }
                        break;
                    case ListActions.Delete:
                        if(selectedIndex >= 0 && selectedIndex < (responsesSize))
                        {
                            responsesList.DeleteArrayElementAtIndex(selectedIndex);
                        }
                        break;
                    case ListActions.Nothing:
                    default:
                        break;
                }
            }
            else
            {
                responsesList.isExpanded = GUILayout.Button("Show");
            }

            EditorGUILayout.EndVertical();
        }

    }
}
