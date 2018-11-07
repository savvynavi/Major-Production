using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dialogue
{
    [CreateAssetMenu(menuName = "Dialogue/Event/AutoloadEvent")]
    class AutoloadEvent : DialogueEvent
    {
        public override string Describe(string target, string parameters)
        {
            return "Autoloads last autosave";
        }

        public override void Execute(DialogueManager manager, string target, string parameters)
        {
            manager.EndConversation();
            GameController.Instance.Autoload();
        }
    }
}
