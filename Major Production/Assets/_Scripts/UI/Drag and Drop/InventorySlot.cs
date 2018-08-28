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

			if (draggedItem.itemBox is InventorySlot)
			{
				// If from inventory, swap positions
				InventorySlot theirSlot = (InventorySlot)draggedItem.itemBox;
				return GameController.Instance.inventory.SwapItems(ContainedItem, theirItem);
			} else if (draggedItem.itemBox is EquipmentSlot)
			{
				EquipmentSlot theirSlot = (EquipmentSlot)draggedItem.itemBox;
				// If from equipment, unequip item and insert before this
				return GameController.Instance.inventory.Unequip(theirItem, theirSlot.character, myIndex);
				// TODO need to make an InventoryBox class so you can just drop it in with an empty inventory
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
