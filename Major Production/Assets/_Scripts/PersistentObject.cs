using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PersistentObject : MonoBehaviour {
	public string ID;   // HACK figure out if there's some good way to get unique but persistent ID
	SceneController controller;

	public void SetController(SceneController sc) { controller = sc; }

	public void Save()
	{
		controller.SaveObject(this);
	}

	public abstract void Load(string data);
}
