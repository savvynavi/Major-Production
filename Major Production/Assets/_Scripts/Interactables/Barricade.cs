using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dialogue;

namespace RPG{
	[RequireComponent(typeof(DialogueActor))]
	public class Barricade : Interactable {

		DialogueManager dialogueManager;
		public Conversation conversation;
		public DialogueActor actor;
		public DialogueActor guardActor;
		public GameObject barricade; // for testing, might change later
		// TODO persistent trigger to store being dropped

		// Use this for initialization
		void Start () {
			dialogueManager = FindObjectOfType<DialogueManager>();
			actor = GetComponent<DialogueActor>();
			//TODO decide whether to have DropBarricade done here or in editor
		}
	
		// Update is called once per frame
		void Update () {
		
		}

		public override void Interact(InteractionUser user)
		{
			// Assign Speakers to my dialogue actors
			dialogueManager.actors["Barricade"] = actor;
			dialogueManager.actors["BarricadeGuard"] = guardActor;
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
			// TODO
			barricade.SetActive(false);
			this.GetComponent<BoxCollider>().enabled = false;
			// Will do nicer thing with animation, trigger, etc later
			Debug.Log("Dropped barricade");
		}
	}
}