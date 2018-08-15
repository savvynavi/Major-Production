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
		public RPGsys.Powers Effect;

		public List<RPGsys.Buff> buffInstances;

		public enum ItemType {
			Consumable,
			Equipable
		}

		public void Initialize(RPGsys.Character target) {


			foreach(RPGsys.Buff buff in Effect.currentEffects) {

				RPGsys.Buff tmp = Instantiate(buff);
				tmp.Clone(target);
				buffInstances.Add(tmp);
			}
		}

		public void DeleteInstances(RPGsys.Character target) {
			foreach(RPGsys.Buff buff in Effect.currentEffects) {

				buffInstances.Clear();
				buff.Remove(target);
			}
		}

	}
}
