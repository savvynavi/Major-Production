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

		public class StatChangeData
		{
			public RPGItems.Item ItemToUse = null;
			public RPGItems.Item ItemToRemove = null;
			// HACK maybe make this a map?
			public float speedChange = 0;
			public float strChange = 0;
			public float defChange = 0;
			public float intChange = 0;
			public float mindChange = 0;
			public float dexChange = 0;
			public float agiChange = 0;
			public float hpChange = 0;
			public float mpChange = 0;

			public void ApplyItemEffects(RPGItems.Item item, bool removing = false)
			{
				float multiplier = removing ? -1 : 1;
				foreach (RPGsys.Status buff in item.Effect.currentEffects)
				{
					//HACK
					if (buff is RPGsys.Buff)
					{
						RPGsys.Buff b = (RPGsys.Buff)buff;
						RPGStats.Stats tgtStat = b.StatusEffects.statBuff;
						float amount = b.StatusEffects.amount * multiplier;
						switch (b.StatusEffects.effect)
						{
							case Status.StatusEffectType.Heal:
							//HACK change tgtStat to hp?
							case Status.StatusEffectType.Buff:
								break;
							case Status.StatusEffectType.Debuff:
								amount *= -1;
								break;
						}
						switch (tgtStat)
						{
							case RPGStats.Stats.Agi:
								agiChange += amount;
								break;
							case RPGStats.Stats.Def:
								defChange += amount;
								break;
							case RPGStats.Stats.Dex:
								dexChange += amount;
								break;
							case RPGStats.Stats.Hp:
								hpChange += amount;
								break;
							case RPGStats.Stats.Int:
								intChange += amount;
								break;
							case RPGStats.Stats.Mind:
								mindChange += amount;
								break;
							case RPGStats.Stats.Mp:
								mpChange += amount;
								break;
							case RPGStats.Stats.Speed:
								speedChange += amount;
								break;
							case RPGStats.Stats.Str:
								strChange += amount;
								break;
						}
					}
				}
			}
		}

		[SerializeField] Dropdown characterDropdown;
		List<RPGsys.Character> characters;
		RPGsys.Character currentChar;

		//HACK might find different way to show thesed
		[SerializeField] Text speedText;
		[SerializeField] Text strText;
		[SerializeField] Text defText;
		[SerializeField] Text intText;
		[SerializeField] Text mindText;
		[SerializeField] Text dexText;
		[SerializeField] Text agiText;
		[SerializeField] Text hpText;
		[SerializeField] Text mpText;
		[SerializeField] Text AbilitiesText;
		//[SerializeField] Text EquipmentText; //HACK

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
			currentChar = characters[characterDropdown.value];
			equipmentBox.character = currentChar;
			DisplayCharacter();
			UpdateEquipment();
		}

		public void DisplayCharacter(StatChangeData changeData = null)
		{
			// TODO just make this set it as dirty and update next update?
			if (changeData == null)
			{

				speedText.text = string.Format("Speed {0:00.}", currentChar.Speed);
				strText.text = string.Format("Strength {0:00.}", currentChar.Str);
				defText.text = string.Format("Defence {0:00.}", currentChar.Def);
				intText.text = string.Format("Intelligence {0:00.}", currentChar.Int);
				mindText.text = string.Format("Mind {0:00.}", currentChar.Mind);
				dexText.text = string.Format("Dexterity {0:00.}", currentChar.Dex);
				agiText.text = string.Format("Agility {0:00.}", currentChar.Agi);

				hpText.text = string.Format("HP {0:0.}/{1:0.}", currentChar.Hp, currentChar.hpStat);
				mpText.text = string.Format("MP {0:0.}/{1:0.}", currentChar.Mp, currentChar.mpStat);

			}
			else
			{
				// HACK probably should have some kind of StatDisplay class
				// In that class would just show stat normally if no change, alter colour, etc, which would be annoying to do here
				speedText.text = string.Format("Speed {0:00.} ({1:00.})", currentChar.Speed, currentChar.Speed + changeData.speedChange);
				strText.text = string.Format("Strength {0:00.} ({1:00.})", currentChar.Str, currentChar.Str + changeData.strChange);
				defText.text = string.Format("Defence {0:00.} ({1:00.})", currentChar.Def, currentChar.Def + changeData.defChange);
				intText.text = string.Format("Intelligence {0:00.} ({1:00.})", currentChar.Int, currentChar.Int + changeData.intChange);
				mindText.text = string.Format("Mind {0:00.} ({1:00.})", currentChar.Mind, currentChar.Mind + changeData.mindChange);
				dexText.text = string.Format("Dexterity {0:00.} ({1:00.})", currentChar.Dex, currentChar.Dex + changeData.dexChange);
				agiText.text = string.Format("Agility {0:00.} ({1:00.})", currentChar.Agi, currentChar.Agi + changeData.agiChange);

				// Figure out how these should be shown
				hpText.text = string.Format("HP {0:0.}/{1:0.}", currentChar.Hp, currentChar.hpStat);
				mpText.text = string.Format("MP {0:0.}/{1:0.}", currentChar.Mp, currentChar.mpStat);
			}

			StringBuilder abilityList = new StringBuilder();
			foreach (RPGsys.Powers ability in currentChar.classInfo.classPowers)
			{
				abilityList.Append(ability.powName + '\n');
			}
			AbilitiesText.text = abilityList.ToString();
		}

		// Use this for initialization
		void Start()
		{

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
			foreach (RPGItems.Item item in currentChar.Equipment)
			{
				GameObject obj = Instantiate(equipmentSlotPrefab, equipmentPanel);
				EquipmentSlot box = obj.GetComponent<EquipmentSlot>();
				box.ContainedItem = item;
				box.draggable.dragArea = this.transform;
				box.character = currentChar;
			}
		}

		//HACK
		public void UnequipAllItems()
		{
			foreach (RPGItems.Item item in new List<RPGItems.Item>(currentChar.Equipment))
			{
				GameController.Instance.inventory.Unequip(item, currentChar);
			}
			SelectCharacter();
		}
	}
}