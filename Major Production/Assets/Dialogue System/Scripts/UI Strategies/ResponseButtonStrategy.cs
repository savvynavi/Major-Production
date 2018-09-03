using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Dialogue
{
    public class ResponseButtonStrategy : UIDisplayStrategy
    {

        [SerializeField]
        Text nameText;
        [SerializeField]
        Image portraitImage;

        [SerializeField]
        RectTransform dialoguePanel;
        [SerializeField]
        Text dialogueText;

        [SerializeField]
        RectTransform responsePanel;

        [SerializeField]
        Button NextButton;

        [SerializeField]
        GameObject ButtonPrototype;
        [SerializeField]
        float buttonSpacing;
        [SerializeField]
        float buttonPadding = 20f;
        [SerializeField]
        float buttonBottomMargin = 20f;

        int numResponses = 0;
        float maxResponseWidth;

        private void Awake()
        {
            uiManager = gameObject.GetComponent<DialogueUI>();
            dialoguePanel.gameObject.SetActive(false);
        }

        private void Start()
        {
            NextButton.onClick.AddListener(uiManager.manager.NextEntry);
        }

        public override void ClearResponses()
        {
            numResponses = 0;
            maxResponseWidth = ButtonPrototype.GetComponent<RectTransform>().rect.width;
            foreach(Transform child in responsePanel)
            {
                GameObject.Destroy(child.gameObject);
            }

            NextButton.gameObject.SetActive(true);
            NextButton.interactable = true;
        }

        public override void DisplayDialogueEntry(DialogueEntry entry)
        {
            nameText.text = uiManager.manager.GetCurrentActor().Name;
            portraitImage.sprite = uiManager.manager.GetCurrentActor().Portrait;

            // Dialogue panel
			// TODO parse entry text and evaluate result
            dialogueText.text = entry.Text;

            uiManager.manager.cutsceneManager.DoCutsceneEvents(entry.cutsceneEvents);
        }

        public override void DisplayResponse(Response response, int ID, bool possible)
        {
            if (possible)
            {
                // Disable Next button
                NextButton.gameObject.SetActive(false);
                NextButton.interactable = false;

                GameObject buttonClone = GameObject.Instantiate(ButtonPrototype, responsePanel);
                RectTransform buttonTransform = buttonClone.GetComponent<RectTransform>();
                Button buttonScript = buttonClone.GetComponent<Button>();
                Text buttonText = buttonClone.GetComponentInChildren<Text>();
                Vector2 buttonPos = buttonTransform.anchoredPosition;

                // Place button at correct position
                buttonPos.y = numResponses * -(buttonTransform.rect.height + buttonSpacing);
                buttonTransform.anchoredPosition = buttonPos;

				// TODO parse response text and evaluate result
                buttonText.text = response.Text;

                buttonScript.onClick.AddListener(() => uiManager.manager.ResponseSelected(ID));

                // Calculate width to determine widest button needed
                TextGenerator textGen = new TextGenerator();
                TextGenerationSettings genSettings = buttonText.GetGenerationSettings(buttonTransform.rect.size);
                float buttonWidth = textGen.GetPreferredWidth(buttonText.text, genSettings) + buttonPadding;
                maxResponseWidth = Mathf.Max(maxResponseWidth, buttonWidth);

                numResponses++;
            }
        }

        public override void FinishedDisplayResponses()
        {

            // Get size needed for buttons if no response panel
            Vector2 buttonAreaSize = NextButton.GetComponent<RectTransform>().rect.size + new Vector2(20, 20);

            if(numResponses > 0)
            {
                foreach(RectTransform button in responsePanel.GetComponentsInChildren<RectTransform>())
                {
                    button.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal,maxResponseWidth);
                }

                responsePanel.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, maxResponseWidth);
                responsePanel.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, buttonBottomMargin + numResponses * (ButtonPrototype.GetComponent<RectTransform>().rect.height + buttonSpacing));

                buttonAreaSize = responsePanel.rect.size;
            }
            

            // Move dialogue text down by padding
            Vector2 dialoguePos = dialogueText.GetComponent<RectTransform>().anchoredPosition;
            dialogueText.GetComponent<RectTransform>().anchoredPosition = dialoguePos;

        }

        public override void OnConversationEnd()
        {
            dialoguePanel.gameObject.SetActive(false);
        }

        public override void OnConversationStart()
        {
            dialoguePanel.gameObject.SetActive(true);
        }
    }
}
