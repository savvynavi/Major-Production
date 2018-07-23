using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConversationTrigger : MonoBehaviour {

    Dialogue.DialogueManager dialogueManager;
    [SerializeField] Dialogue.Conversation conversation;
    [SerializeField] Dialogue.StringActorDict actors;
    Dictionary<string, Dialogue.DialogueActor> m_actorDict;

    bool triggered;

	// Use this for initialization
	void Start () {
        dialogueManager = FindObjectOfType<Dialogue.DialogueManager>();
        m_actorDict = actors.ToDictionary();
        triggered = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    //HACK
    private void OnTriggerEnter(Collider other)
    {
        if(!triggered && other.GetComponent<CharacterController>() != null)
        {
            // Add this conversation's actors to manager
            foreach(KeyValuePair<string,Dialogue.DialogueActor> entry in m_actorDict)
            {
                dialogueManager.actors[entry.Key] = entry.Value;
            }
            dialogueManager.StartConversation(conversation);
            triggered = true;
        }
    }
}
