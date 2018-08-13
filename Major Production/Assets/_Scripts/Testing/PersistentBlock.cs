using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistentBlock : PersistentObject {

	public bool Triggered;
	Renderer renderer;

	public override void Load(string data)
	{
		JsonUtility.FromJsonOverwrite(data, this);
		if (Triggered)
		{
			ChangeColour();
		}
	}

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
}
