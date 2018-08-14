using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RPGItems {
	//add safety list==null stuff 
	public class InventoryManager : MonoBehaviour {
		public List<Item> playerInventory;

		private void Start() {
			foreach(Item item in playerInventory) {
				item.Initialize();
			}
		}

		//add item to the inventory
		public void Add(Item item) {
			playerInventory.Add(item);
			SortByName();
		}

		//remove idem from the inventory
		public void Discard(Item item) {
			playerInventory.Remove(item);
		}

		public void DiscardStack(Item item) {
			//playerInventory.RemoveAll(playerInventory[] == item);
		}

		//delete entire inventory
		public void Clear() {
			playerInventory.Clear();
		}

		//sort the inventory by name
		public void SortByName() {
			List<Item> sortedList = playerInventory.OrderBy(o => o.name).ToList();
			playerInventory = sortedList;
		}


		//either use a potion or equip an item
		public void Use(Item item, RPGsys.Character character) {
			if(item != null && character != null && playerInventory.Count() > 0) {
				Debug.Log("defence of bard Before adding: " + character.Def);
				//if the item can be eaten, uses it and then discards from list, if equipable moves it to character list
				if(item.Type == Item.ItemType.Equipable) {
					character.Equipment.Add(item);
				}

				//foreach(RPGsys.Powers power in item.Effects) {
					item.Effect.Apply(character, item);
				//}
				Discard(item);
				Debug.Log("defence of bard after adding: " + character.Def);
			} else {
				Debug.Log("Inventory empty");
			}
		}

		public void Unequip(Item item, RPGsys.Character character) {
			if(character.Equipment.Count() > 0) {
				//foreach(RPGsys.Powers pow in item.Effects) {
					foreach(RPGsys.Buff buff in item.Effect.currentEffects) {
						buff.Remove(character);

						//character.currentEffects.Remove(buff);
					}
				//}

				character.Equipment.Remove(item);

				playerInventory.Add(item);

				Debug.Log("defence of bard after removal: " + character.Def);
			} else {
				Debug.Log("nothing to remove");
			}

		}
	}
}
