using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI
{
	public class MainMenu : MonoBehaviour
	{

		public Dropdown sceneSelect;

		public void Play()
		{
			GameController.Instance.InitializeGame();
			SceneLoader.Instance.LoadScene(sceneSelect.options[sceneSelect.value].text);
		}

		public void Quit()
		{
			Application.Quit();
		}
	}
}