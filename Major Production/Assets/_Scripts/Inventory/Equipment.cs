using System.Collections;
using System.Collections.Generic;
using RPGsys;
using UnityEngine;

namespace RPGItems {
	[CreateAssetMenu(fileName = "Equipment", menuName = "RPG/Equipment", order = 5)]

	public class Equipment : Item {
		public GameObject WeaponModel;
		public EquipmentType equipmentType;

		public enum EquipmentType {
			Weapon,
			Ring
		}

		public override bool Use(Character character)
		{
			bool usable = IsUsable(character);
			if (usable)
			{
				switch (equipmentType)
				{
					case EquipmentType.Weapon:
						throw new System.NotImplementedException();
						break;
					case EquipmentType.Ring:
						if (character.PlaceInEmptyRingSlot(this))
						{
							ApplyEffect(character);
							return true;
						} else
						{
							return false;
						}
						break;
					default:
						throw new System.NotImplementedException();
				}
			}
			return usable;
		}

		public override bool IsUsable(Character character)
		{
			// TODO check if class matches
			// TODO if ring check if slot available?
			throw new System.NotImplementedException();
		}
	}
}
