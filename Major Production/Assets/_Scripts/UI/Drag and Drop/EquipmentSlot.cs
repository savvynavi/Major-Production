using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using RPGsys;
using RPGItems;

namespace RPG.UI {
	public class EquipmentSlot : ItemBox
	{

		public Character character;

		// TODO make slot have specific type of item it can be
		// TODO override tooltip text so that empty slot says valid type?

		public override bool Drop(Draggable dragged)
		{
			// If item from another itembox, swap items

			DraggableItem draggedItem = (DraggableItem)dragged;
			if (draggedItem != null && draggedItem.itemBox != this)
			{
				// TODO like with InventorySlot, check types and act accordingly
				int myIndex = character.Equipment.IndexOf(ContainedItem);
				Item theirItem = draggedItem.itemBox.ContainedItem;

				if (draggedItem.itemBox is InventorySlot)
				{
					// If from inventory unequip this and equip theirs
					// Note: both will end up at end, not bothering with that since
					// equipment slots will be changed later
					InventorySlot theirSlot = (InventorySlot)draggedItem.itemBox;
					if (theirItem.Type == Item.ItemType.Equipable)
					{
						character.Unequip(ContainedItem);
						character.UseItem(theirItem);
						return GameController.Instance.inventory.Replace(theirItem, ContainedItem);
					}
					else
					{
						return false;
					}
				}
				else if (draggedItem.itemBox is EquipmentSlot)
				{
					EquipmentSlot theirSlot = (EquipmentSlot)draggedItem.itemBox;
					Character theirCharacter = theirSlot.character;
					if (character == theirCharacter)
					{
						int theirIndex = character.Equipment.IndexOf(theirItem);
						if (myIndex >= 0 && theirIndex >= 0)
						{
							theirSlot.ContainedItem = ContainedItem;
							character.Equipment[theirIndex] = ContainedItem;
							ContainedItem = theirItem;
							character.Equipment[myIndex] = theirItem;
							return true;
						}
						else
						{
							return false;
						}
					}
					else
					{
						// order would change on reloading, but again don't care since temporary
						character.Unequip(ContainedItem);
						theirCharacter.Unequip(theirItem);
						character.UseItem(theirItem);
						theirCharacter.UseItem(ContainedItem);
						theirSlot.ContainedItem = ContainedItem;
						ContainedItem = theirItem;
						return true;
					}

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
			// Shows stat changes from removing this item and adding another
			if(dragged is DraggableItem)
			{
				DraggableItem di = (DraggableItem)dragged;
				if (di.itemBox is InventorySlot)
				{
					StatDisplay.StatChangeData statChangeData = new StatDisplay.StatChangeData();
					statChangeData.ItemToUse = di.itemBox.ContainedItem;
					statChangeData.ApplyItemEffects(di.itemBox.ContainedItem);
					// remove this item
					statChangeData.ApplyItemEffects(ContainedItem, true);

					GetComponentInParent<CharacterScreen>().DisplayCharacter(statChangeData);
				}
			}
		}

		protected override void OnHoverLeave()
		{
			// This causes a bug where the change isn't displayed for the box around it
			// will not fix for now, since we're changing how equipment works
			GetComponentInParent<CharacterScreen>().DisplayCharacter();
		}
	}
}
