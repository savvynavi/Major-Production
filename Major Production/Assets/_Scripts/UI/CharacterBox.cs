using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using RPGsys;

public class CharacterBox : DragTarget, IPointerEnterHandler, IPointerExitHandler {

	[SerializeField] Text nameText;
	Character character;
	
	public Character ContainedCharacter { get { return character; } set { SetCharacter(value); } }

	public override bool Drop(Draggable dragged)
	{
		DraggableItem item = (DraggableItem)dragged;
		if(item != null)
		{
			// TODO check item is usable?
			GameController.Instance.inventory.Use(item.itemBox.ContainedItem, ContainedCharacter);
			return true;
		}
		else
		{
			return false;
		}
	}


	public void OnPointerEnter(PointerEventData eventData)
	{
		Debug.Log("Pointer entered");
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		Debug.Log("Pointer exited");
	}

	protected override void OnHoverEnter(Draggable dragged)
	{
		//TODO
	}

	protected override void OnHoverLeave()
	{
		//TODO
	}

	void SetCharacter(Character c)
	{
		character = c;
		nameText.text = character.name;
	}

	
}
