using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using RPG.Save;
using Newtonsoft.Json.Linq;

namespace RPGItems {
	//add safety list==null stuff 
	public class InventoryManager : MonoBehaviour, ISaveable {
		public List<Item> playerInventory = new List<Item>();

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

		//add item to the inventory. index parameter can be used to insert instead
		public void Add(Item item, int index = -1)
		{
			if (item != null)
			{
				if (index < 0 || index >= playerInventory.Count)
				{
					playerInventory.Add(item);
				}
				else
				{
					playerInventory.Insert(index, item);
				}
				OnInventoryChanged.Invoke();
			}
		}

		public void AddRange(IEnumerable<Item> items)
		{
			playerInventory.AddRange(items);
			OnInventoryChanged.Invoke();
		}

		//remove idem from the inventory
		public void Discard(Item item) {
			playerInventory.Remove(item);
			OnInventoryChanged.Invoke();
		}

		//Removes item from the inventory and puts another in its place
		public bool Replace(Item inventoryItem, Item incomingItem)
		{
			int itemIndex = playerInventory.IndexOf(inventoryItem);
			if (itemIndex >= 0 && incomingItem != null)
			{
				playerInventory[itemIndex] = incomingItem;
				OnInventoryChanged.Invoke();
				return true;
			}
			else
			{
				return false;
			}
		}

		// Swaps the position of two items in the inventory
		public bool SwapItems(Item first, Item second)
		{
			int firstIndex = playerInventory.IndexOf(first);
			int secondIndex = playerInventory.IndexOf(second);
			if(firstIndex >= 0 && secondIndex >= 0 && firstIndex != secondIndex)
			{
				playerInventory[firstIndex] = second;
				playerInventory[secondIndex] = first;
				OnInventoryChanged.Invoke();
				return true;
			} else
			{
				return false;
			}
		}

		public bool MoveItem(Item item, int index)
		{
			index = Mathf.Clamp(index, 0, playerInventory.Count);
			int oldIndex = playerInventory.IndexOf(item);
			if(oldIndex < 0)
			{
				return false;
			} else {
				if(index > oldIndex)
				{
					--index;
				}
				playerInventory.Remove(item);
				playerInventory.Insert(index, item);
				OnInventoryChanged.Invoke();
				return true;
			}
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
		public bool Use(Item item, RPGsys.Character character) {
			if(item != null && character != null && playerInventory.IndexOf(item) >= 0) {
				Debug.Log("defence of bard Before adding: " + character.Def);
				//if the item can be eaten, uses it and then discards from list, if equipable moves it to character list

				if (character.UseItem(item))
				{
					Discard(item);
					return true;
				} else
				{
					return false;
				}
			} else {
				Debug.Log("Item not found");
				return false;
			}
		}


		//public bool Unequip(Item item, RPGsys.Character character, int index = -1) {
		//	if (character.Unequip(item))
		//	{
		//		Add(item, index);
		//		return true;
		//	}
		//	else
		//	{
		//		Debug.Log("Item not found");
		//		return false;
		//	}

		//}

		public JObject Save()
		{
			return new JObject(
				new JProperty("playerInventory",
					new JArray(from i in playerInventory
							   select Utility.TrimCloned(i.name))));
		}

		public void Load(JObject data)
		{
            playerInventory.Clear();
			foreach(JToken i in data["playerInventory"])
			{
				playerInventory.Add(Factory<Item>.CreateInstance((string)i));
			}
			OnInventoryChanged.Invoke();
		}
	}
}
