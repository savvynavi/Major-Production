using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base class for components which have data saved between scene changes. Each PersistentObject
/// in a scene must have a unique ID as this is used as a key for its data in SceneLoader's dictionary.
/// 
/// Any public fields in derived classes will be serialized and saved to SceneLoader when Save is called.
/// When the scene loads again, those fields will be deserialized and copied into this object.
/// </summary>
public abstract class PersistentObject : MonoBehaviour {
	public string ID;   // HACK figure out if there's some good way to get unique but persistent ID
	SceneController controller;

	public void SetController(SceneController sc) { controller = sc; }

	public void Save()
	{
		controller.SaveObject(this);
	}

	// TODO consider making this return/take a JObject instead?
	public string Serialize()
	{
		return JsonUtility.ToJson(this);
	}

	public virtual void Load(string data)
	{
		JsonUtility.FromJsonOverwrite(data, this);
	}
}
