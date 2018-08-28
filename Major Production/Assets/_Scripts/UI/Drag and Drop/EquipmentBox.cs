using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using RPGsys;

public class EquipmentBox : DragTarget
{
	// TODO make items draggable from this back to inventory
	public Character character;


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
		//TODO
		if(dragged is DraggableItem)
		{
			DraggableItem di = (DraggableItem)dragged;
			if(di.itemBox is InventorySlot)
			{
				CharacterScreen.StatChangeData statChangeData = new CharacterScreen.StatChangeData();
				statChangeData.ItemToUse = di.itemBox.ContainedItem;
				statChangeData.ApplyItemEffects(di.itemBox.ContainedItem);
				//HACK
				GetComponentInParent<CharacterScreen>().DisplayCharacter(statChangeData);
			}
		}
	}

	protected override void OnHoverLeave()
	{
		//TODO
		//HACK
		GetComponentInParent<CharacterScreen>().DisplayCharacter();
	}
}
