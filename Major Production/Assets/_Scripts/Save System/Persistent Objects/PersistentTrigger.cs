using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using UnityEngine;

public class PersistentTrigger : PersistentObject {

	public bool Triggered;

	public override void Load(JObject data)
	{
		Triggered = (bool)data["Triggered"];
	}

	public override JObject ToJObject()
	{
		return new JObject(new JProperty("Triggered", Triggered));
	}
}
