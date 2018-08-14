using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RPGItems {
	//[CreateAssetMenu(fileName = "Inventory", menuName = "RPG/Inventory", order = 5)]


		//add safety list==null stuff 
	public class InventoryManager : MonoBehaviour {
		public List<Item> playerInventory;

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
			if(item != null && character != null) {
				//if the item can be eaten, uses it and then discards from list, if equipable moves it to character list
				if(item.Type == Item.ItemType.Consumable) {
					foreach(RPGsys.Powers power in item.Effects) {
						power.Apply(character, item);
					}
				}

				Discard(item);
			}
		}
	}
}
