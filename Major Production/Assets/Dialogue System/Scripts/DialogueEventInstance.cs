using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dialogue
{
    // Event and data to perform the event for a dialogue entry
    [Serializable]
    public class DialogueEventInstance : ISerializationCallbackReceiver
    {
#if UNITY_EDITOR
        // Description to show in editor
        [HideInInspector]
        public string description;
#endif

        [SerializeField]
        string target;      // key to actor
        [SerializeField]
        string parameters;
        [SerializeField]
        DialogueEvent dialogueEvent;

		public bool IsRoutine { get { return dialogueEvent is DialogueRoutineEvent; } }

        public void Execute(DialogueManager manager)
        {
            dialogueEvent.Execute(manager, target, parameters);
        }

		public IEnumerator DoRoutine(DialogueManager manager)
		{
			if(IsRoutine)
			{
				yield return (dialogueEvent as DialogueRoutineEvent).DoRoutine(manager, target, parameters);
			}
		}

        // Update description of event
        public void OnAfterDeserialize()
        {
#if UNITY_EDITOR
            if(dialogueEvent != null)
            {
                description = dialogueEvent.Describe(target, parameters);
            }
#endif
        }

        public void OnBeforeSerialize()
        {

        }
    }
}
