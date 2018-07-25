using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// TODO make more general: use a stack structure to keep track of scenes instead of assuming overword->dungeon->battle

public class SceneLoader : MonoBehaviour {

    Scene dungeonScene;
    Scene battleScene;

	// Use this for initialization
	void Start () {
        // HACK for testing, later dungeonScene will be loaded on reaching dungeon
        dungeonScene = SceneManager.GetActiveScene();
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

    public void LoadBattle(string sceneName)
    {
        StartCoroutine(AsyncBattleLoad(sceneName));
    }

    public void EndBattle()
    {
        SetSceneObjectActive(dungeonScene, true);
        SceneManager.SetActiveScene(dungeonScene);
        SceneManager.UnloadSceneAsync(battleScene);
    }

    IEnumerator AsyncBattleLoad(string sceneName)
    {
        AsyncOperation loadOp = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        // TODO do battle loading effects
        yield return new WaitUntil(() => { return loadOp.isDone; });
        // Deactivate objects in dungeon scene
        SetSceneObjectActive(dungeonScene, false);
        // Get Battle scene and set as active scene
        battleScene = SceneManager.GetSceneByName(sceneName);
        SceneManager.SetActiveScene(battleScene);
    }
}
