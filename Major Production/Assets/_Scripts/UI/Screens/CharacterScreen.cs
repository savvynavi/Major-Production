using System.Collections;
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
		[SerializeField] List<PowerToggle> powerToggles;	// maybe use GetComponentsInChildren?
		[SerializeField] Text LevelText; //HACK

		List<StatDisplay> statDisplays;

		// TODO health and mana bar

		//HACK make these WeaponSlot and RingSlots
		[SerializeField] EquipmentSlotUI weaponSlot;
		[SerializeField] EquipmentSlotUI ringLSlot;
		[SerializeField] EquipmentSlotUI ringRSlot;
		[SerializeField] GameObject equipmentSlotPrefab;
		
		public InventoryPanel inventoryPanel;

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

			for (int i = 0; i < powerToggles.Count; ++i)
			{
				if (i < CurrentChar.classInfo.classPowers.Count) {
					powerToggles[i].SetContents(CurrentChar, CurrentChar.classInfo.classPowers[i]);
				}
				else
				{
					powerToggles[i].SetContents(CurrentChar, null);
				}
			}
			LevelText.text = string.Format("Level {0}", CurrentChar.experience.CharacterLevel);
		}



		void Awake()
		{
			statDisplays = new List<StatDisplay>(GetComponentsInChildren<StatDisplay>(true));
		}

		private void Start()
		{
			weaponSlot.draggable.dragArea = this.transform;
			ringLSlot.draggable.dragArea = this.transform;
			ringRSlot.draggable.dragArea = this.transform;
			inventoryPanel.dragArea = this.transform;
		}

		// Update is called once per frame
		void Update()
		{

		}

		public void UpdateItems()
		{
			inventoryPanel.UpdateItems();
		}

		public void UpdateEquipment()
		{
			// TODO show weapon, both rings
			weaponSlot.SetEquipmentSlot(CurrentChar.weapon);
			ringLSlot.SetEquipmentSlot(CurrentChar.ringL);
			ringRSlot.SetEquipmentSlot(CurrentChar.ringR);
		}

		//HACK
		public void UnequipAllItems()
		{
			CurrentChar.UnequipAll();
			SelectCharacter();
		}

		//HACK for testing level up system
		public void LevelUpCharacter()
		{
			CurrentChar.GetComponent<RPG.XP.Experience>().LevelUp();
			DisplayCharacter();
		}

		public void RefreshAllPowerToggles()
		{
			foreach(PowerToggle pt in powerToggles)
			{
				pt.RefreshState();
			}
		}
	}
}