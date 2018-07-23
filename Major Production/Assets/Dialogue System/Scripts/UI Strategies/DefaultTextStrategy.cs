using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Dialogue
{
    // Copies lines given to text objects and gets response from keyboard input
    public class DefaultTextStrategy : UIDisplayStrategy
    {
        public Text dialogueText;
        public Text actorName;
        public Text responseText;
        public Image portraitImage;
        public RectTransform dialoguePanel;

        private void Awake()
        {
            uiManager = gameObject.GetComponent<DialogueUI>();
            dialoguePanel.gameObject.SetActive(false);
        }

        // Use this for initialization
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {
            DialogueEntry current = uiManager.manager.current;
            if (current != null && current.Responses.Count > 0)
            {
                for (int i = 0; i < current.Responses.Count; ++i)
                {
                    if (Input.GetKeyDown(i.ToString())) //Input.GetKeyDown("" + i)
                    {
                        uiManager.manager.ResponseSelected(i);
                    }
                }
            } else
            {
                if (Input.GetButtonDown("Submit"))
                {
                    uiManager.manager.NextEntry();
                }
            }
        }

        public override void DisplayDialogueEntry(DialogueEntry entry)
        {
            dialogueText.text = entry.Text;
            actorName.text = uiManager.manager.GetCurrentActor().Name;
            portraitImage.sprite = uiManager.manager.GetCurrentActor().Portrait;

            uiManager.manager.cutsceneManager.DoCutsceneEvents(entry.cutsceneEvents);
        }

        public override void DisplayResponse(Response response, int ID, bool possible)
        {
            if (possible)
            {
                responseText.text += ID.ToString() + ": " + response.Text + "\n";
            }
        }

        public override void ClearResponses()
        {
            responseText.text = "";
        }

        public override void OnConversationStart()
        {
            this.enabled = true;
            dialoguePanel.gameObject.SetActive(true);
        }

        public override void OnConversationEnd()
        {
            this.enabled = false;
            dialogueText.text = "";
            actorName.text = "";
            responseText.text = "";
            dialoguePanel.gameObject.SetActive(false);
        }
    }
}