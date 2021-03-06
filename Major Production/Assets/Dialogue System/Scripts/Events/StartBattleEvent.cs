﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Dialogue
{
	[CreateAssetMenu(menuName = "Dialogue/Event/Start Battle")]
	public class StartBattleEvent : DialogueRoutineEvent
	{
		public RPG.Encounter encounter;
		public string defaultScene = "Test Battle";
        public bool continueOnLoss = false;

		public override string Describe(string target, string parameters)
		{
			return "Start battle";
		}

		public override IEnumerator DoRoutine(DialogueManager manager, string target, string parameters)
		{
            // TODO make parameters do something?
            // get scenename from there?
            if (continueOnLoss)
            {
                UnityEvent battleLossEvent = new UnityEvent();
                RPG.BattleManager.Instance.StartBattle(defaultScene, encounter, lossEvent: battleLossEvent);
            }
            else
            {
                RPG.BattleManager.Instance.StartBattle(defaultScene, encounter);
            }
            yield return new WaitWhile(() => { return RPG.BattleManager.InBattle; });
		}

		public override void Execute(DialogueManager manager, string target, string parameters)
		{

		}
	}
}