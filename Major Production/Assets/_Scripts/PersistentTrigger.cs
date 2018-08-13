using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistentTrigger : PersistentObject {

	public bool Triggered;

	public override void Load(string data)
	{
		JsonUtility.FromJsonOverwrite(data, this);
	}
}
