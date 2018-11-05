﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using RPG;

public class NecromancerBattleTrigger : MonoBehaviour {

	[SerializeField] string battleScene;
	[SerializeField] RPG.Encounter encounter;
	[SerializeField] RPGsys.Character undeadWizardPrefab;

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			StartBossBattle();
		}
	}

	private void StartBossBattle()
	{
		UnityEvent battleEndEvent = new UnityEvent();
		battleEndEvent.AddListener(BossBattleEnd);
		BattleManager.Instance.StartBattle(battleScene, encounter,lossEvent:battleEndEvent);
	}

	private void BossBattleEnd()
	{
		//TODO heal mage, remove other party members, go to other scene
		Debug.Log("Boss battle finished");
		SceneLoader.Instance.LoadScene("Overworld Test"); // change this around

		// HACK figure out nicer way to do this, especially finding the wizard
		RPGsys.Character[] characters = GameController.Instance.Characters;
		foreach(RPGsys.Character character in characters)
		{
			GameObject.Destroy(character.gameObject);
		}
		// TODO change wizard portrait
		// empty inventory and remove all items
		GameController.Instance.inventory.Clear();
		Utility.InstantiateSameName<RPGsys.Character>(undeadWizardPrefab, GameController.Instance.playerTeam.transform);
	}
}
