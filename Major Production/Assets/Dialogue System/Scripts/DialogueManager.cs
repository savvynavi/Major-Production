using System.Collections;
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

		bool m_inConversation = false;
		bool acceptingInput = true; // TODO maybe also have display disable buttons?

		public bool InConversation {get{return m_inConversation;}}

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

        public bool StartConversation()
        {
			if (!m_inConversation) {
				current = conversation.Start;
				UISystem.OnConversationStart ();
				OnConversationStart.Invoke ();
				m_inConversation = true;
				acceptingInput = true;
				return true;
			} else {
				return false;
			}
        }

		public bool StartConversation(Conversation con){
			if (!m_inConversation) {
				conversation = con;
				return StartConversation ();
			} else {
				return false;
			}
		}

		public void NextEntry()
		{
			if (acceptingInput)
			{
				StartCoroutine(NextEntryRoutine());
			}
		}

		public void ResponseSelected(int id)
		{
			if (acceptingInput)
			{
				StartCoroutine(ResponseSelectedRoutine(id));
			}
		}

        public void EndConversation()
        {
            current = null;
            UISystem.OnConversationEnd();
            OnConversationEnd.Invoke();
			m_inConversation = false;
        }

		private IEnumerator NextEntryRoutine()
		{
			bool entryFound = false;
			acceptingInput = false;
			if (current != null)
			{
				// Do onExit events and routines
				foreach (DialogueEventInstance e in current.OnExit)
				{
					e.Execute(this);
					if (e.IsRoutine)
					{
						yield return e.DoRoutine(this);
					}
				}
				if (!current.isEnd)
				{
					Transition selectedTransition = current.transitions.SelectTransition(this);
					if (selectedTransition != null)
					{
						DialogueEntry nextEntry = conversation.FindEntry(selectedTransition.TargetID);
						if (nextEntry != null)
						{
							current = nextEntry;
							// Do onEntry events and routines
							foreach (DialogueEventInstance e in current.OnEnter)
							{
								e.Execute(this);
								if (e.IsRoutine)
								{
									yield return e.DoRoutine(this);
								}
							}
							UISystem.SetDialogueEntry(current);
							entryFound = true;
						}
					}

				}
			}
			if (!entryFound)
			{
				EndConversation();
			}
			acceptingInput = true;
		}


		public IEnumerator ResponseSelectedRoutine(int id)
        {
			acceptingInput = false;
            if (id >= 0 && id < current.Responses.Count)
            {
                Response response = current.Responses[id];
                if (response.CheckPrerequisite(this))
                {
					// Do onExit events
					foreach(DialogueEventInstance e in current.OnExit)
					{
						e.Execute(this);
						if (e.IsRoutine)
						{
							yield return e.DoRoutine(this);
						}
					}
					// Do response events
                    foreach(DialogueEventInstance e in response.OnChosen)
                    {
						e.Execute(this);
						if (e.IsRoutine)
						{
							yield return e.DoRoutine(this);
						}
					}
                    Transition selectedTransition = response.transitions.SelectTransition(this);
					if (selectedTransition != null)
					{
						current = conversation.FindEntry(selectedTransition.TargetID);
						// do onEnter for current
						foreach (DialogueEventInstance e in current.OnEnter)
						{
							e.Execute(this);
							if (e.IsRoutine)
							{
								yield return e.DoRoutine(this);
							}
						}
						UISystem.SetDialogueEntry(current);
					} else
					{
						EndConversation();
					}
                }
            }
			acceptingInput = true;
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
