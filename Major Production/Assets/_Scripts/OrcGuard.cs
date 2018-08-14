using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrcGuard : MonoBehaviour {

	Dialogue.DialogueManager dialogueManager;
	public Dialogue.Conversation conversation;
	bool triggered = false;

	// Use this for initialization
	void Start () {
		dialogueManager = FindObjectOfType<Dialogue.DialogueManager>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void OnTriggerEnter(Collider other)
	{
		if (!triggered)
		{
			if (other.CompareTag("Player"))
			{
				dialogueManager.StartConversation(conversation);
				triggered = true;
			}
		}
	}
}
