using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace RPGItems {

	[CreateAssetMenu(fileName = "Item", menuName = "RPG/Item", order = 4)]
	public class Item : ScriptableObject {

		public string Name;
		public string Description;
		public Sprite Sprite;
		public ItemType Type;
		public List<RPGsys.Powers> Effects;

		public enum ItemType {
			Consumable,
			Equipable
		}
	}

}
