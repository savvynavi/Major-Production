using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickupTest : MonoBehaviour {

	public RPGItems.Item item;
	public RPGItems.InventoryManager manager;

	// Use this for initialization
	void Start () {
		item = Instantiate(item);
	}


	private void OnTriggerEnter(Collider other) {
		if(item != null) {
			manager.Add(item);
			item = null;
			Debug.Log("collision!");
		}
	}
}
