using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using RPGsys;

namespace RPG.UI{
	// Represents equipment list of a character
	public class EquipmentBox : DragTarget
	{
		public Character character;

		// Uses item on character
		public override bool Drop(Draggable dragged)
		{
			DraggableItem item = (DraggableItem)dragged;
			if (item != null)
			{
				// TODO check item is usable?
				GameController.Instance.inventory.Use(item.itemBox.ContainedItem, character);
				return true;
			}
			else
			{
				return false;
			}
		}

		protected override void OnHoverEnter(Draggable dragged)
		{
			// Shows changes from equipping item
			if(dragged is DraggableItem)
			{
				DraggableItem di = (DraggableItem)dragged;
				if(di.itemBox is InventorySlot)
				{
					StatDisplay.StatChangeData statChangeData = new StatDisplay.StatChangeData();
					statChangeData.ItemToUse = di.itemBox.ContainedItem;
					statChangeData.ApplyItemEffects(di.itemBox.ContainedItem);
					//HACK
					GetComponentInParent<CharacterScreen>().DisplayCharacter(statChangeData);
				}
			}
		}

		protected override void OnHoverLeave()
		{
			GetComponentInParent<CharacterScreen>().DisplayCharacter();
		}
	}
}