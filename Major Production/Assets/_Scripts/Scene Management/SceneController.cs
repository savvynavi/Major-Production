using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class placed in each scene, to correctly initialize player placement/persistent stuff
/// </summary>
public class SceneController : MonoBehaviour {

	// TODO move player to correct entrypoint
	// TODO change how data stored (just have it in SceneLoader, make this create the dictionary if path doesn't exist already)
	List<PersistentObject> persistentObjects;
	string sceneKey;

	private void Awake()
	{
	}

	// Use this for initialization
	void Start () {
		persistentObjects = new List<PersistentObject>(FindObjectsOfType<PersistentObject>());
		foreach(PersistentObject po in persistentObjects)
		{
			po.SetController(this);
		}

		// If this scene has dictionary, load from it
		sceneKey = gameObject.scene.path;
		Dictionary<string, string> objectData;
		if (SceneLoader.Instance.persistentSceneData.TryGetValue(sceneKey, out objectData)){
			LoadPersistentObjects(objectData);
		} else
		{
			// If dictionary not found, create it
			SceneLoader.Instance.persistentSceneData[sceneKey] = new Dictionary<string, string>();
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void SaveObject(PersistentObject po)
	{
		//HACK might change where it's stored
		SceneLoader.Instance.persistentSceneData[sceneKey][po.ID] = JsonUtility.ToJson(po);
	} 

	public void LoadPersistentObjects(Dictionary<string,string> loadData)
	{
		foreach(PersistentObject po in persistentObjects)
		{
			string data;
			if(loadData.TryGetValue(po.ID, out data))
			{
				po.Load(data);
			}
		}
	}
}
