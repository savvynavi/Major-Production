using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using RPGsys;
using RPGItems;

namespace RPG.UI {
	//TODO rename this class
	public class EquipmentSlotUI : ItemBox
	{

		public EquipmentSlot equipmentSlot { get; private set; }
		public void SetEquipmentSlot(EquipmentSlot slot)
		{
			equipmentSlot = slot;
			ContainedItem = slot.equippedItem;
		}
		

		public override string TooltipText
		{
			get
			{
				if(equipmentSlot != null)
				{
					if (equipmentSlot.IsEmpty)
					{
						return string.Format("{0} Slot", equipmentSlot.type.ToString("g"));
					} else
					{
						return equipmentSlot.equippedItem.Description;
					}
				}
				else
				{
					return null;
				}
			}
		}

		public override bool Drop(Draggable dragged)
		{
			// If item from another itembox, swap items

			DraggableItem draggedItem = (DraggableItem)dragged;
			if (draggedItem != null && draggedItem.itemBox != this)
			{
				// TODO like with InventorySlot, check types and act accordingly
				Equipment theirItem = (Equipment)draggedItem.itemBox.ContainedItem;
				if(theirItem == null)
				{
					return false;
				}

				if (draggedItem.itemBox is InventorySlot)
				{
					// If from inventory unequip this and equip theirs
					// Note: both will end up at end, not bothering with that since
					// equipment slots will be changed later
					if (equipmentSlot.CanEquip(theirItem))
					{
						Equipment myItem = equipmentSlot.Unequip();
						equipmentSlot.Equip(theirItem);
						if(myItem != null)
						{
							GameController.Instance.inventory.Replace(theirItem, myItem);
						}
						else
						{
							GameController.Instance.inventory.Discard(theirItem);
						}
						return true;
					}
					else
					{
						return false;
					}
				}
				else if (draggedItem.itemBox is EquipmentSlotUI)
				{
					// TODO if both rings, swap rings?
					EquipmentSlotUI otherUI = (EquipmentSlotUI)draggedItem.itemBox;
					EquipmentSlot theirSlot = otherUI.equipmentSlot;
					if(theirSlot.character == equipmentSlot.character)
					{
						if (equipmentSlot.SwapItem(theirSlot))
						{
							ContainedItem = equipmentSlot.equippedItem;
							otherUI.ContainedItem = theirSlot.equippedItem;
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
					if (equipmentSlot.CanEquip((Equipment)di.itemBox.ContainedItem))
					{
						StatDisplay.StatChangeData statChangeData = new StatDisplay.StatChangeData();
						statChangeData.ItemToUse = di.itemBox.ContainedItem;
						statChangeData.ApplyItemEffects(di.itemBox.ContainedItem);
						// remove this item
						statChangeData.ApplyItemEffects(ContainedItem, true);
						GetComponentInParent<CharacterScreen>().DisplayCharacter(statChangeData);
					} else
					{
						// TODO show item can't be equipped in this slot
					}
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
