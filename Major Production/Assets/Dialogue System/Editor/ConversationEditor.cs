using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace Dialogue
{
    [CustomEditor(typeof(Conversation))]
    class ConversationEditor : Editor
    {
        private int entrySelectedIndex;
        //private List<DialogueEntry> entries;
        SerializedProperty entries;
        SerializedProperty selectedEntry;
        SerializedProperty startingID;

        private void OnEnable()
        {
            entries = serializedObject.FindProperty("Entries");
            startingID = serializedObject.FindProperty("startingID");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel("Starting Entry");
            NameIDListPair popupLists = SerializedConversationUtility.GetEntryNameAndID(serializedObject, true);
            startingID.intValue = EditorGUILayout.IntPopup(startingID.intValue, popupLists.Names.ToArray(), popupLists.IDs.ToArray());
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.PropertyField(serializedObject.FindProperty("Speakers"),true);
            

            EditorGUILayout.Separator();
            EditorGUILayout.LabelField("Dialogue Entries");
            // popup to select start dialogue
            entrySelectedIndex = DialogueEntryPopup(entrySelectedIndex);

            // add/remove entries buttons
            EditorGUILayout.BeginHorizontal();
            bool addEntry = false, removeEntry = false;
            addEntry = GUILayout.Button("Add Entry");
            using (new EditorGUI.DisabledScope(entrySelectedIndex >= entries.arraySize)) //disable if index out of range
            {
                removeEntry = GUILayout.Button("Remove Entry");
            }
            EditorGUILayout.EndHorizontal();
            if (addEntry)
            {
                SerializedConversationUtility.AddEntry(serializedObject);
            }
            else if (removeEntry)
            {
                if (entrySelectedIndex < entries.arraySize)
                {
                    entries.DeleteArrayElementAtIndex(entrySelectedIndex);
                    entrySelectedIndex = Mathf.Clamp(entrySelectedIndex,0, entries.arraySize - 1);
                }
            }

            // Show selected entry
            if (entries.arraySize != 0 && entrySelectedIndex < entries.arraySize)
            {
                selectedEntry = entries.GetArrayElementAtIndex(entrySelectedIndex);
                EditorGUILayout.PropertyField(selectedEntry, GUIContent.none, true);
            } else
            {
                EditorGUILayout.LabelField("No Entries");
            }
            serializedObject.ApplyModifiedProperties();
        }

        private int DialogueEntryPopup(int index)
        {
            List<String> entryNames = new List<string>();
            if (entries.arraySize != 0)
            {
                foreach (DialogueEntry entry in ((target as Conversation).Entries))
                {
                    // Prepend ID to force uniqueness, due to popup not showing duplicates
                    entryNames.Add(entry.Name(true));
                }
            }
            return EditorGUILayout.Popup(index, entryNames.ToArray());
            //TODO figure out what to do in case of empty list
        }
    }
}
