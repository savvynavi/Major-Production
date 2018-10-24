using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using UnityEngine;

public class PersistentBlock : PersistentObject {

	public bool Triggered;
	new Renderer renderer;

	// Use this for initialization
	void Awake () {
		renderer = GetComponent<Renderer>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void OnTriggerEnter(Collider other)
	{
		if (!Triggered && other.CompareTag("Player"))
		{
			ChangeColour();
			Triggered = true;
			Save();
		}
	}

	private void ChangeColour()
	{
		renderer.material.color = Color.red;
	}

	public override JObject ToJObject()
	{
		return new JObject(new JProperty("Triggered", Triggered));
	}

	public override void Load(JObject data)
	{
		Triggered = (bool)data["Triggered"];
		if (Triggered)
		{
			ChangeColour();
		}
	}
}
