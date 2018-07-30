using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// TODO figure out if this needs to call anything on loading/unloading scenes
// TODO work out lifecycle (singleton? made on game start?)

public class SceneLoader : MonoBehaviour {

    Stack<SceneFrame> scenes;

	// Use this for initialization
	void Start () {
        scenes = new Stack<SceneFrame>();

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

    public void LoadScene(string sceneName)
    {
        StartCoroutine(AsyncSceneLoad(sceneName));
    }

    

    IEnumerator AsyncSceneLoad(string sceneName)
    {
        AsyncOperation loadOp = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        loadOp.allowSceneActivation = false;
        // TODO do battle loading effects
        yield return new WaitUntil(() => { return loadOp.progress >= 0.9f; });
        // Deactivate objects in dungeon scene
        scenes.Peek().SetSceneObjectsActive(false);
        loadOp.allowSceneActivation = true;

        yield return new WaitUntil(() =>
        {
            return loadOp.isDone;
        });

        // Get Battle scene and set as active scene
        SceneFrame newFrame = new SceneFrame(SceneManager.GetSceneByName(sceneName));
        SceneManager.SetActiveScene(newFrame.ContainedScene);
        scenes.Push(newFrame);
    }

    public void EndScene()
    {
        SceneFrame lastScene = scenes.Pop();
        SceneFrame newScene = scenes.Peek();

        newScene.SetSceneObjectsActive(true);
        SceneManager.SetActiveScene(newScene.ContainedScene);

        lastScene.SetSceneObjectsActive(false);
        SceneManager.UnloadSceneAsync(lastScene.ContainedScene);
    }
}
