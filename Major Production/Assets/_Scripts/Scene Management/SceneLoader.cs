using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using RPG.Save;
using Newtonsoft.Json.Linq;

// TODO Saving/Loading puts character back at entrypoint. Maybe think about having specific point to put character instead?

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

	public Dictionary<string, Dictionary<string, string>> persistentSceneData;

	public int EntrypointIndex { get; private set; }

	public ELoaderState State { get; private set; }

	[Header("Loading Events")]
	public StringEvent OnStartLoading;
	public FloatEvent OnLoadProgress;
	public UnityEvent OnLoadDone;

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
		persistentSceneData = new Dictionary<string, Dictionary<string, string>>();
		EntrypointIndex = -1;
	}

	// Use this for initialization
	void Start () {

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
		if(GameController.Instance.state == GameController.EGameStates.Menu)
		{
			State = ELoaderState.StartGame;
		} else
		{
			State = ELoaderState.SceneLoad;
		}
		EntrypointIndex = entrypointIndex;
        StartCoroutine(AsyncSceneLoad(sceneName));
    }

	// Loads a battle scene, suspending the current WorldScene
    public void LoadBattle(string sceneName)
	{
		State = ELoaderState.StartBattle;
		StartCoroutine(AsyncBattleLoad(sceneName));
	}

	IEnumerator AsyncBattleLoad(string sceneName)
	{
		AsyncOperation loadOp = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
		loadOp.allowSceneActivation = false;
		OnStartLoading.Invoke(sceneName);
		// TODO do battle loading effects
		while(loadOp.progress < 0.9f)
		{
			OnLoadProgress.Invoke(loadOp.progress);
			yield return new WaitForEndOfFrame();
		}
		// Deactivate objects in world scene
		SetSceneObjectActive(worldScene, false);
		loadOp.allowSceneActivation = true;
		yield return new WaitUntil(() => { return loadOp.isDone; });
		OnLoadDone.Invoke();
		battleScene = SceneManager.GetSceneByName(sceneName);
		SceneManager.SetActiveScene(battleScene);
		GameController.Instance.state = GameController.EGameStates.Battle;

		//TODO maybe some event/function called here letting battle initialize itself?
		Debug.Log(battleScene.name);
		State = ELoaderState.Idle;
	}

	IEnumerator AsyncSceneLoad(string sceneName)
	{
		AsyncOperation loadOp = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
		OnStartLoading.Invoke(sceneName);
		//TODO tell last scene's SceneController it's ending so it can save changes?
		// TODO scene loading effects (maybe full load scene?)
		while (!loadOp.isDone)
		{
			OnLoadProgress.Invoke(loadOp.progress);
			yield return new WaitForEndOfFrame();
		}
		OnLoadDone.Invoke();
		Scene newScene = SceneManager.GetSceneByName(sceneName);
		SceneManager.SetActiveScene(newScene);
		// TODO find newScene's SceneController, pass through info needed for initialization
		worldScene = newScene;
		Debug.Log(newScene.name);
		State = ELoaderState.Idle;
	}

	// Unloads the BattleScene and reactivates the current World scene
	public void EndBattle()
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

	public JObject Save()
	{
		return new JObject(
			new JProperty("scene", worldScene.name),
			new JProperty("entrypointIndex", EntrypointIndex),
			new JProperty("sceneData", JObject.FromObject(persistentSceneData)));
	}

	public void Load(JObject data)
	{
		persistentSceneData = data["sceneData"].ToObject<Dictionary<string, Dictionary<string, string>>>();
		LoadScene((string)data["scene"], (int)data["entrypointIndex"]);
	}

	public static bool DataValid(JObject data)
	{
		JToken nameToken;
		JToken entrypointToken;
		JToken sceneDataToken;
		if(data.TryGetValue("scene",out nameToken) &&
			data.TryGetValue("entrypointIndex",out entrypointToken) &&
			data.TryGetValue("sceneData",out sceneDataToken))
		{
			if(nameToken.Type == JTokenType.String &&
				entrypointToken.Type == JTokenType.String &&
				sceneDataToken.Type == JTokenType.Object)
			{
				return true;
			} else
			{
				return false;
			}

		}
		else
		{
			return false;
		}
	}
}
