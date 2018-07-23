using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Dialogue
{
    [System.Serializable]
    public class Response
    {
        // Text to display for this response
        public string Text;
        // Events that occur when the response is choses
        public List<DialogueEventInstance> OnChosen = new List<DialogueEventInstance>();
        public Condition Prerequisite;
        public TransitionList transitions;

        [HideInInspector]
        public Vector2 Position;// For display in editor

        public bool CheckPrerequisite(DialogueManager manager)
        {
            if(Prerequisite == null)
            {
                return true;
            } else
            {
                return Prerequisite.Evaluate(manager);
            }
        }
    }

}