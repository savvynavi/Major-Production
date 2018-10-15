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
						// TODO remove previous weapon if exists
						// TODO equip this weapon and apply it
					case EquipmentType.Ring:
						if (character.PlaceInEmptyRingSlot(this))
						{
							ApplyEffect(character);
							return true;
						} else
						{
							return false;
						}
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
			switch (equipmentType)
			{
				case EquipmentType.Weapon:
					// TODO check if character's Weapon slot can equip
					throw new System.NotImplementedException();
				case EquipmentType.Ring:
					return character.HasEmptyRingSlot();
				default:
					throw new System.NotImplementedException();
			}
			throw new System.NotImplementedException();
		}

		public void RemoveEffect(Character character)
		{
			foreach(Buff buff in Effect.currentEffects)
			{
				buff.EquipRemove(character, this);
			}
		}
	}
}
