using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dialogue
{
	[CreateAssetMenu(menuName = "Dialogue/Event/Start Battle")]
	public class StartBattleEvent : DialogueEvent
	{
		public RPG.Encounter encounter;
		public string defaultScene = "Test Battle";

		public override string Describe(string target, string parameters)
		{
			return "Start battle";
		}

		public override void Execute(DialogueManager manager, string target, string parameters)
		{
			// TODO make parameters do something?
			// get scenename from there? Fight now vs later?
			FindObjectOfType<BattleManager>().StartBattle(defaultScene, encounter.InstantiateEnemyTeam().transform);
		}
	}
}