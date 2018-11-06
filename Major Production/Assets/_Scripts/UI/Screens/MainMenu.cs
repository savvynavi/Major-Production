using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI
{
	public class MainMenu : MonoBehaviour
	{

		public Dropdown sceneSelect;
		[SerializeField] Button ContinueButton;

		private void Start()
		{
			//HACK should probably have a SaveManager class knowing what last savefile was
			if(File.Exists(Application.persistentDataPath + "/savegame.json"))
			{
				ContinueButton.interactable = true;
			}
			else
			{
				ContinueButton.interactable = false;
			}
		}

		public void NewGame()
		{
			GameController.Instance.StartNewGame();
		}

		public void Play()
		{
			GameController.Instance.InitializeGame();
			SceneLoader.Instance.LoadScene(sceneSelect.options[sceneSelect.value].text);
		}

		public void Quit()
		{
			Application.Quit();
		}

		public void Continue()
		{
			GameController.Instance.LoadGame();
		}
	}
}