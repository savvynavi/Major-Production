using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dialogue
{
    // An asset used to allow conversations to perform actions
    public abstract class DialogueEvent : ScriptableObject
    {
        // Performs the event's actions
        public abstract void Execute(DialogueManager manager, string target, string parameters);

        // Describes the event in text
        public abstract string Describe(string target, string parameters);
    }
}