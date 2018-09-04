using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickupTest : MonoBehaviour {

	public RPGItems.Item item;

	// Use this for initialization
	void Start () {
		item = Instantiate(item);
	}


	private void OnTriggerEnter(Collider other) {
		if(item != null) {
			GameController.Instance.inventory.Add(item);
			item = null;
			Debug.Log("collision!");
		}
	}
}
