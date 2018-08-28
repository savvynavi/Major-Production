using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using RPGsys;

public class EquipmentBox : DragTarget, IPointerEnterHandler, IPointerExitHandler
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

	public void OnPointerEnter(PointerEventData eventData)
	{
	}

	public void OnPointerExit(PointerEventData eventData)
	{
	}

	protected override void OnHoverEnter(Draggable dragged)
	{
		//TODO
	}

	protected override void OnHoverLeave()
	{
		//TODO
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
