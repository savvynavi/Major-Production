using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPGItems;

namespace RPG.UI
{
	public class InventorySlot : ItemBox
	{

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
				}
				else if (draggedItem.itemBox is EquipmentSlot)
				{
					EquipmentSlot theirSlot = (EquipmentSlot)draggedItem.itemBox;
					throw new System.NotImplementedException();
					// If from equipment, unequip item and insert before this

					//return GameController.Instance.inventory.Unequip(theirItem, theirSlot.character, myIndex);

					// TODO need to make an InventoryBox class so you can just drop it in with an empty inventory
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

		protected override void OnHoverEnter(Draggable dragged) {
			//If dragging equipment out, show effect from its removal
			//HACK checking if in character screen
			CharacterScreen screen = GetComponentInParent<CharacterScreen>();
			if(screen != null) {
				if(dragged is DraggableItem) {
					DraggableItem di = (DraggableItem)dragged;
					if(di.itemBox is EquipmentSlot) {
						StatDisplay.StatChangeData statChangeData = new StatDisplay.StatChangeData();
						statChangeData.ItemToUse = di.itemBox.ContainedItem;
						statChangeData.ApplyItemEffects(di.itemBox.ContainedItem, true);
						screen.DisplayCharacter(statChangeData);
					}
				}
			}
		}

		protected override void OnHoverLeave() {
			// reset character display if in character screen
			CharacterScreen screen = GetComponentInParent<CharacterScreen>();
			if(screen != null) {
				screen.DisplayCharacter();
			}
		}
	}
}