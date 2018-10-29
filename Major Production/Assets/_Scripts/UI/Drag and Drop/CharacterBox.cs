using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using RPGItems;
using RPGsys;

namespace RPG.UI {
	// Dragtarget representing a character in the Inventory menu
	// Items dragged onto it uses the item on the character
	public class CharacterBox : DragTarget
	{
		[SerializeField] CharacterUI characterPortrait;
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
				Item containedItem = item.itemBox.ContainedItem;
				if (containedItem.IsUsable(character))
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

					ResetHealBars();

					if (statChangeData != null)
					{
						float hpChange;
						float mpChange;
						if (statChangeData.changes.TryGetValue(RPGStats.Stats.Hp, out hpChange))
						{
							characterPortrait.HPRegenBar.Init(hpChange + character.Hp, character.hpStat);
						}
						if (statChangeData.changes.TryGetValue(RPGStats.Stats.Mp, out mpChange))
						{
							characterPortrait.MPRegenBar.Init(mpChange + character.Mp, character.mpStat);
						}
					}
				}
				else
				{
					// TODO show it's not usable
				}
			}
		}

		protected override void OnHoverLeave()
		{
			ResetHealBars();
		}

		private void ResetHealBars()
		{
			characterPortrait.HPRegenBar.Init(character.Hp, character.hpStat);
			characterPortrait.MPRegenBar.Init(character.Mp, character.mpStat);
		}

		void SetCharacter(Character c)
		{
			character = c;
			if (characterPortrait != null)
			{
				characterPortrait.SetCharacter(c);
			}
		}

	}	
}
