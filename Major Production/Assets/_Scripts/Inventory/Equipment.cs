using System.Collections;
using System.Collections.Generic;
using RPGsys;
using UnityEngine;

namespace RPGItems {
	[CreateAssetMenu(fileName = "Equipment", menuName = "RPG/Equipment", order = 5)]

	public class Equipment : Item {
		public GameObject WeaponModel;
		public EquipmentType equipmentType;
		public EClassType classRequirement;

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
						if (character.weapon.CanEquip(this))
						{
							// Remove previous weapon if it exists
							Equipment oldWeapon = character.weapon.Unequip();
							// Equip this weapon
							character.weapon.Equip(this);
							if(oldWeapon != null)
							{
								GameController.Instance.inventory.Add(oldWeapon);
							}
							return true;
						} else
						{
							return false;
						}
					case EquipmentType.Ring:
						return character.PlaceInEmptyRingSlot(this);
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
					// check if character's Weapon slot can equip
					return character.weapon.CanEquip(this);
				case EquipmentType.Ring:
					return character.HasEmptyRingSlot();
				default:
					throw new System.NotImplementedException();
			}
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
