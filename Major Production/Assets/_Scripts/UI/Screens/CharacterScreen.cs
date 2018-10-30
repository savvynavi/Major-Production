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
		[SerializeField] List<GameObject> Tabs;
		[SerializeField] GameObject CurrentTab;

		[SerializeField] RectTransform characterSelectPanel;
		[SerializeField] CharacterSelectButton characterSelectPrefab;
		[SerializeField] CharacterMenuPortrait characterPortrait;
		List<RPGsys.Character> characters;
		public RPGsys.Character CurrentChar { get; private set; }

		//HACK might find different way to show this
		[SerializeField] List<PowerToggle> powerToggles;    // maybe use GetComponentsInChildren?
		[SerializeField] Text powerDescriptionText;
		[SerializeField] Text NameText;
		[SerializeField] Text LevelText;
		[SerializeField] Text xpText;

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
			PopulationCharacterSelection();
			// TODO instead of these, have the change trigger a Dirty flag and do it at update
			GameController.Instance.inventory.OnInventoryChanged.AddListener(UpdateItems);
			GameController.Instance.inventory.OnInventoryChanged.AddListener(UpdateEquipment);
			SelectCharacter(characters[0]);
			UpdateItems();
			powerDescriptionText.text = "";
			foreach(PowerToggle toggle in powerToggles)
			{
				toggle.descriptionText = powerDescriptionText;
			}
			// TODO deactivate all tabs and activate current one
			foreach(GameObject tab in Tabs)
			{
				tab.SetActive(false);
			}
			CurrentTab.SetActive(true);
		}

		public void ChangeTab(GameObject newTab)
		{
			// TODO use scroll effect here too
			if(newTab != CurrentTab)
			{
				CurrentTab.SetActive(false);
				CurrentTab = newTab;
				CurrentTab.SetActive(true);
			}

		}

		void PopulationCharacterSelection()
		{
			foreach(Transform child in characterSelectPanel)
			{
				GameObject.Destroy(child.gameObject);
			}
			characters = new List<RPGsys.Character>(GameController.Instance.Characters);
			foreach (RPGsys.Character character in characters)
			{
				GameObject clone = Instantiate(characterSelectPrefab.gameObject, characterSelectPanel);
				CharacterSelectButton charButton = clone.GetComponent<CharacterSelectButton>();
				charButton.SetCharacter(character);
				charButton.button.onClick.AddListener(() => SelectCharacter(character));
			}
		}

		public void SelectCharacter(Character character)
		{
			// TODO make CharacterSelectButton with this character highlighted (but unselectable)
			CurrentChar = character;
			foreach (Transform child in characterSelectPanel)
			{
				child.GetComponent<CharacterSelectButton>().CharacterSelected(character);
			}
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
			NameText.text = CurrentChar.characterName;
			characterPortrait.SetCharacter(CurrentChar);
			foreach(StatDisplay stat in statDisplays)
			{
				stat.Display(changeData);
			}

			if(changeData != null)
			{
				float hpChange;
				float mpChange;
				if(changeData.changes.TryGetValue(RPGStats.Stats.Hp, out hpChange))
				{
					characterPortrait.characterUI.HPRegenBar.Init(hpChange + CurrentChar.Hp, CurrentChar.hpStat);
				}
				if (changeData.changes.TryGetValue(RPGStats.Stats.Mp, out mpChange))
				{
					characterPortrait.characterUI.MPRegenBar.Init(mpChange + CurrentChar.Mp, CurrentChar.mpStat);
				}
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
			xpText.text = string.Format("XP {0}/{1}", CurrentChar.experience.Exp, CurrentChar.experience.XPToLevel);
		}



		void Awake()
		{
			statDisplays = new List<StatDisplay>(GetComponentsInChildren<StatDisplay>(true));
		}

		private void Start()
		{

		}

		// Update is called once per frame
		void Update()
		{
			// HACK
			if (Input.GetKeyDown(KeyCode.H))
			{
				CurrentChar.Hp = CurrentChar.Hp - 100;
				DisplayCharacter();
			}
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
			// HACK
			SelectCharacter(CurrentChar);
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