using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// TODO make more general: use a stack structure to keep track of scenes instead of assuming overword->dungeon->battle

public class SceneLoader : MonoBehaviour {

    // HACK for testing scene transition stuff
    Scene dungeonScene;
    Scene battleScene;

    Stack<SceneFrame> scenes;

	// Use this for initialization
	void Start () {
        scenes = new Stack<SceneFrame>();

        // HACK for testing, later dungeonScene will be loaded on reaching dungeon
        dungeonScene = SceneManager.GetActiveScene();

        scenes.Push(new SceneFrame(SceneManager.GetActiveScene()));
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

    //HACK for test
    public void LoadBattle(string sceneName)
    {
        StartCoroutine(AsyncBattleLoad(sceneName));
    }

    //HACK for testing
    public void EndBattle()
    {
        SetSceneObjectActive(dungeonScene, true);
        SceneManager.SetActiveScene(dungeonScene);
        SceneManager.UnloadSceneAsync(battleScene);
    }

    IEnumerator AsyncBattleLoad(string sceneName)
    {
        SetSceneObjectActive(dungeonScene, false);

        AsyncOperation loadOp = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        // TODO do battle loading effects
        yield return new WaitUntil(() => { return loadOp.isDone; });
        // Deactivate objects in dungeon scene
        
        // Get Battle scene and set as active scene
        battleScene = SceneManager.GetSceneByName(sceneName);
        SceneManager.SetActiveScene(battleScene);
    }

    IEnumerator AsyncSceneLoad(string sceneName)
    {
        AsyncOperation loadOp = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        // TODO do battle loading effects
        yield return new WaitUntil(() => { return loadOp.isDone; });
        // Deactivate objects in dungeon scene
        scenes.Peek().PushedOnto();
        // Get Battle scene and set as active scene
        SceneFrame newFrame = new SceneFrame(SceneManager.GetSceneByName(sceneName));
        newFrame.Pushed();
        scenes.Push(newFrame);
    }

    void EndScene()
    {
        SceneFrame lastScene = scenes.Pop();
        scenes.Peek().PoppedInto();
        lastScene.Popped();
    }
}
