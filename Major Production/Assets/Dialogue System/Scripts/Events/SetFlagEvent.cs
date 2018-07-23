using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dialogue
{
    /// <summary>
    /// Sets a flag on the indicated DialogueActor, or on the DialogueManager
    /// </summary>
    [CreateAssetMenu(menuName = "Dialogue/Event/Set Flag")]
    public class SetFlagEvent : DialogueEvent
    {

        public override void Execute(DialogueManager manager, string target, string parameters)
        {
            FieldManager fields;
            // If no target, target is manager
            if (string.IsNullOrEmpty(target))
            {
                fields = manager.fields;
            } else
            {
                try {
                    fields = manager.actors[target].fields;
                }
                catch (KeyNotFoundException)
                {
                    Debug.LogWarning("Actor '" + target + "' not found in dialogue manager");
                    return;
                }
            }
            fields.SetFlag(parameters);
        }
        
        public override string Describe(string target, string parameters)
        {
            string description = "Set flag '" + parameters + "'";
            if (!string.IsNullOrEmpty(target))
            {
                description += " on " + target;
            }
            return description;
        }
    }
}
