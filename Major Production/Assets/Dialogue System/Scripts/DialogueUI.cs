using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Dialogue {
    // Class to connect dialogue manager and the game's UI
    public class DialogueUI : MonoBehaviour
    {
        public DialogueManager manager;
        public CutsceneManager cutsceneManager;
        public UIDisplayStrategy displayStrategy;
        public LocalizationManager localizer;

        private void Awake()
        {
            if (displayStrategy == null)
            {
                displayStrategy = gameObject.GetComponent<UIDisplayStrategy>();
            }
        }
        
        // Called when the DialogueManager starts a conversation
        public void OnConversationStart()
        {
            displayStrategy.OnConversationStart();
            SetDialogueEntry(manager.current);
        }

        // Called when a new DialogueEntry is entered
        public void SetDialogueEntry(DialogueEntry entry)
        {
            displayStrategy.DisplayDialogueEntry(entry);
            displayStrategy.ClearResponses();
            if (entry.Responses.Count > 0)
            {
                for (int i = 0; i < entry.Responses.Count; ++i)
                {
                    Response response = entry.Responses[i];
                    displayStrategy.DisplayResponse(response, i, response.CheckPrerequisite(manager));
                }
            }
            // Inform display strategy all responses have been sent
            displayStrategy.FinishedDisplayResponses();
        }

        // Called when DialogueManager ends conversation
        public void OnConversationEnd()
        {
            displayStrategy.OnConversationEnd();
        }
    }
}
