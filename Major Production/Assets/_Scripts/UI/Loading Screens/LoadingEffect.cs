using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.UI
{
	public abstract class LoadingEffect : MonoBehaviour
	{
		public abstract void BeginFileLoad();

		public abstract void FinishFileLoad();

		public abstract void BeginSceneLoad(string scene);

		public abstract void UpdateSceneProgress(float progress);

		public abstract void SceneReady();

		public abstract void FinishSceneLoad();

		public abstract void LoadFailed();
	}
}