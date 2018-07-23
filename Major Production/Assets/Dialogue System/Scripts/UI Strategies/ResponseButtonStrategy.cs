using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Dialogue
{
    public class ResponseButtonStrategy : UIDisplayStrategy
    {
        [SerializeField]
        RectTransform namePanel;
        float namePanelMinWidth;
        float namePanelMinHeight;
        [SerializeField]
        Text nameText;
        [SerializeField]
        float namePanelPadding = 20f;

        [SerializeField]
        RectTransform dialoguePanel;
        Vector2 dialoguePanelMinSize;
        [SerializeField]
        Text dialogueText;
        [SerializeField]
        float dialogueTopPadding;
        [SerializeField]
        float dialogueBottomPadding;

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
        }

        private void Start()
        {
            NextButton.onClick.AddListener(uiManager.manager.NextEntry);

            namePanelMinWidth = namePanel.rect.width;
            namePanelMinHeight = namePanel.rect.height;

            dialoguePanelMinSize = dialoguePanel.rect.size;
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

            

            // Dialogue panel
            dialogueText.text = Localize(entry.Text);

            uiManager.manager.cutsceneManager.DoCutsceneEvents(entry.cutsceneEvents);
        }

        public override void DisplayResponse(Response response, int ID, bool possible)
        {
            //TODO if necessary resize panel to fit new response
            //TODO spawn new button with action to select appropriate response
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

                buttonText.text = Localize(response.Text);

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
            // TODO resize panels to fit dialogue and buttons

            // Resize Name Panel
            TextGenerator textGen = new TextGenerator();
            TextGenerationSettings genSettings = nameText.GetGenerationSettings(new Vector2(namePanelMinWidth, namePanelMinHeight));
            float namePanelWidth = textGen.GetPreferredWidth(nameText.text, genSettings) + namePanelPadding;
            float namePanelHeight = textGen.GetPreferredHeight(nameText.text, genSettings);
            namePanel.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, Mathf.Max(namePanelMinWidth, namePanelWidth));
            namePanel.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, Mathf.Max(namePanelMinHeight, namePanelHeight));

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

            // TODO resize dialogue panel to fit both dialogue and the response panel (or next button)
            genSettings = dialogueText.GetGenerationSettings(dialoguePanelMinSize);
            float dialogueHeight = textGen.GetPreferredHeight(dialogueText.text, genSettings) + dialogueTopPadding + dialogueBottomPadding;

            // Move dialogue text down by padding
            Vector2 dialoguePos = dialogueText.GetComponent<RectTransform>().anchoredPosition;
            dialoguePos.y = -dialogueTopPadding;
            dialogueText.GetComponent<RectTransform>().anchoredPosition = dialoguePos;

            // Move response panel down by padding
            responsePanel.anchoredPosition = new Vector2(0,-dialogueHeight);

            // Calculate total size
            float totalHeight = Mathf.Max(dialogueHeight + buttonAreaSize.y, dialoguePanelMinSize.y);
            dialoguePanel.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, totalHeight);

        }

        public override void OnConversationEnd()
        {
            dialoguePanel.gameObject.SetActive(false);
        }

        public override void OnConversationStart()
        {
            dialoguePanel.gameObject.SetActive(true);
        }

        string Localize(string key)
        {
            return uiManager.localizer.GetLine(key);
        }
    }
}
