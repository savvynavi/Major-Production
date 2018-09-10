﻿using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using RPGsys;

namespace RPG.UI
{
	public class CharacterScreen : MenuScreen
	{

		

		[SerializeField] Dropdown characterDropdown;
		List<RPGsys.Character> characters;
		public RPGsys.Character CurrentChar { get; private set; }

		//HACK might find different way to show this
		[SerializeField] Text AbilitiesText;

		List<StatDisplay> statDisplays;

		// TODO health and mana bar

		//HACK 
		[SerializeField] EquipmentBox equipmentBox;
		[SerializeField] RectTransform equipmentPanel;
		[SerializeField] GameObject equipmentSlotPrefab;

		// TODO maybe make itempanel its own class so behaviour can be reused?
		[SerializeField] GameObject ItemBoxPrefab;
		public RectTransform itemPanel;

		public override void Close()
		{
			gameObject.SetActive(false);
			GameController.Instance.inventory.OnInventoryChanged.RemoveListener(UpdateItems);
			GameController.Instance.inventory.OnInventoryChanged.RemoveListener(UpdateEquipment);
		}

		public override void Open()
		{
			gameObject.SetActive(true);
			PopulateDropdownOptions();
			// TODO instead of these, have the change trigger a Dirty flag and do it at update
			GameController.Instance.inventory.OnInventoryChanged.AddListener(UpdateItems);
			GameController.Instance.inventory.OnInventoryChanged.AddListener(UpdateEquipment);
			SelectCharacter();
			UpdateItems();
		}

		void PopulateDropdownOptions()
		{
			characterDropdown.options.Clear();
			characters = new List<RPGsys.Character>(GameController.Instance.Characters);
			foreach (RPGsys.Character character in characters)
			{
				characterDropdown.options.Add(new Dropdown.OptionData(character.name));
			}
			int temp = characterDropdown.value;
			characterDropdown.value = temp + 1;
			characterDropdown.value = temp;
		}

		public void SelectCharacter()
		{
			// TODO based on dropdown value pick character
			CurrentChar = characters[characterDropdown.value];
			equipmentBox.character = CurrentChar;
			foreach (StatDisplay stat in statDisplays)
			{
				stat.character = CurrentChar;
			}
			DisplayCharacter();
			UpdateEquipment();
		}

		public void DisplayCharacter(StatDisplay.StatChangeData changeData = null)
		{
			// TODO just make this set it as dirty and update next update?

			foreach(StatDisplay stat in statDisplays)
			{
				stat.Display(changeData);
			}

			StringBuilder abilityList = new StringBuilder();
			foreach (RPGsys.Powers ability in CurrentChar.classInfo.classPowers)
			{
				abilityList.Append(ability.powName + '\n');
			}
			AbilitiesText.text = abilityList.ToString();
		}

		void Awake()
		{
			statDisplays = new List<StatDisplay>(GetComponentsInChildren<StatDisplay>(true));
		}

		// Update is called once per frame
		void Update()
		{

		}

		public void UpdateItems()
		{
			foreach (Transform child in itemPanel)
			{
				GameObject.Destroy(child.gameObject);
			}
			foreach (RPGItems.Item item in GameController.Instance.inventory.playerInventory)
			{
				GameObject obj = Instantiate(ItemBoxPrefab, itemPanel);
				InventorySlot box = obj.GetComponent<InventorySlot>();
				box.ContainedItem = item;
				box.draggable.dragArea = this.transform;
			}
		}

		public void UpdateEquipment()
		{
			foreach (Transform child in equipmentPanel)
			{
				GameObject.Destroy(child.gameObject);
			}
			foreach (RPGItems.Item item in CurrentChar.Equipment)
			{
				GameObject obj = Instantiate(equipmentSlotPrefab, equipmentPanel);
				EquipmentSlot box = obj.GetComponent<EquipmentSlot>();
				box.ContainedItem = item;
				box.draggable.dragArea = this.transform;
				box.character = CurrentChar;
			}
		}

		//HACK
		public void UnequipAllItems()
		{
			foreach (RPGItems.Item item in new List<RPGItems.Item>(CurrentChar.Equipment))
			{
				GameController.Instance.inventory.Unequip(item, CurrentChar);
			}
			SelectCharacter();
		}
	}
}