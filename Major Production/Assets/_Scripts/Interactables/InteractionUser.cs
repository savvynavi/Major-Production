using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RPG
{
	[RequireComponent(typeof(Collider))]
	public class InteractionUser : MonoBehaviour
	{
		// TODO figure out nicer way to hook these up?
		public RectTransform ActionPopup;
		public Text ActionText;

		Interactable selected;

		public bool CanInteract = true;

		// Use this for initialization
		void Start()
		{
			ActionPopup.gameObject.SetActive(false);
			// Don't interact during dialogue
			Dialogue.DialogueManager dialogueManager = FindObjectOfType<Dialogue.DialogueManager>();
			if (dialogueManager != null)
			{
				dialogueManager.OnConversationStart.AddListener(() => { CanInteract = false; });
				dialogueManager.OnConversationEnd.AddListener(() => { CanInteract = true; });
			}
		}

		// Update is called once per frame
		void Update()
		{
			if(!GameController.Instance.Paused &&
				CanInteract &&
				selected != null &&
				Input.GetButtonDown("Interact"))
			{
				ActionPopup.gameObject.SetActive(false);
				selected.Interact(this);
			}
		}

		public void Select(Interactable interactable)
		{
			if (CanInteract)
			{
				selected = interactable;
				interactable.Hilight();

				ActionPopup.gameObject.SetActive(true);
				ActionText.text = "E: " + interactable.ActionDescription;
				LayoutRebuilder.MarkLayoutForRebuild(ActionPopup);
			}
		}
		public void Unselect(Interactable interactable)
		{
			selected = null;
			interactable.Unhilight();

			ActionPopup.gameObject.SetActive(false);
		}
	}
}