using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dialogue;

namespace RPG{
	[RequireComponent(typeof(DialogueActor))]
	[RequireComponent(typeof(PersistentTrigger))]
	public class Barricade : Interactable {

		DialogueManager dialogueManager;
		public PersistentTrigger trigger;
		public Conversation conversation;
		public DialogueActor actor;
		public BarricadeGuard guard;
		public GameObject barricade; // for testing, might change later
		// TODO persistent trigger to store being dropped

		// Use this for initialization
		void Start () {
			dialogueManager = FindObjectOfType<DialogueManager>();
			actor = GetComponent<DialogueActor>();
			trigger = GetComponent<PersistentTrigger>();
			if (trigger.Triggered)
			{
				SetBarricadeDown();
			}
		}
	
		// Update is called once per frame
		void Update () {
		
		}

		public override void Interact(InteractionUser user)
		{
			// Assign Speakers to my dialogue actors
			dialogueManager.actors["Barricade"] = actor;
			dialogueManager.actors["BarricadeGuard"] = guard.actor;
			dialogueManager.StartConversation(conversation);
		}

		public override void Hilight()
		{
			//TODO
		}

		public override void Unhilight()
		{
			//TODO
		}

		public void DropBarricade()
		{
			SetBarricadeDown();
			// Will do nicer thing with animation, trigger, etc later
			trigger.Triggered = true;
			trigger.Save();
			Debug.Log("Dropped barricade");
		}

		private void SetBarricadeDown()
		{
			barricade.SetActive(false);
			guard.gameObject.SetActive(false);
			this.GetComponent<BoxCollider>().enabled = false;
		}

	}
}