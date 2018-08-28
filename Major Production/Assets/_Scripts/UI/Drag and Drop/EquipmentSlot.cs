using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using RPGsys;
using RPGItems;


public class EquipmentSlot : ItemBox {

	public Character character;

	// TODO make slot have specific type of item it can be

	public override bool Drop(Draggable dragged)
	{
		// If item from another itembox, swap items
		// TODO either swap within equipment, or 

		DraggableItem draggedItem = (DraggableItem)dragged;
		if (draggedItem != null && draggedItem.itemBox != this)
		{
			List<Item> inventory = GameController.Instance.inventory.playerInventory;
			ItemBox theirBox = draggedItem.itemBox;
			Item theirItem = theirBox.ContainedItem;
			int myIndex = inventory.IndexOf(ContainedItem);
			int theirIndex = inventory.IndexOf(draggedItem.itemBox.ContainedItem);
			if (myIndex >= 0 && theirIndex >= 0)
			{
				theirBox.ContainedItem = ContainedItem;
				inventory[theirIndex] = ContainedItem;
				ContainedItem = theirItem;
				inventory[myIndex] = theirItem;
				return true;
			}
			else
			{
				return false;
			}
		}
		else
		{
			return false;
		}
	}

	protected override void OnHoverEnter(Draggable dragged)
	{
		//TODO
	}

	protected override void OnHoverLeave()
	{
		//TODO
	}
}
