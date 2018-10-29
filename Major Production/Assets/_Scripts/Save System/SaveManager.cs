using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;

namespace RPG.Save
{
	[Serializable]
	public class SaveManager
	{
		// Asynchronously saves data to file
		public class SaveOperation : ThreadOperation
		{
			private string _filepath;
			private JObject _data;

			public SaveOperation(string filepath, JObject data)
			{
				_filepath = filepath;
				_data = data;
				thread = new Thread(WriteData);
				thread.Start();
			}

			void WriteData()
			{
				try
				{
					string serializedData = _data.ToString();
					File.WriteAllText(_filepath, serializedData);
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
		}

		// Asynchronously loads data from file and parses it as a JObject
		public class LoadOperation : ThreadOperation
		{
			private string _filepath;
			private JSchema _schema;
			public JObject data { get; private set; }

			public LoadOperation(string filepath, JSchema schema)
			{
				_filepath = filepath;
				data = null;
				_schema = schema;
				thread = new Thread(ReadData);
				thread.Start();
			}

			void ReadData()
			{
				JObject saveData = null;
				try
				{
					saveData = JObject.Parse(File.ReadAllText(_filepath));
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
				if (saveData.IsValid(_schema, out validationErrors))
				{
					data = saveData;
				}
				else
				{
					Debug.LogWarning("Save File had invalid json");
					foreach (string error in validationErrors)
					{
						Debug.LogWarning(error);
					}
				}
			}
		}

		[SerializeField] TextAsset saveSchemaFile;
		JSchema saveSchema;
		// Events for UI to subscribe to
		public UnityEvent OnStartSave;
		public UnityEvent OnFinishSave;
		public UnityEvent OnStartLoad;
		public UnityEvent OnFinishLoad;

		SaveManager()
		{

		}

		public void Init()
		{
			// This takes some time but it's on startup so no problem
			saveSchema = JSchema.Parse(saveSchemaFile.text);
		}

		public IEnumerator SaveToFile(string filepath)
		{
			SaveOperation saveOp;
			JObject saveData = GameController.Instance.Save();
			saveOp = new SaveOperation(filepath, saveData);
			OnStartSave.Invoke();
			yield return new WaitUntil(() => saveOp.IsDone);
			OnFinishSave.Invoke();
		}


		// loading from specific file
		public IEnumerator LoadFromFile(string filepath)
		{
			LoadOperation loadOp;
			loadOp = new LoadOperation(filepath, saveSchema);
			OnStartLoad.Invoke();
			yield return new WaitUntil(() => loadOp.IsDone);
			Debug.Log(loadOp.ToString());
			if (loadOp.data != null)
			{
				GameController.Instance.Load(loadOp.data);
			}
			OnFinishLoad.Invoke();
		}
	}
}
