using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.Linq;

// TODO setup nodes/update nodes also creates response nodes
// TODO test REsponseEditorNode functionality once displayed
// TODO expand/collapse response nodes?


    //TODO when Selected Conversation changes, errors from selected things bieng out of range
namespace Dialogue
{
    public class DialogueEditorWindow : EditorWindow
    {

        const float RESIZER_WIDTH = 10.0f;
        const float PANEL_MIN_WIDTH = 0.05f;
        const int NODE_BORDER = 12;
        const float NODE_WIDTH = 200.0f;
        const float NODE_HEIGHT = 50.0f;
        const int NODE_FONT_SIZE = 14;

        Conversation conversation;                  // Conversation selected in Unity Editor
        public Conversation SelectedConversation { get { return conversation; } }
        SerializedObject serializedConversation;    // Serialized object created from conversation
        public SerializedObject SerializedConversation { get { return serializedConversation; } }

        List<EditorNode> nodes;

        EditorNode selectedNode;

        EditorConnector selectedConnector;

        public EditorConnector SelectedConnector { get { return selectedConnector; } }

        Rect nodePanel;
        Rect editPanel;
        Rect resizeBar;

        Vector2 editScrollPosition;

        GUIStyle resizerStyle;
        GUIStyle dialogueNodeStyle;
        GUIStyle dialogueSelectedNodeStyle;
        GUIStyle responseNodeStyle;
        GUIStyle responseSelectedNodeStyle;
        // TODO will need to have styles for different types of node? 
        
        // TODO maybe use an enum of states instead?
        bool isResizing;
        bool isMovingCanvas;

        // HACK maybe edit panel should have absolute width?
        float nodePanelWidth = 0.8f;

        [MenuItem("Window/Dialogue")]
        static void Init()
        {
            DialogueEditorWindow window = EditorWindow.GetWindow<DialogueEditorWindow>();
            window.Show();
        }

        private void OnEnable()
        {
            // Setup GUI styles
            resizerStyle = new GUIStyle();
            resizerStyle.normal.background = EditorGUIUtility.Load("icons/d_AvatarBlendBackground.png") as Texture2D;

            dialogueNodeStyle = new GUIStyle();
            dialogueNodeStyle.normal.background = EditorGUIUtility.Load("builtin skins/darkskin/images/node1.png") as Texture2D;
            dialogueNodeStyle.border = new RectOffset(NODE_BORDER, NODE_BORDER, NODE_BORDER, NODE_BORDER);
            dialogueNodeStyle.alignment = TextAnchor.MiddleCenter;
            dialogueNodeStyle.fontSize = NODE_FONT_SIZE;

            dialogueSelectedNodeStyle = new GUIStyle();
            dialogueSelectedNodeStyle.normal.background = EditorGUIUtility.Load("builtin skins/darkskin/images/node1 on.png") as Texture2D;
            dialogueSelectedNodeStyle.border = new RectOffset(NODE_BORDER, NODE_BORDER, NODE_BORDER, NODE_BORDER);
            dialogueSelectedNodeStyle.alignment = TextAnchor.MiddleCenter;
            dialogueSelectedNodeStyle.fontSize = NODE_FONT_SIZE;

            responseNodeStyle = new GUIStyle();
            responseNodeStyle.normal.background = EditorGUIUtility.Load("builtin skins/darkskin/images/node2.png") as Texture2D;
            responseNodeStyle.border = new RectOffset(NODE_BORDER, NODE_BORDER, NODE_BORDER, NODE_BORDER);
            responseNodeStyle.alignment = TextAnchor.MiddleCenter;
            responseNodeStyle.fontSize = NODE_FONT_SIZE;

            responseSelectedNodeStyle = new GUIStyle();
            responseSelectedNodeStyle.normal.background = EditorGUIUtility.Load("builtin skins/darkskin/images/node2 on.png") as Texture2D;
            responseSelectedNodeStyle.border = new RectOffset(NODE_BORDER, NODE_BORDER, NODE_BORDER, NODE_BORDER);
            responseSelectedNodeStyle.alignment = TextAnchor.MiddleCenter;
            responseSelectedNodeStyle.fontSize = NODE_FONT_SIZE;

            nodes = new List<EditorNode>();
            conversation = null;
            serializedConversation = null;
        }

        // OnGUI is called whenever a GUI event occurs and the window is in focus
        private void OnGUI()
        {
            // Get currently selected object in unity
            Conversation selectedConversation = Selection.activeObject as Conversation;

            //HACK maybe this stuff with drawing nodes should be moved to Update or OnInspectorUpdate
            // If so, UpdateNodes should return whether anything changed/set a dirty flag, which is used to decide if repainting needed

            if(selectedConversation == null)
            {
                // If it's not a conversation, clear everything
                // TODO dealing with no conversation selected (clear everything?)
                nodes.Clear();
                conversation = null;
                serializedConversation = null;
            } else if (selectedConversation != conversation)
            {
                // If a new conversation is selected, change to it
                ChangeConversation(selectedConversation);
            } else
            {
                // If it's the same, update nodes to reflect any changes
                UpdateNodes();
            }

            if(selectedConnector != null)
            {
                // If creating a new connection, move the connection's end to the mouse cursor
                selectedConnector.FollowMouse(Event.current);
                GUI.changed = true;
            }

            // Draw each panel of the window
            DrawNodePanel();
            DrawEditPanel();
            DrawResizeBar();
            
            // Handle current GUI event
            ProcessEvents(Event.current);

            // Repaint window on GUI change
            if (GUI.changed)
            {
                Repaint();
            }
        }

        private void DrawNodePanel()
        {
            nodePanel = new Rect(0, 0, position.width * nodePanelWidth, position.height);
            // Begins a GUILayout block for the Node panel
            GUILayout.BeginArea(nodePanel);
            // TODO draw grid
            if (conversation != null)
            {
                DrawNodes();
            }
            else
            {
                GUILayout.Label("No Conversation Selected", EditorStyles.boldLabel);
            }
            GUILayout.EndArea();
        }

        private void DrawNodes()
        {
            if (nodes != null)
            {
                // Draw connections between nodes underneath them
                foreach (EditorNode node in nodes)
                {
                    foreach (EditorConnector connector in node.Connections)
                    {
                        connector.Draw();
                    }
                }

                // SelectedConnector doesn't belong to any node yet so draw it here
                if (selectedConnector != null)
                {
                    selectedConnector.Draw();
                }

                // Draw each node
                foreach (EditorNode node in nodes)
                {
                    node.Draw(node == selectedNode);    // passing true makes node draw with Selected style
                }
            }
        }

        private void DrawEditPanel()
        {
            // Begins GUI layout area for edit panel
            editPanel = new Rect((position.width * nodePanelWidth) + 5, 0, position.width * (1 - nodePanelWidth) - (RESIZER_WIDTH * 0.5f), position.height);
            GUILayout.BeginArea(editPanel);
            // Begins a Scroll View area. This takes the current scroll position and returns the new position scrolled to
            editScrollPosition = GUILayout.BeginScrollView(editScrollPosition);
            // HACK display conversation selected some other way
            EditorGUILayout.ObjectField("Conversation", conversation, typeof(Conversation), false);
            if (conversation == null) {
                //todo maybe say something?
            }
            else {
                // Draws the property for the selected node
                // As with custom inspectors, the SerializedObject representing the Conversation needs to be updated and modified
                serializedConversation.Update();

                SerializedProperty startingID = serializedConversation.FindProperty("startingID");
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.PrefixLabel("Starting Entry");
                NameIDListPair popupLists = SerializedConversationUtility.GetEntryNameAndID(serializedConversation, true);
                startingID.intValue = EditorGUILayout.IntPopup(startingID.intValue, popupLists.Names.ToArray(), popupLists.IDs.ToArray());
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.PropertyField(serializedConversation.FindProperty("Speakers"), true);
                EditorGUILayout.Separator();
                // Get the property from the serializedConversation corresponding to the selected node
                SerializedProperty selectedProperty = null;
                if (selectedNode != null)
                {
                    selectedProperty = selectedNode.ContentsAsProperty(serializedConversation);
                }
                if (selectedProperty != null)
                {
                    // Displays the selected property using the appropriate custom drawer
                    EditorGUILayout.PropertyField(selectedProperty);
                }
                // End by applying any modifications to the SerializedObject
                serializedConversation.ApplyModifiedProperties();
            }
            // Close ScrollView and Area
            GUILayout.EndScrollView();
            GUILayout.EndArea();
        }

        private void DrawResizeBar()
        {
            resizeBar = new Rect((position.width * nodePanelWidth) - (0.5f * RESIZER_WIDTH), 0, RESIZER_WIDTH, position.height);
            GUILayout.BeginArea(new Rect(resizeBar.position + (Vector2.right * 0.5f * RESIZER_WIDTH), new Vector2(2, position.height)), resizerStyle);
            GUILayout.EndArea();
            
            // Change mouse cursor while over resize bar
            EditorGUIUtility.AddCursorRect(resizeBar, MouseCursor.ResizeHorizontal);
        }

        private void ProcessEvents(Event e)
        {
            // Events are produced for user input
            // Calling e.Use changes the event's type to Used, so other objects know it's already been handled

            // Block for things that should happen regardless of mouse position
            switch (e.type)
            {
                case EventType.MouseDown:
                    switch (e.button)
                    {
                        case 1:
                            // Right click anywhere cancels make transition
                            OnCancelMakeTransition();
                            break;
                    }
                    break;
            }

            ProcessResizeBarEvents(e);
            ProcessEditPanelEvents(e);
            // HACK skipping processing for later events without consuming so property drawer can use event
            if (!(e.isMouse && editPanel.Contains(e.mousePosition)))
            {
                ProcessNodeEvents(e);
                ProcessNodePanelEvents(e);
            }
        }

        private void ProcessResizeBarEvents(Event e)
        {
            switch (e.type)
            {
                case EventType.MouseDown:
                    if (e.button == 0 && resizeBar.Contains(e.mousePosition))
                    {
                        // LMB click inside resize bar starts resizing
                        isResizing = true;
                        e.Use();    // Consumes the event
                    }
                    break;

                case EventType.MouseUp:
                    // Stop resizing on mouse up
                    if (e.button == 0)
                    {
                        isResizing = false;
                    }
                    break;

                case EventType.MouseDrag:
                    if (isResizing)
                    {
                        // If resizing, resize panels when mouse dragged
                        ResizePanels(e);
                        e.Use();    // Consumes the event
                    }
                    break;
                // Jump here if already used (not sure if this is actually helpful though)
                case EventType.Used:
                default:
                    break;
            }
        }

        private void ProcessEditPanelEvents(Event e)
        {
            switch (e.type)
            {
                case EventType.MouseUp:
                    switch (e.button) {
                        case 0:
                        if (selectedNode != null)
                        {
                            // Unselect nodes when mouse up
                            selectedNode.isDragged = false;
                        }
                            break;
                        case 1:
                            break;
                        case 2:
                            // Stop dragging canvas on mouse up
                            isMovingCanvas = false;
                            break;
                    }
                    break;
                default:
                    break;
            }
        }

        private void ProcessNodePanelEvents(Event e)
        {
            switch (e.type)
            {
                case EventType.MouseDown:
                    if (nodePanel.Contains(e.mousePosition))
                    {
                        switch (e.button)
                        {
                            case 0: //LMB
                                // Deselect node
                                SelectNode(null);
                                e.Use();    //Consumes the event
                                break;
                            case 1: //RMB
                                // Show a menu on right clicking
                                ProcessContextMenu(e.mousePosition);
                                e.Use();    // Consumes the event
                                break;
                            case 2: //Middle
                                // Start dragging canvas on middle click
                                isMovingCanvas = true;
                                e.Use(); // Consumes the event
                                break;
                        }
                        
                    }
                    break;
                case EventType.MouseUp:
                    switch (e.button)
                    {
                        case 0: //LMB
                        case 1: //RMB
                            break;
                        case 2: // middle
                            // stop dragging canvas
                            isMovingCanvas = false;
                            break;
                    }
                    break;
                case EventType.MouseDrag:
                    if (isMovingCanvas)
                    {
                        // If canvas being dragged, move the nodes
                        foreach (EditorNode node in nodes)
                        {
                            node.Drag(e.delta);
                        }
                        e.Use();    // Consumes the event
                    }
                    break;
                case EventType.Used:
                default:
                    break;
            }
        }

        private void ProcessNodeEvents(Event e)
        {
            if(nodes != null)
            {
                // Going backwards so last drawn is checked first
                // This ensures the node clicked on is the one the user can actually see
                for(int i = nodes.Count - 1; i >= 0; --i)
                {
                    nodes[i].ProcessEvents(e, this);

                    // If the node's consumed the event, break loop
                    if (e.type == EventType.Used)
                    {
                        break;
                    }
                }
            }
        }

        private void ResizePanels(Event e)
        {
            // Sets panel width as proportion of total window width
            nodePanelWidth = Mathf.Clamp(e.mousePosition.x / position.width, PANEL_MIN_WIDTH, 1.0f - PANEL_MIN_WIDTH);
            Repaint();
        }

        private void ProcessContextMenu(Vector2 mousePos)
        {
            // Creates and displays a context menu
            GenericMenu menu = new GenericMenu();
            // Set items based on area clicked
            if (nodePanel.Contains(mousePos))
            {
                // Add menu items with functions to be called on clicking item
                menu.AddItem(new GUIContent("Add dialogue entry"), false, () => OnClickAddNode(mousePos));
            }
            // Show the menu
            menu.ShowAsContext();
        }

        private void OnClickAddNode(Vector2 pos)
        {
            if(serializedConversation != null)
            {
                serializedConversation.Update();
                // Add DialogueEntry to serialized conversation
                SerializedProperty newEntry = SerializedConversationUtility.AddEntry(serializedConversation);
                newEntry.FindPropertyRelative("position").vector2Value = pos;
                serializedConversation.ApplyModifiedProperties();
            }
        }

        public void SelectNode(EditorNode node)
        {
            selectedNode = node;
        }


        void OnStartMakeTransition(EditorConnector connector)
        {
            selectedConnector = connector;
        }

        void OnCancelMakeTransition()
        {
            if (selectedConnector != null)
            {
                selectedConnector = null;
            }
        }

        public void OnFinishMakeTransition()
        {
            selectedConnector = null;
        }

        void SetupNodeActions(EditorNode node)
        {
            node.OnStartMakeTransition = OnStartMakeTransition;
        }

        void ChangeConversation(Conversation newConversation)
        {
            selectedConnector = null;
            selectedNode = null;
            if (nodes != null)
            {
                nodes.Clear();
            } else
            {
                nodes = new List<EditorNode>();
            }
            if(newConversation  != null)
            {
                conversation = newConversation;
                serializedConversation = new SerializedObject(conversation);
                foreach(DialogueEntry entry in conversation.Entries)
                {
                    AddNode(entry);
                }
                foreach(DialogueEntry entry in conversation.Entries)
                {
                    SetupNodeConnections(entry);
                }
            }
        }

        void UpdateNodes()
        {
            HashSet<int> seenIDs = new HashSet<int>();
            List<EditorNode> toRemove = new List<EditorNode>();
            List<EditorNode> nodeToAdd = new List<EditorNode>();
            foreach(EditorNode node in nodes)
            {
                DialogueEntryEditorNode dialogueNode = node as DialogueEntryEditorNode;
                ResponseEditorNode responseNode = node as ResponseEditorNode;
                if(dialogueNode != null)
                {
                    // Check if node is removed from conversation
                    seenIDs.Add(dialogueNode.entryID);
                    DialogueEntry entry = conversation.FindEntry(dialogueNode.entryID);
                    if (entry == null)
                    {
                        toRemove.Add(node);
                        // Remove any responses from destroyed node
                        foreach (EditorConnector connection in dialogueNode.Connections)
                        {
                            if (connection.Target as ResponseEditorNode != null)
                            {
                                toRemove.Add(connection.Target);
                            }
                        }

                    }
                    else
                    {
                        // Check responses on node
                        dialogueNode.entry = entry;
                        int maxIndexSeen = -1;
                        foreach (EditorConnector connection in dialogueNode.Connections)
                        {
                            //check if there's responses to remove
                            ResponseEditorNode childNode = connection.Target as ResponseEditorNode;
                            if(childNode != null)
                            {
                                if(childNode.index < entry.Responses.Count)
                                {
                                    maxIndexSeen = Mathf.Max(maxIndexSeen, childNode.index);
                                    childNode.response = entry.Responses[childNode.index];

                                } else
                                {
                                    // Node has out of range index
                                    toRemove.Add(childNode);
                                }
                            }
                        }
                        // Add any needed responses
                        for(int i = maxIndexSeen + 1; i < entry.Responses.Count(); ++i)
                        {
                            Response response = entry.Responses[i];
                            ResponseEditorNode newNode = new ResponseEditorNode(response.Position, NODE_WIDTH, NODE_HEIGHT, responseNodeStyle, responseSelectedNodeStyle);
                            newNode.response = response;
                            newNode.entryID = entry.ID;
                            newNode.index = i;
                            SetupNodeActions(newNode);
                            EditorConnector connector = new EditorConnector();
                            connector.Parent = dialogueNode;
                            connector.Target = newNode;
                            dialogueNode.Connections.Add(connector);
                            nodeToAdd.Add(newNode);
                        }
                    }
                }
            }
            // Remove nodes and any connections to them
            nodes.RemoveAll(n => toRemove.Contains(n));
            if (selectedConnector != null && toRemove.Contains(selectedConnector.Parent))
            {
                selectedConnector = null;
            }
            if (toRemove.Contains(selectedNode))
            {
                selectedNode = null;
            }
            foreach (EditorNode node in nodes)
            {
                node.Connections.RemoveAll(c => toRemove.Contains(c.Target));
            }
            // Create new nodes for any dialogue entries not already seen
            List<DialogueEntry> toAdd = conversation.Entries.Where(e => !seenIDs.Contains(e.ID)).ToList();
            foreach(DialogueEntry entry in toAdd)
            {
                AddNode(entry);
            }
            // Add new response nodes to list
            nodes.AddRange(nodeToAdd);
            // Now all nodes exist (or don't) check all transitions
            foreach (EditorNode node in nodes)
            {
                DialogueEntryEditorNode dialogueNode = node as DialogueEntryEditorNode;
                ResponseEditorNode responseNode = node as ResponseEditorNode;
                List<int> targetsRequired;
                List<EditorConnector> connectorToRemove = new List<EditorConnector>();
                if (dialogueNode != null)
                {
                    targetsRequired = dialogueNode.entry.transitions.transitions.Select(o => o.transition.TargetID).ToList();
                } else if(responseNode != null)
                {
                    targetsRequired = responseNode.response.transitions.transitions.Select(o => o.transition.TargetID).ToList();
                } else 
                {
                    targetsRequired = new List<int>();
                }


                foreach (EditorConnector connection in node.Connections)
                {
                    DialogueEntryEditorNode targetDialogue = connection.Target as DialogueEntryEditorNode;
                    if (targetDialogue != null)
                    {
                        if (targetsRequired.Contains(targetDialogue.entryID))
                        {
                            targetsRequired.Remove(targetDialogue.entryID);
                        }
                        else
                        {
                            connectorToRemove.Add(connection);
                        }
                    }
                }
                node.Connections.RemoveAll(c => connectorToRemove.Contains(c));
                // Create required connections
                foreach (int targetID in targetsRequired)
                {
                    DialogueEntryEditorNode targetNode = FindDialogueNode(targetID);
                    if (targetNode != null)
                    {
                        EditorConnector newConnector = new EditorConnector();
                        newConnector.Parent = node;
                        newConnector.Target = targetNode;
                        node.Connections.Add(newConnector);
                    }
                }
            }
        }

        DialogueEntryEditorNode FindDialogueNode(int ID)
        {
            DialogueEntryEditorNode found = null;
            if(nodes != null)
            {
                foreach (EditorNode n in nodes)
                {
                    DialogueEntryEditorNode dialogueNode = n as DialogueEntryEditorNode;
                    if (dialogueNode != null)
                    {
                        if (ID == dialogueNode.entryID)
                        {
                            found = dialogueNode;
                            break;
                        }
                    }
                }
            }
            return found;
        }

        ResponseEditorNode FindResponseNode(int ID, int index)
        {
            ResponseEditorNode found = null;
            if(nodes != null)
            {
                foreach(EditorNode n in nodes)
                {
                    ResponseEditorNode responseNode = n as ResponseEditorNode;
                    if(responseNode != null)
                    {
                        if(responseNode.entryID == ID && responseNode.index == index)
                        {
                            found = responseNode;
                            break;
                        }
                    }
                }
            }
            return found;
        }

        DialogueEntryEditorNode AddNode(DialogueEntry entry)
        {
            DialogueEntryEditorNode node = new DialogueEntryEditorNode(entry.position, NODE_WIDTH, NODE_HEIGHT, dialogueNodeStyle, dialogueSelectedNodeStyle);
            Response response;
            EditorConnector connector;
            node.conversation = conversation;
            node.entryID = entry.ID;
            node.entry = entry;
            SetupNodeActions(node);
            nodes.Add(node);
            
            // Setup response nodes for node
            for(int i = 0; i < entry.Responses.Count; ++i)
            {
                response = entry.Responses[i];
                ResponseEditorNode responseNode = new ResponseEditorNode(response.Position, NODE_WIDTH, NODE_HEIGHT, responseNodeStyle, responseSelectedNodeStyle);
                responseNode.conversation = conversation;
                responseNode.entryID = entry.ID;
                responseNode.index = i;
                responseNode.response = response;
                SetupNodeActions(responseNode);
                nodes.Add(responseNode);
                connector = new EditorConnector();
                connector.Parent = node;
                connector.Target = responseNode;
                node.Connections.Add(connector);
            }
            return node;
        }

        DialogueEntryEditorNode SetupNodeConnections(DialogueEntry entry)
        {
            // Find corresponding node
            DialogueEntryEditorNode startNode = FindDialogueNode(entry.ID);
            if (startNode != null)
            {
                foreach (TransitionOption o in entry.transitions.transitions)
                {
                    DialogueEntryEditorNode endNode = FindDialogueNode(o.transition.TargetID);
                    if (endNode != null)
                    {
                        EditorConnector connector = new EditorConnector();
                        connector.Parent = startNode;
                        connector.Target = endNode;
                        startNode.Connections.Add(connector);
                    }
                }

                for(int i = 0; i < entry.Responses.Count; ++i)
                {
                    Response response = entry.Responses[i];
                    ResponseEditorNode responseNode = FindResponseNode(entry.ID, i);
                    foreach(TransitionOption o in response.transitions.transitions)
                    {
                        DialogueEntryEditorNode endNode = FindDialogueNode(o.transition.TargetID);
                        if (endNode != null)
                        {
                            EditorConnector connector = new EditorConnector();
                            connector.Parent = responseNode;
                            connector.Target = endNode;
                            responseNode.Connections.Add(connector);
                        }
                    }
                }
            }
            return startNode;
        }
    }
}