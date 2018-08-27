using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using RPGsys;

public class EquipmentBox : MonoBehaviour, IDragTarget, IPointerEnterHandler, IPointerExitHandler
{
	// TODO make items draggable from this back to inventory
	public Character character;

	public bool Drop(Draggable dragged)
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

	public void Hover(Draggable dragged)
	{
		//TODO make character screen show change from item
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		Debug.Log("Pointer entered");
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		Debug.Log("Pointer exited");
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
