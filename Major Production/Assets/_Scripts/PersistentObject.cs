using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PersistentObject : MonoBehaviour {
	public string Save()
	{
		return JsonUtility.ToJson(this, false);
	}

	public abstract void Load(string data);
}
