using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI
{
	public class LoadScreen : MonoBehaviour
	{
		LoadingEffect selectedEffect;
		BattleLoadEffect battleEffect;
		[SerializeField] Text infoText;
		[SerializeField] Slider progressBar;
		public LoadingEffect defaultLoad;
		public LoadingEffect cutsceneLoad;
		public BattleLoadEffect defaultBattleEffect;

		// TODO loadscreen stuff for initial cutscene?

		public void SelectEffect(LoadingEffect effect)
		{
			selectedEffect = effect;
		}

		public void SelectBattleEffect(BattleLoadEffect effect)
		{
			battleEffect = effect;
		}

		// Use this for initialization
		void Start()
		{
			SelectEffect(defaultLoad);
			SelectBattleEffect(defaultBattleEffect);
		}

		public void BeginFileLoad()
		{
			selectedEffect.BeginFileLoad();
		}

		public void FinishFileLoad()
		{
			selectedEffect.FinishFileLoad();
		}

		public void BeginSceneLoad(string scene)
		{
			selectedEffect.BeginSceneLoad(scene);
		}

		public void UpdateProgress(float progress)
		{
			selectedEffect.UpdateSceneProgress(progress);
		}

		public void SceneReady()
		{
			selectedEffect.SceneReady();
		}

		public void FinishSceneLoad()
		{
			selectedEffect.FinishSceneLoad();
			// Go back to default effect
			SelectEffect(defaultLoad);
		}

		public void LoadFailed()
		{
			selectedEffect.LoadFailed();
			// Go back to default effect
			SelectEffect(defaultLoad);
		}

		public void BeginBattleLoad()
		{
			battleEffect.BeginBattleLoad();
		}

		public void UpdateBattleLoadProgress(float progress)
		{
			battleEffect.UpdateLoadProgress(progress);
		}

		public bool IsBattleReady()
		{
			return battleEffect.IsActivationAllowed;
		}

		public void BattleSceneReady()
		{
			battleEffect.BattleSceneReady();
		}

		public void FinishBattleLoad()
		{
			battleEffect.FinishBattleLoad();
		}

		public void BattleLoadFailed()
		{
			battleEffect.LoadFailed();
		}

		public void StartBattleOutro()
		{
			battleEffect.StartBattleOutro();
		}

		public void FinishBattleOutro()
		{
			battleEffect.FinishBattleOutro();
		}

		public bool BattleOutroDone()
		{
			return battleEffect.IsOutroDone;
		}
	}
}