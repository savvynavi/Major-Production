﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI
{
	public class LoadScreen : MonoBehaviour
	{
		LoadingEffect selectedEffect;
		[SerializeField] Text infoText;
		[SerializeField] Slider progressBar;
		public DefaultLoadEffect defaultLoad;

		// TODO loadscreen stuff for initial cutscene?

		public void SelectEffect(LoadingEffect effect)
		{
			selectedEffect = effect;
		}

		// Use this for initialization
		void Start()
		{
			SelectEffect(defaultLoad);
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
		}

		public void LoadFailed()
		{
			selectedEffect.LoadFailed();
		}
	}
}