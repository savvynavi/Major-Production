using System.Collections;
using System.Collections.Generic;
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
	}
}
