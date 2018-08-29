using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using RPGsys;

namespace RPG.UI {
	public class CharacterBox : DragTarget
	{

		[SerializeField] Text nameText;
		Character character;

		public Character ContainedCharacter { get { return character; } set { SetCharacter(value); } }

		public override bool Drop(Draggable dragged)
		{
			DraggableItem item = (DraggableItem)dragged;
			if (item != null)
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
}
