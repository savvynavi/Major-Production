using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using RPGItems;

// Represents an item in the inventory screen. Contains draggable elements which can be used to apply
// the item to things
// May change around, making putting more item logic in the item itself?
public class ItemBox : MonoBehaviour
{

	[SerializeField] Text inventoryText;
	Item item;
	public DraggableItem draggable;
	public Item ContainedItem { get{ return item; } set{SetItem(value);} }


	

	void SetItem(RPGItems.Item _item)
	{
		item = _item;
		inventoryText.text = item.Name;
	}

	// Use this for initialization
	void Start () {
		draggable.container = this.transform;
		draggable.itemBox = this;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void OnDestroy()
	{
		GameObject.Destroy(draggable.gameObject);
	}
}
