using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.UI
{
	public class DefaultBattleLoad : BattleLoadEffect
	{
		bool battleReady;
		bool loadFinished;

		private void Awake()
		{
			gameObject.SetActive(false);
		}

		public override void BattleSceneReady()
		{
			battleReady = true;
		}

		public override void BeginBattleLoad()
		{
			// Turn on loading effects
			gameObject.SetActive(true);
			battleReady = false;
			IsActivationAllowed = false;
			// TODO further initialization

			StartCoroutine(BattleLoadRoutine());
		}

		IEnumerator BattleLoadRoutine()
		{
			// Load effect goes in

			// When it's done going in, allow activation (or maybe after it's ready)
			IsActivationAllowed = true;

			// wait until battle ready
			yield return new WaitUntil(() => { return this.battleReady; });

			// wait until load has finished
			yield return new WaitUntil(() => { return this.loadFinished; });

			// load effect goes out
			gameObject.SetActive(false);
		}

		public override void FinishBattleLoad()
		{
			loadFinished = true;
		}

		public override void LoadFailed()
		{
			gameObject.SetActive(false);
		}

		public override void StartBattleOutro()
		{
			gameObject.SetActive(true);
			IsOutroDone = false;
			StartCoroutine(OutroRoutine());
		}

		IEnumerator OutroRoutine()
		{
			// Do outro effect stuff
			yield return new WaitForEndOfFrame();
			IsOutroDone = true;
			gameObject.SetActive(false);
		}

		public override void UpdateLoadProgress(float progress)
		{
			// Don't need to display this
		}
	}
}