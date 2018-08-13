using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dialogue
{
    // Uses the DialogueBox class to show dialogue entries
    public class DialogueBoxStrategy : UIDisplayStrategy
    {
        [SerializeField] DialogueBox dialogueBox;
        int responseCount = 0;

        private void Awake()
        {
            uiManager = gameObject.GetComponent<DialogueUI>();
        }

        public override void ClearResponses()
        {
            dialogueBox.ClearButtons();
            responseCount = 0;
        }

        public override void DisplayDialogueEntry(DialogueEntry entry)
        {
            DialogueActor actor = uiManager.manager.GetCurrentActor();
            dialogueBox.SetTitle(actor.Name);
            dialogueBox.SetPortrait(actor.Portrait);
            dialogueBox.SetDialogue(entry.Text);
            uiManager.manager.cutsceneManager.DoCutsceneEvents(entry.cutsceneEvents);
        }

        public override void DisplayResponse(Response response, int ID, bool possible)
        {
            if (possible)
            {
                dialogueBox.AddButton(response.Text, () => uiManager.manager.ResponseSelected(ID));
                ++responseCount;
            }
        }

        public override void FinishedDisplayResponses()
        {
            // If no responses add Next button
            if(responseCount == 0)
            {
                dialogueBox.AddButton("Next", uiManager.manager.NextEntry);
            }
            // Resize elements to fit contents
            dialogueBox.RebuildLayout();
        }

        public override void OnConversationEnd()
        {
            dialogueBox.HideBox();
        }

        public override void OnConversationStart()
        {
            dialogueBox.ShowBox();
        }

        // Use this for initialization
        void Start()
        {
            dialogueBox.HideBox();
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}