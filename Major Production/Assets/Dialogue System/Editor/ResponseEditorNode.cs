using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace Dialogue
{
    public class ResponseEditorNode : EditorNode
    {
        public Response response;
        public int entryID;
        public int index;
        public Conversation conversation;

        public ResponseEditorNode(Vector2 position, float width, float height, GUIStyle nodeStyle, GUIStyle selectedStyle) : base(position, width, height, nodeStyle, selectedStyle)
        {
        }

        public override SerializedProperty ContentsAsProperty(SerializedObject conversation)
        {
            SerializedProperty entry = SerializedArrayUtility.FindPropertyByValue(conversation.FindProperty("Entries"), "ID", entryID);
            return entry.FindPropertyRelative("Responses").GetArrayElementAtIndex(index);
        }

        public override void Drag(Vector2 delta)
        {
            base.Drag(delta);
            response.Position = rect.position;  // Save position of response
        }

        public override void Draw(bool selected)
        {
            if (string.IsNullOrEmpty(response.Text))
            {
                title = "Response " + index.ToString();
            }
            else
            {
                title = StringUtility.TruncateString(response.Text, MAX_TITLE_CHARACTERS);
            }
            base.Draw(selected);
        }

        protected override void OnClickAsTarget(DialogueEditorWindow window, EditorConnector connector)
        {
            // Response can't be made a target as it belongs to its dialogue entry
        }

        protected override void OnClickDelete(DialogueEditorWindow window)
        {
            SerializedObject conversation = window.SerializedConversation;
            if(conversation != null)
            {
                // Delete response from conversation
                conversation.Update();
                SerializedProperty entry = SerializedConversationUtility.FindEntry(conversation, entryID);
                entry.FindPropertyRelative("Responses").DeleteArrayElementAtIndex(index);
                conversation.ApplyModifiedProperties();
            }
        }

        protected override void ProcessContextMenu(DialogueEditorWindow window)
        {
            // Make context menu for right click
            GenericMenu menu = new GenericMenu();
            menu.AddItem(new GUIContent("Add Transition"), false, OnClickNewTransition);
            menu.AddItem(new GUIContent("Delete"), false, () => OnClickDelete(window));
            menu.ShowAsContext();
        }

        protected internal override bool ConnectObjectToDialogueEntry(DialogueEditorWindow window, int targetID)
        {
            SerializedObject conversation = window.SerializedConversation;
            bool success = false;
            if(conversation != null)
            {
                conversation.Update();
                SerializedProperty serializedEntry = SerializedConversationUtility.FindEntry(conversation, entryID);
                SerializedProperty serializedResponse = serializedEntry.FindPropertyRelative("Responses").GetArrayElementAtIndex(index);
                SerializedProperty transitions = serializedResponse.FindPropertyRelative("transitions.transitions");
                //TODO write some SerializedTransitionListUtility with an insert new transition function
                int oldSize = transitions.arraySize;
                transitions.InsertArrayElementAtIndex(oldSize);
                SerializedProperty newTransition = transitions.GetArrayElementAtIndex(oldSize);
                newTransition.FindPropertyRelative("condition").objectReferenceValue = null;
                newTransition.FindPropertyRelative("transition.TargetID").intValue = targetID;
                conversation.ApplyModifiedProperties();
                success = true;
            }
            return success;
        }
    }
}
