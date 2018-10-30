using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json.Linq;

/// <summary>
/// Base class for components which have data saved between scene changes. Each PersistentObject
/// in a scene must have a unique ID as this is used as a key for its data in SceneLoader's dictionary.
/// 
/// Any public fields in derived classes will be serialized and saved to SceneLoader when Save is called.
/// When the scene loads again, those fields will be deserialized and copied into this object.
/// </summary>
public abstract class PersistentObject : MonoBehaviour {
	public string ID;   // HACK figure out if there's some good way to get unique but persistent ID
	public SceneController controller;

	public void Save()
	{
		controller.SaveObject(this);
	}

	// TODO consider making this return/take a JObject instead?
	public abstract JObject ToJObject();

	public abstract void Load(JObject data);
}
