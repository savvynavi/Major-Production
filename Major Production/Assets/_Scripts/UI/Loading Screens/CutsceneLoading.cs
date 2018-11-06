using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI {
	public class CutsceneLoading : LoadingEffect {
		// TODO show some actual effect
		public Text loadText;

		public override void BeginFileLoad()
		{
			gameObject.SetActive(true);
			loadText.text = "Loading...";
		}

		public override void BeginSceneLoad(string scene)
		{
			gameObject.SetActive(true);
			loadText.text = "Loading...";
		}

		public override void FinishFileLoad()
		{
		}

		public override void FinishSceneLoad()
		{
			gameObject.SetActive(false);
		}

		public override void LoadFailed()
		{
			gameObject.SetActive(false);
		}

		public override void SceneReady()
		{
			loadText.text = "Load Complete!";
		}

		public override void UpdateSceneProgress(float progress)
		{
		}

		// Use this for initialization
		void Awake() {
			gameObject.SetActive(false);
		}

	}
}