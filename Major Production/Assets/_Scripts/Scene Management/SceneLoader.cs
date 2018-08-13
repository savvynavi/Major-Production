﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// TODO figure out if this needs to call anything on loading/unloading scenes
// TODO "horizontal" scenes (switching top scene instead of just push or pop
// TODO work out lifecycle (singleton? made on game start?)

public class SceneLoader : MonoBehaviour {

	public static SceneLoader Instance = null;

	Scene worldScene;
	Scene battleScene;

	Dictionary<string, Dictionary<string, string>> persistentSceneData;

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
		persistentSceneData = new Dictionary<string, Dictionary<string, string>>();
		worldScene = SceneManager.GetActiveScene();
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

    public void LoadScene(string sceneName)
    {
        StartCoroutine(AsyncSceneLoad(sceneName));
    }

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

	public void EndBattle()
    {
		SetSceneObjectActive(worldScene, true);
        SceneManager.SetActiveScene(worldScene);
		// TODO may need to tell scene it was just reactivated?

		SetSceneObjectActive(battleScene, false);
        SceneManager.UnloadSceneAsync(battleScene);
    }
}
