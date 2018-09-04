using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using RPG.Save;
using Newtonsoft.Json.Linq;

// TODO figure out if this needs to call anything on loading/unloading scenes

/// <summary>
/// This class manages loading and unloading scenes. It also stores data for PersistentObjects
/// in scenes
/// </summary>
public class SceneLoader : MonoBehaviour, ISaveable {

	public static SceneLoader Instance = null;

	Scene worldScene;
	Scene battleScene;

	public Dictionary<string, Dictionary<string, string>> persistentSceneData;

	public int EntrypointIndex { get; private set; }

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
		EntrypointIndex = entrypointIndex;
        StartCoroutine(AsyncSceneLoad(sceneName));
    }

	// Loads a battle scene, suspending the current WorldScene
    public void LoadBattle(string sceneName)
	{
		StartCoroutine(AsyncBattleLoad(sceneName));
	}

	IEnumerator AsyncBattleLoad(string sceneName)
	{
		AsyncOperation loadOp = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
		loadOp.allowSceneActivation = false;
		// TODO do battle loading effects
		yield return new WaitUntil(() => { return loadOp.progress >= 0.9f; });
		// Deactivate objects in world scene
		SetSceneObjectActive(worldScene, false);
		loadOp.allowSceneActivation = true;
		yield return new WaitUntil(() => { return loadOp.isDone; });
		battleScene = SceneManager.GetSceneByName(sceneName);
		SceneManager.SetActiveScene(battleScene);
		GameController.Instance.state = GameController.EGameStates.Battle;

		//TODO maybe some event/function called here letting battle initialize itself?
		Debug.Log(battleScene.name);
	}

	IEnumerator AsyncSceneLoad(string sceneName)
	{
		AsyncOperation loadOp = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
		//TODO tell last scene's SceneController it's ending so it can save changes?
		// TODO scene loading effects (maybe full load scene?)
		yield return new WaitUntil(() =>
		{
			return loadOp.isDone;
		});
		Scene newScene = SceneManager.GetSceneByName(sceneName);
		SceneManager.SetActiveScene(newScene);
		// TODO find newScene's SceneController, pass through info needed for initialization
		worldScene = newScene;
		Debug.Log(newScene.name);
	}

	// Unloads the BattleScene and reactivates the current World scene
	public void EndBattle()
    {
		SetSceneObjectActive(worldScene, true);
        SceneManager.SetActiveScene(worldScene);
		// TODO may need to tell scene it was just reactivated?

		SetSceneObjectActive(battleScene, false);
        SceneManager.UnloadSceneAsync(battleScene);
		GameController.Instance.state = GameController.EGameStates.Overworld;
	}

	public JObject Save()
	{
		return new JObject(
			new JProperty("scene", worldScene.name),
			new JProperty("entrypointIndex", EntrypointIndex),
			new JProperty("sceneData", new JObject(persistentSceneData)));
		// TODO check this works correctly
	}

	public void Load(JObject data)
	{
		persistentSceneData = data["sceneData"].ToObject<Dictionary<string, Dictionary<string, string>>>();
		EntrypointIndex = (int)data["entrypointIndex"];
		LoadScene((string)data["scene"]);
	}
}
