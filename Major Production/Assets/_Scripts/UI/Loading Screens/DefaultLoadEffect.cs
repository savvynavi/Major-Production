using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI {
	public class DefaultLoadEffect : LoadingEffect
	{
		[SerializeField] Text infoText;
		[SerializeField] Slider progressBar;
		[SerializeField] float fileProgressAmount = 0.5f;
		bool loadedFile = false;

		public override void BeginFileLoad()
		{
			progressBar.value = 0;
			infoText.text = "Reading Save File";
		}

		public override void BeginSceneLoad(string scene)
		{
			gameObject.SetActive(true);
			infoText.text = "Loading " + scene;
			if (loadedFile)
			{
				progressBar.value = fileProgressAmount;
			}
			else
			{
				progressBar.value = 0;
			}
		}

		public override void FinishFileLoad()
		{

			loadedFile = true;
			progressBar.value = fileProgressAmount;
		}

		public override void FinishSceneLoad()
		{
			loadedFile = false;
			gameObject.SetActive(false);
		}

		public override void LoadFailed()
		{
			gameObject.SetActive(false);
		}

		public override void SceneReady()
		{
			progressBar.value = 1;
		}

		public override void UpdateSceneProgress(float progress)
		{
			float totalValue;
			if (loadedFile)
			{
				totalValue = (progress + fileProgressAmount) / (fileProgressAmount + 0.9f);
			}
			else
			{
				totalValue = progress / 0.9f;
			}
			progressBar.value = totalValue;
		}

		// Use this for initialization
		void Start()
		{
			gameObject.SetActive(false);
		}

		// Update is called once per frame
		void Update()
		{

		}
	}
}