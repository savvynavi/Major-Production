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
	Dictionary<string, string> objectData;

	private void Awake()
	{
		objectData = new Dictionary<string, string>();
	}

	// Use this for initialization
	void Start () {
		persistentObjects = new List<PersistentObject>(FindObjectsOfType<PersistentObject>());
		foreach(PersistentObject po in persistentObjects)
		{
			po.SetController(this);
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void SaveObject(PersistentObject po)
	{
		//HACK might change where it's stored
		objectData[po.ID] = JsonUtility.ToJson(po);
	} 

	public Dictionary<string,string> GetPersistentData()
	{
		return objectData;
	}

	public void LoadPersistentObjects(Dictionary<string,string> loadData)
	{
		// HACK might change where it's stored
		objectData = loadData;
		foreach(PersistentObject po in persistentObjects)
		{
			string data;
			if(objectData.TryGetValue(po.ID, out data))
			{
				po.Load(data);
			}
		}
	}
}
