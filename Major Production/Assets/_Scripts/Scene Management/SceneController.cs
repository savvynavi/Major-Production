using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Newtonsoft.Json.Linq;

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

	public Controller player { get; private set; }

	// Maybe identify by names instead of index?
	[SerializeField] List<Transform> Entrypoints;

	public UnityEvent OnBusyStart;
	public UnityEvent OnBusyEnd;

	public bool Busy { get; private set; }


	// Use this for initialization
	// This is given custom execution order before default time so 
	// PersistentObjects are set before anything else initializes
	void Start () {
		player = FindObjectOfType<Controller>();

		OnBusyStart.AddListener(player.Freeze);
		OnBusyEnd.AddListener(player.Unfreeze);

		persistentObjects = new List<PersistentObject>(FindObjectsOfType<PersistentObject>());
		foreach(PersistentObject po in persistentObjects)
		{
			po.controller = this;
		}

		// If this scene has dictionary, load from it
		sceneKey = gameObject.scene.path;
		Dictionary<string, JObject> objectData;
		if (SceneLoader.Instance.persistentSceneData.TryGetValue(sceneKey, out objectData)){
			LoadPersistentObjects(objectData);
		} else
		{
			// If dictionary not found, create it
			SceneLoader.Instance.persistentSceneData[sceneKey] = new Dictionary<string, JObject>();
		}

		// TODO move player to entrypoint set
		int entryIndex = SceneLoader.Instance.EntrypointIndex;
		if(entryIndex >=0 && entryIndex < Entrypoints.Count)
		{
			// HACK move player to indicated location
			Transform entryPoint = Entrypoints[entryIndex];
			player.transform.position = entryPoint.position;
			player.transform.rotation = entryPoint.rotation;
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void SetBusy()
	{
		OnBusyStart.Invoke();
	}

	public void ClearBusy()
	{
		Busy = false;
		OnBusyEnd.Invoke();
	}

	// Saves PersistentObject to SceneLoader's dictionary
	public void SaveObject(PersistentObject po)
	{
		SceneLoader.Instance.persistentSceneData[sceneKey][po.ID] = po.ToJObject();
	} 

	public void LoadPersistentObjects(Dictionary<string,JObject> loadData)
	{
		foreach(PersistentObject po in persistentObjects)
		{
			JObject data;
			if(loadData.TryGetValue(po.ID, out data))
			{
				po.Load(data);
			}
		}
	}
}
