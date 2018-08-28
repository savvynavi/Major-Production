using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace RPGItems {
	//add safety list==null stuff 
	public class InventoryManager : MonoBehaviour {
		public List<Item> playerInventory;

		public UnityEvent OnInventoryChanged;

		private void Awake() {
			for(int i = 0; i < playerInventory.Count(); i++) {
				playerInventory[i] = Instantiate(playerInventory[i]);
			}
		}

		public void Initialize(List<Item> initialInventory)
		{
			playerInventory.Clear();
			foreach(Item item in initialInventory)
			{
				playerInventory.Add(Instantiate(item));
			}
			OnInventoryChanged.Invoke();
		}

		//add item to the inventory
		public void Add(Item item) {
			playerInventory.Add(item);
			OnInventoryChanged.Invoke();
		}

		//remove idem from the inventory
		public void Discard(Item item) {
			playerInventory.Remove(item);
			OnInventoryChanged.Invoke();
		}

		public void DiscardStack(Item item) {
			//playerInventory.RemoveAll(playerInventory[] == item);
		}

		//delete entire inventory
		public void Clear() {
			playerInventory.Clear();
			OnInventoryChanged.Invoke();
		}

		//sort the inventory by name
		public void SortByName() {
			List<Item> sortedList = playerInventory.OrderBy(o => o.name).ToList();
			playerInventory = sortedList;
			OnInventoryChanged.Invoke();
		}


		//yeah this whole sectoion is bad but works, refactor later (changes in basically all base stat/buff class)
		//either use a potion or equip an item
		public void Use(Item item, RPGsys.Character character) {
			if(item != null && character != null && playerInventory.Count() > 0) {
				Debug.Log("defence of bard Before adding: " + character.Def);
				//if the item can be eaten, uses it and then discards from list, if equipable moves it to character list

				item.Effect.Apply(character, item);

				if(item.Type == Item.ItemType.Equipable) {
					character.Equipment.Add(item);
				}

				Discard(item);
				Debug.Log("defence of bard after adding: " + character.Def);
			} else {
				Debug.Log("Inventory empty");
			}
		}


		public void Unequip(Item item, RPGsys.Character character) {
			if(character.Equipment.Count() > 0) {
				foreach(RPGsys.Buff buff in item.Effect.currentEffects) {
					buff.EquipRemove(character, item);
					character.Equipment.Remove(item);
				}

				Add(item);	// Using class Add method so InventoryChanged event fires

				Debug.Log("defence of bard after removal: " + character.Def);
			} else {
				Debug.Log("nothing to remove");
			}

		}
	}
}
