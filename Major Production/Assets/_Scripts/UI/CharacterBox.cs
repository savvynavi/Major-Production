using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using RPGsys;


public class CharacterBox : MonoBehaviour, IDragTarget {

	[SerializeField] Text nameText;
	Character character;
	
	public Character ContainedCharacter { get { return character; } set { SetCharacter(value); } }

	public bool Drop(Draggable dragged)
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

	public void Hover(Draggable dragged)
	{
		//TODO
	}

	void SetCharacter(Character c)
	{
		character = c;
		nameText.text = character.name;
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
