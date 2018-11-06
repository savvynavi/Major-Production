using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using RPG.Save;
using Newtonsoft.Json.Linq;

// TODO Saving/Loading puts character back at entrypoint. Maybe think about having specific point to put character instead?
// TODO write stuff for allowing operation activation

/// <summary>
/// This class manages loading and unloading scenes. It also stores data for PersistentObjects
/// in scenes
/// </summary>
public class SceneLoader : MonoBehaviour, ISaveable {

	public enum ELoaderState
	{
		Idle,
		StartGame,
		SceneLoad,
		StartBattle,
		EndBattle
	}

	public static SceneLoader Instance = null;

	Scene worldScene;
	Scene battleScene;

	public SceneController currentSceneController { get; private set; }

	public Dictionary<string, Dictionary<string, JObject>> persistentSceneData;

	public int EntrypointIndex { get; private set; }

	public ELoaderState State { get; private set; }

	public RPG.UI.LoadScreen loadScreen { get { return GameController.Instance.loadScreen; } }

	public bool SceneReady { get
		{
			if(loadOp != null)
			{
				return !(loadOp.progress < 0.9f);
			} else
			{
				return false;
			}
		}
	}

	public float SceneLoadProgress { get
		{
			if(loadOp != null)
			{
				return loadOp.progress;
			}
			else
			{
				return 0;
			}
		} }

	AsyncOperation loadOp;
	bool blockSceneActivation;
	private void Awake()
	{
		if(Instance == null)
		{
			Instance = this;
		} else if (Instance != this)
		{
			Destroy(gameObject);
		}
		GameObject.DontDestroyOnLoad(this.gameObject);
		worldScene = SceneManager.GetActiveScene();
		State = ELoaderState.Idle;
		Init();
	}

	public void Init()
	{
		persistentSceneData = new Dictionary<string, Dictionary<string, JObject>>();
		EntrypointIndex = -1;
	}

	// Use this for initialization
	void Start () {
		currentSceneController = FindObjectOfType<SceneController>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    // Activates or deactivates root objects in scene not tagged "Don't Suspend"
    void SetSceneObjectActive(Scene scene, bool value)
    {
        int rootCount = scene.rootCount;
        GameObject[] rootObjects = scene.GetRootGameObjects();
        foreach(GameObject o in rootObjects)
        {
            if(!o.CompareTag("Don't Suspend"))
            {
                o.SetActive(value);
            }
        }
    }

	public void LoadScene(string sceneName, int entrypointIndex = -1)
	{
		if (State == ELoaderState.Idle)
		{
			if (GameController.Instance.state == GameController.EGameStates.Menu)
			{
				State = ELoaderState.StartGame;
			}
			else
			{
				State = ELoaderState.SceneLoad;
			}
			EntrypointIndex = entrypointIndex;
			if (blockSceneActivation)
			{
				StartCoroutine(WaitingAsyncSceneLoad(sceneName));
			}
			else
			{
				StartCoroutine(AsyncSceneLoad(sceneName));
			}
		}
	}
	// Loads a battle scene, suspending the current WorldScene
	public void LoadBattle(string sceneName)
	{
		if (State == ELoaderState.Idle)
		{
			State = ELoaderState.StartBattle;
			StartCoroutine(AsyncBattleLoad(sceneName));
		}

	}
	IEnumerator AsyncBattleLoad(string sceneName)
	{
		loadOp = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
		loadOp.allowSceneActivation = false;
		loadScreen.BeginSceneLoad(sceneName);
		// TODO do battle loading effects
		while(loadOp.progress < 0.9f)
		{
			loadScreen.UpdateProgress(loadOp.progress);
			yield return new WaitForEndOfFrame();
		}
		loadScreen.SceneReady();
		// Deactivate objects in world scene
		SetSceneObjectActive(worldScene, false);
		loadOp.allowSceneActivation = true;
		yield return new WaitUntil(() => { return loadOp.isDone; });
		loadScreen.FinishSceneLoad();
		battleScene = SceneManager.GetSceneByName(sceneName);
		SceneManager.SetActiveScene(battleScene);
		GameController.Instance.state = GameController.EGameStates.Battle;

		//TODO maybe some event/function called here letting battle initialize itself?
		Debug.Log(battleScene.name);
		State = ELoaderState.Idle;
		loadOp = null;
	}

	IEnumerator AsyncSceneLoad(string sceneName)
	{
		loadOp = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
		loadScreen.BeginSceneLoad(sceneName);
		//TODO tell last scene's SceneController it's ending so it can save changes?
		// TODO scene loading effects (maybe full load scene?)
		while (!loadOp.isDone)
		{
			loadScreen.UpdateProgress(loadOp.progress);
			yield return new WaitForEndOfFrame();
		}
		loadScreen.SceneReady();
		loadScreen.FinishSceneLoad();
		Scene newScene = SceneManager.GetSceneByName(sceneName);
		SceneManager.SetActiveScene(newScene);
		currentSceneController = FindObjectOfType<SceneController>();
		worldScene = newScene;
		Debug.Log(newScene.name);
		State = ELoaderState.Idle;
		loadOp = null;
	}

	IEnumerator WaitingAsyncSceneLoad(string sceneName)
	{
		loadOp = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
		loadOp.allowSceneActivation = false;
		loadScreen.BeginSceneLoad(sceneName);
		while(loadOp.progress < 0.9f)
		{
			loadScreen.UpdateProgress(loadOp.progress);
			yield return new WaitForEndOfFrame();
		}
		loadScreen.SceneReady();
		yield return new WaitUntil(() => { return loadOp.isDone; });

		// HACK extract following out (or otherwise refactor)
		loadScreen.FinishSceneLoad();
		Scene newScene = SceneManager.GetSceneByName(sceneName);
		SceneManager.SetActiveScene(newScene);
		currentSceneController = FindObjectOfType<SceneController>();
		worldScene = newScene;
		Debug.Log(newScene.name);
		State = ELoaderState.Idle;
		GameController.Instance.state = GameController.EGameStates.Overworld;
		loadOp = null;
	}

	// Unloads the BattleScene and reactivates the current World scene
	public void EndBattle()
	{
		if (State == ELoaderState.Idle)
		{
			State = ELoaderState.EndBattle;
			SetSceneObjectActive(worldScene, true);
			SceneManager.SetActiveScene(worldScene);
			// TODO may need to tell scene it was just reactivated?

			SetSceneObjectActive(battleScene, false);
			SceneManager.UnloadSceneAsync(battleScene);
			GameController.Instance.state = GameController.EGameStates.Overworld;
			State = ELoaderState.Idle;
		}
	}

	public void BlockNextSceneActivation()
	{
		blockSceneActivation = true;
	}

	public void AllowSceneActivation()
	{
		blockSceneActivation = false;
		if(loadOp != null)
		{
			loadOp.allowSceneActivation = true;
		}
	}

	#region ISaveable Implementation
	public JObject Save()
	{
        JObject sceneDataObject = new JObject();
        foreach (KeyValuePair<string,Dictionary<string,JObject>> sceneData in persistentSceneData)
        {
            JObject sceneProperty = new JObject();
            foreach(KeyValuePair<string,JObject> objectData in sceneData.Value)
            {
                sceneProperty.Add(objectData.Key, objectData.Value);
            }
            sceneDataObject.Add(sceneData.Key, sceneProperty);
        }
		return new JObject(
			new JProperty("scene", worldScene.name),
			new JProperty("entrypointIndex", EntrypointIndex),
			new JProperty("sceneData", sceneDataObject));	// TODO check this works right
	}

	public void Load(JObject data)
	{
		persistentSceneData = new Dictionary<string, Dictionary<string, JObject>>();
		foreach(KeyValuePair<string, JToken> sceneProperty in data["sceneData"] as JObject)
		{
			Dictionary<string, JObject> sceneData = new Dictionary<string, JObject>();
			foreach(KeyValuePair<string, JToken> objectProperty in sceneProperty.Value as JObject)
			{
				sceneData.Add(objectProperty.Key, objectProperty.Value as JObject);
			}
			persistentSceneData.Add(sceneProperty.Key, sceneData);
		}
		LoadScene((string)data["scene"], (int)data["entrypointIndex"]);
	}
	#endregion
}
