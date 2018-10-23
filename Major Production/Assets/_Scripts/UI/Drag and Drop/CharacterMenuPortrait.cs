using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPGsys;
using RPGItems;

namespace RPG.UI
{
	[RequireComponent(typeof(CharacterUI))]
	public class CharacterMenuPortrait : DragTarget
	{
		protected Character character;
		public CharacterUI characterUI { get; private set; }

		public Character ContainedCharacter { get { return character; } set { SetCharacter(value); } }

		private void Awake()
		{
			characterUI = GetComponent<CharacterUI>();
		}

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
				Item containedItem = item.itemBox.ContainedItem;
				if (item.itemBox.ContainedItem.IsUsable(character))
				{
					StatDisplay.StatChangeData statChangeData = new StatDisplay.StatChangeData();
					statChangeData.ItemToUse = containedItem;
					statChangeData.ApplyItemEffects(containedItem);
					if (containedItem is Equipment)
					{
						if (((Equipment)containedItem).equipmentType == Equipment.EquipmentType.Weapon)
						{
							statChangeData.ApplyItemEffects(character.weapon.equippedItem, true);
						}
					}
					GetComponentInParent<CharacterScreen>().DisplayCharacter(statChangeData);
				}
				else
				{
					// TODO show it's not usable
				}
			}
		}

		protected override void OnHoverLeave()
		{
			GetComponentInParent<CharacterScreen>().DisplayCharacter();
		}

		public void SetCharacter(Character c)
		{
			character = c;
			characterUI.SetCharacter(c);
		}
	}
}