using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dialogue
{
    // An asset which can be used to define preconditions in conversations
    public class Condition : ScriptableObject
    {
        public bool Not;

        virtual public bool Evaluate(DialogueManager dialogue)
        {

            return true != Not;
        }

        // Describes the condition in text
        virtual public string Describe()
        {
            return (!Not).ToString();
        }
    }
}
