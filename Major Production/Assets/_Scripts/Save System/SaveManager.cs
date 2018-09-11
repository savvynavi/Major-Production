using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;

namespace RPG.Save
{
	[Serializable]
	class SaveManager
	{
		[SerializeField] TextAsset saveSchemaFile;
		JSchema saveSchema;

		SaveManager()
		{

		}

		public void Init()
		{
			// todo move out to a coroutine?
			saveSchema = JSchema.Parse(saveSchemaFile.text);
		}

		public void SaveToFile(string filepath)
		{
			// TODO use a coroutine or other asynchronous operation to do this
			// TODO error for writing to file
			string saveData = GameController.Instance.Save().ToString();
			try
			{
				File.WriteAllText(filepath, saveData);
			}
			catch (IOException exception)
			{
				// probably need to do more than this when passing in a save file
				Debug.LogWarning("Save game exception:" + exception.Message);
			}
			catch (System.Security.SecurityException exception)
			{
				Debug.LogWarning("No permission to access save file:" + exception.Message);
			}
		}

		// TODO loading from specific file
		public void LoadFromFile(string filepath)
		{
			// TODO first check that file exists
			// TODO error handling (could read from file? Parsed as JSON? JToken is a JObject?
			JObject saveData = null;
			try
			{
				saveData = JObject.Parse(File.ReadAllText(filepath));
			}
			catch (FileNotFoundException exception)
			{
				// should probably check for file existing before enabling button too?
				Debug.LogWarning("Load file not found: " + exception.Message);
				return;
			}
			catch (IOException exception)
			{
				Debug.LogWarning("Load file IO Exception: " + exception.Message);
				return;
			}
			catch (System.Security.SecurityException exception)
			{
				Debug.LogWarning("No permission to access save file:" + exception.Message);
				return;
			}
			catch (Newtonsoft.Json.JsonReaderException exception)
			{
				Debug.LogWarning("Save file not valid JSON: " + exception.Message);
				return;
			}

			IList<string> validationErrors;
			if (saveData.IsValid(saveSchema, out validationErrors))
			{
				GameController.Instance.Load(saveData);
			} else
			{
				Debug.LogWarning("Save File had invalid json");
				foreach (string error in validationErrors)
				{
					Debug.LogWarning(error);
				}
				// might do more?
			}
		}
	}
}
