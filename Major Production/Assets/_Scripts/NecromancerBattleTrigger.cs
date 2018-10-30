using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using RPG;

public class NecromancerBattleTrigger : MonoBehaviour {

	[SerializeField] string battleScene;
	[SerializeField] RPG.Encounter encounter;

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
	}
}
