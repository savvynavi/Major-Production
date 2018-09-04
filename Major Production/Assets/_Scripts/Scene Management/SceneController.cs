using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class placed in each scene, to correctly initialize player placement/persistent stuff
/// 
/// When started, this looks for a dictionary in SceneLoader with the scene's path. If found, 
/// it loads the data from that dictionary into the scene's PersistentObjects. Otherwise it 
/// creates that dictionary.
/// PersistentObjects are saved by this
/// </summary>
public class SceneController : MonoBehaviour {

	// TODO move player to correct entrypoint
	List<PersistentObject> persistentObjects;
	string sceneKey;

	// Maybe identify by names instead of index?
	[SerializeField] List<Transform> Entrypoints;

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

		// TODO move player to entrypoint set
		int entryIndex = SceneLoader.Instance.EntrypointIndex;
		if(entryIndex >=0 && entryIndex < Entrypoints.Count)
		{
			// HACK move player to indicated location
			Controller player = FindObjectOfType<Controller>();
			Transform entryPoint = Entrypoints[entryIndex];
			player.transform.position = entryPoint.position;
			player.transform.rotation = entryPoint.rotation;
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	// Saves PersistentObject to SceneLoader's dictionary
	public void SaveObject(PersistentObject po)
	{
		SceneLoader.Instance.persistentSceneData[sceneKey][po.ID] = po.Serialize();
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
