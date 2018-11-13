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
		public Dropdown skipSelect;
		public Button skipButton;
		public Dialogue.StringTextAssetDict savesDict;
		public Dictionary<string, TextAsset> skipSaves;
		[SerializeField] Button ContinueButton;

		private void Start()
		{
			// set up skip ui
			skipSaves = savesDict.ToDictionary();
			skipSelect.ClearOptions();
			List<Dropdown.OptionData> skipOptions = new List<Dropdown.OptionData>();
			foreach(KeyValuePair<string,TextAsset> skipEntry in skipSaves)
			{
				skipOptions.Add(new Dropdown.OptionData(skipEntry.Key));
			}
			skipSelect.AddOptions(skipOptions);
			if(skipSelect.options.Count > 0)
			{
				skipSelect.value = 0;
			}
			else
			{
				skipSelect.gameObject.SetActive(false);
				skipButton.gameObject.SetActive(false);
			}
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

		public void StartSkip()
		{
			TextAsset saveFile = skipSaves[skipSelect.options[skipSelect.value].text];
			GameController.Instance.LoadGame(saveFile);
		}
	}
}