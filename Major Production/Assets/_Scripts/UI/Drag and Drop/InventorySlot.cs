using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPGItems;

public class InventorySlot : ItemBox {

	public override bool Drop(Draggable dragged)
	{
		// If item from another itembox, swap items
		DraggableItem draggedItem = (DraggableItem)dragged;
		if (draggedItem != null && draggedItem.itemBox != this)
		{
			List<Item> inventory = GameController.Instance.inventory.playerInventory;
			int myIndex = inventory.IndexOf(ContainedItem);
			Item theirItem = draggedItem.itemBox.ContainedItem;

			InventorySlot theirInvSlot = (InventorySlot)draggedItem.itemBox;
			EquipmentSlot theirEquipSlot = (EquipmentSlot)draggedItem.itemBox;

			if (theirInvSlot != null)
			{
				// If from inventory, swap positions
				int theirIndex = inventory.IndexOf(draggedItem.itemBox.ContainedItem);
				if (myIndex >= 0 && theirIndex >= 0)
				{
					theirInvSlot.ContainedItem = ContainedItem;
					inventory[theirIndex] = ContainedItem;
					ContainedItem = theirItem;
					inventory[myIndex] = theirItem;
					return true;
				}
				else
				{
					return false;
				}
			} else if (theirEquipSlot != null)
			{
				// If from equipment, unequip item and insert before this
				GameController.Instance.inventory.Unequip(theirItem, theirEquipSlot.character, myIndex);
				// TODO need to make an InventoryBox class so you can just drop it in with an empty inventory
				return true;
			} else
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
