using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using RPG;


public class NecromancerBattleTrigger : MonoBehaviour {

	Dialogue.DialogueManager dialogueManager;
	[SerializeField] Dialogue.DialogueActor actor;
	[SerializeField] Dialogue.Conversation necromancerConversation;
	[SerializeField] RPGsys.Character undeadWizardPrefab;
	bool triggered = false;

	private void Start()
	{
		dialogueManager = FindObjectOfType<Dialogue.DialogueManager>();
		dialogueManager.actors["Necromancer"] = actor;
	}

	private void OnTriggerEnter(Collider other)
	{
		if (!triggered && other.CompareTag("Player"))
		{
			dialogueManager.StartConversation(necromancerConversation);
			triggered = true;
		}
	}

	public void BossBattleEnd()
	{
		//TODO heal mage, remove other party members, go to other scene
		//SceneLoader.Instance.LoadScene("03 Forest Path"); // change this around

		// HACK figure out nicer way to do this, especially finding the wizard
		RPGsys.Character[] characters = GameController.Instance.Characters;
		foreach(RPGsys.Character character in characters)
		{
			GameObject.Destroy(character.gameObject);
		}
		// TODO change wizard portrait
		// maybe totally replace wizard with undead wizard?
		// empty inventory and remove all items
		GameController.Instance.inventory.Clear();
		RPGsys.Character newMage = Utility.InstantiateSameName<RPGsys.Character>(undeadWizardPrefab);
        newMage.transform.SetParent(GameController.Instance.playerTeam.transform);
	}
}
