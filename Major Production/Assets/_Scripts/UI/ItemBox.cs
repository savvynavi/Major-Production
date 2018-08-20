using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using RPGItems;

public class ItemBox : MonoBehaviour {

	[SerializeField] Text inventoryText;
	Item item;
	public Item ContainedItem { get{ return item; } set{SetItem(value);} }

	void SetItem(RPGItems.Item _item)
	{
		item = _item;
		inventoryText.text = item.Name;
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
