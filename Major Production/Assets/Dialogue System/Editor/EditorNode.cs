using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

namespace Dialogue
{
    public abstract class EditorNode
    {
        public const int MAX_TITLE_CHARACTERS = 20;

        public Rect rect;

        public string title;

        public GUIStyle Style;
        public GUIStyle SelectedStyle;
        
        public Action<EditorConnector> OnStartMakeTransition;

        public bool isDragged;

        public List<EditorConnector> Connections;

        public EditorNode(Vector2 position, float width, float height, GUIStyle nodeStyle, GUIStyle selectedStyle)
        {
            rect = new Rect(position.x, position.y, width, height);
            Style = nodeStyle;
            SelectedStyle = selectedStyle;
 
            Connections = new List<EditorConnector>();
        }

        public virtual void Drag(Vector2 delta)
        {
            rect.position += delta;
        }

        public virtual void Draw(bool selected)
        {
            if (selected)
            {
                GUI.Box(rect, title, SelectedStyle);
            }
            else
            {
                GUI.Box(rect, title, Style);
            }
        }

        public abstract SerializedProperty ContentsAsProperty(SerializedObject conversation);

        public void ProcessEvents(Event e, DialogueEditorWindow window)
        {
            switch (e.type)
            {
                case EventType.MouseDown:
                    if (rect.Contains(e.mousePosition))
                    {
                        switch (e.button)
                        {
                            case 0://LMB
                                if (window.SelectedConnector == null)
                                {
                                    // If not making connection, select node
                                    GUI.changed = true;
                                    isDragged = true;
                                    window.SelectNode(this);
                                    e.Use();
                                } else
                                {
                                    // If making connection, connect to this
                                    OnClickAsTarget(window, window.SelectedConnector);
                                    window.OnFinishMakeTransition();
                                    e.Use();
                                }
                                break;
                            case 1: //RMB
                                // Create dropdown menu
                                ProcessContextMenu(window);
                                e.Use();
                                break;
                            default:
                                break;
                        }
                    }
                    break;
                case EventType.MouseUp:
                    switch (e.button)
                    {
                        case 0: //LMB up, stop dragging
                            isDragged = false;
                            break;
                        default:
                            break;
                    }
                    break;
                case EventType.MouseDrag:
                    if(e.button == 0 && isDragged)  // LMB
                    {
                        // Drag the event
                        Drag(e.delta);
                        e.Use();
                        GUI.changed = true;
                    }
                    break;
                default:
                    break;
            }
            // Returns whether event consumed by node
        }

        protected abstract void ProcessContextMenu(DialogueEditorWindow window);

        protected abstract void OnClickDelete(DialogueEditorWindow window);

        protected void OnClickNewTransition()
        {
            if(OnStartMakeTransition != null)
            {
                EditorConnector newConnection = new EditorConnector();
                newConnection.Parent = this;
                OnStartMakeTransition(newConnection);
            }
        }

        protected abstract void OnClickAsTarget(DialogueEditorWindow window, EditorConnector connector);

        protected internal abstract bool ConnectObjectToDialogueEntry(DialogueEditorWindow window, int targetID);
    }
}
