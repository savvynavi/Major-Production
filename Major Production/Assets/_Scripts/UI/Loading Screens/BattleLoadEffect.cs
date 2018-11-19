using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.UI
{
	public abstract class BattleLoadEffect : MonoBehaviour
	{
		// TODO figure out what this needs to know
		public abstract void BeginBattleLoad();

		public abstract void UpdateLoadProgress(float progress);

		public abstract void BattleSceneReady();

		public abstract void FinishBattleLoad();

		public bool IsActivationAllowed { get; protected set; }

		// TODO figure out this side of things with BattleManager
		public abstract void StartBattleOutro();

		public bool IsOutroDone { get; protected set; }
	} 
}