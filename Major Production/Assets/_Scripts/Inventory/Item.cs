using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using RPG.Save;
using Newtonsoft.Json.Linq;

namespace RPGItems {
	public abstract class Item : ScriptableObject {

		public string Name;
		public string Description;
		public Sprite Sprite;
		public RPGsys.Powers Effect;

		public List<RPGsys.Buff> buffInstances;

        private void OnEnable()
        {
            RPG.Save.Factory<Item>.Register(this);
        }

		public abstract bool Use(RPGsys.Character character);

		public abstract bool IsUsable(RPGsys.Character character);

		public void ApplyEffect(RPGsys.Character character) { Effect.Apply(character, this); }

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
