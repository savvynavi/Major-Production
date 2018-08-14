using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dialogue
{
	[CreateAssetMenu(menuName = "Dialogue/Event/Start Battle")]
	public class StartBattleEvent : DialogueEvent
	{
		public GameObject enemies; // Prefab of enemies in battle
		public string defaultScene = "Test Battle";

		public override string Describe(string target, string parameters)
		{
			return "Start battle";
		}

		public override void Execute(DialogueManager manager, string target, string parameters)
		{
			// TODO make parameters do something?
			// get scenename from there? Fight now vs later?
			GameObject go = GameObject.Instantiate(enemies.gameObject);
			go.SetActive(false);
			FindObjectOfType<BattleManager>().StartBattle(defaultScene, go.transform);
		}
	}
}