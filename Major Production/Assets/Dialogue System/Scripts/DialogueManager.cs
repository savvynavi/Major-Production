﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace Dialogue
{
    public class DialogueManager : MonoBehaviour
    {
        public DialogueUI UISystem;
        public CutsceneManager cutsceneManager;
        public Conversation conversation;
        [HideInInspector]
        public DialogueEntry current;
        
        public FieldManager fields;
        [SerializeField]
        private StringActorDict actorDictionary;
        public Dictionary<string,DialogueActor> actors;

        public UnityEvent OnConversationStart;
        public UnityEvent OnConversationEnd;

        private void Awake()
        {
            fields = new FieldManager();
            actors = actorDictionary.ToDictionary();
            UISystem.manager = this;
        }

        private void Reset()
        {
            fields = new FieldManager();
            actors = actorDictionary.ToDictionary();
        }

        public void StartConversation()
        {
            current = conversation.Start;
            UISystem.OnConversationStart();
            OnConversationStart.Invoke();
        }

        public void NextEntry()
        {
            bool entryFound = false;
            if (current != null)
            {
                if (!current.isEnd)
                {
                    Transition selectedTransition = current.transitions.SelectTransition(this);
                    if(selectedTransition != null){
                        DialogueEntry nextEntry = conversation.FindEntry(selectedTransition.TargetID);
                        if(nextEntry != null)
                        {
                            current = nextEntry;
                            foreach(DialogueEventInstance e in current.OnEnter)
                            {
                                e.Execute(this);
                            }
                            UISystem.SetDialogueEntry(current);
                            entryFound = true;
                        }
                    }
                    
                }
                if (!entryFound)
                {
                    EndConversation();
                }
            }
        }

        public void EndConversation()
        {
            current = null;
            UISystem.OnConversationEnd();
            OnConversationEnd.Invoke();
        }

        public void ResponseSelected(int id)
        {
            if (id >= 0 && id < current.Responses.Count)
            {
                Response response = current.Responses[id];
                if (response.CheckPrerequisite(this))
                {
                    foreach(DialogueEventInstance e in response.OnChosen)
                    {
                        e.Execute(this);
                    }
                    Transition selectedTransition = response.transitions.SelectTransition(this);

                    current = conversation.FindEntry(selectedTransition.TargetID);
                    UISystem.SetDialogueEntry(current);
                }
            }
        }

        public DialogueActor GetCurrentActor()
        {
            if(current != null)
            {
                return actors[current.Speaker];
            } else
            {
                return null;
            }
        }

        public void SetFlag(string flag)
        {
            fields.SetFlag(flag);
        }

        public void UnsetFlag(string flag)
        {
            fields.UnsetFlag(flag);
        }

        public bool CheckFlag(string flag)
        {
            return fields.CheckFlag(flag);
        }
    }
}
