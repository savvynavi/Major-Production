using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using RPG.Save;
using Newtonsoft.Json.Linq;

namespace RPGItems {

	[CreateAssetMenu(fileName = "Item", menuName = "RPG/Item", order = 4)]
	public class Item : ScriptableObject {

		public string Name;
		public string Description;
		public Sprite Sprite;
		public ItemType Type;
		public RPGsys.Powers Effect;

		public List<RPGsys.Buff> buffInstances;

		public enum ItemType {
			Consumable,
			Equipable
		}
		
		//initializes an item and then stores all instances in a list so they can be accessed later
		public void Initialize(RPGsys.Character target) {


			foreach(RPGsys.Buff buff in Effect.currentEffects) {

				RPGsys.Buff tmp = Instantiate(buff);
				tmp.Clone(target);
				buffInstances.Add(tmp);
			}
		}

		//clears all applied buffs from item from character as well as all particle effects
		public void DeleteInstances(RPGsys.Character target) {
			foreach(RPGsys.Buff buff in buffInstances) {
				buff.Remove(target);
			}

			buffInstances.Clear();
		}
	}
}
