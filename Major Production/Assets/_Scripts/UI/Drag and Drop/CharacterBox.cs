using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using RPGsys;

namespace RPG.UI {
	// Dragtarget representing a character in the Inventory menu
	// Items dragged onto it uses the item on the character
	public class CharacterBox : DragTarget
	{

		[SerializeField] Text nameText;
		Character character;

		public Character ContainedCharacter { get { return character; } set { SetCharacter(value); } }

		// If item dropped, uses it on character
		public override bool Drop(Draggable dragged)
		{
			DraggableItem item = (DraggableItem)dragged;
			if (item != null)
			{
				// TODO check item is usable?
				return GameController.Instance.inventory.Use(item.itemBox.ContainedItem, ContainedCharacter);
			}
			else
			{
				return false;
			}
		}


		protected override void OnHoverEnter(Draggable dragged)
		{
			//TODO popup showing effect?
			DraggableItem item = (DraggableItem)dragged;
			if (item != null)
			{
				if (item.itemBox.ContainedItem.IsUsable(character))
				{
					//TODO show it's usable
				}
				else
				{
					// TODO show it's not usable
				}
			}
		}

		protected override void OnHoverLeave()
		{
			//TODO
		}

		void SetCharacter(Character c)
		{
			character = c;
			if (nameText != null)
			{
				nameText.text = character.name;
			}
		}

	}	
}
