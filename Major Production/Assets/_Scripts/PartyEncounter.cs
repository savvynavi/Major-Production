using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PersistentTrigger))]
public class PartyEncounter : MonoBehaviour {

	Dialogue.DialogueManager dialogueManager;
	[SerializeField] string actorName;
	[SerializeField] Dialogue.DialogueActor actor;
	[SerializeField] Dialogue.Conversation encounterConversation;
	[SerializeField] RPGsys.Character partyMemberPrefab;
	PersistentTrigger encounterWon;
	bool triggered = false;

	private void Awake()
	{
		encounterWon = GetComponent<PersistentTrigger>();
	}
	// Use this for initialization
	void Start () {
		dialogueManager = FindObjectOfType<Dialogue.DialogueManager>();
		dialogueManager.actors[actorName] = actor;

		if (encounterWon.Triggered)
		{
			gameObject.SetActive(false);
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	private void OnTriggerEnter(Collider other)
	{
		if(!triggered && other.CompareTag("Player"))
		{
			dialogueManager.StartConversation(encounterConversation);
			triggered = true;
		}
	}

	public void BossBattleWin()
	{
		// make character join party
		Utility.InstantiateSameName<RPGsys.Character>(partyMemberPrefab, GameController.Instance.playerTeam.transform);
		encounterWon.Triggered = true;
		encounterWon.Save();
		// TODO some effect on character joining instead of just disappearing
		gameObject.SetActive(false);
	}
}
