using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

//TODO AAA URGENT make equipped items update on dropping
public class CharacterScreen : MenuScreen {

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
		foreach(RPGsys.Character character in characters)
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

	}

	public void DisplayCharacter(RPGItems.Item newItem = null)
	{
		//TODO if newItem is not null, calculate changes and show those
		
		speedText.text = string.Format("Speed {0:00.}", currentChar.Speed);
		strText.text = string.Format("Strength {0:00.}", currentChar.Str);
		defText.text = string.Format("Defence {0:00.}", currentChar.Dex);
		intText.text = string.Format("Intelligence {0:00.}", currentChar.Int);
		mindText.text = string.Format("Mind {0:00.}", currentChar.Mind);
		dexText.text = string.Format("Dexterity {0:00.}", currentChar.Dex);
		agiText.text = string.Format("Agility {0:00.}", currentChar.Agi);

		hpText.text = string.Format("HP {0:0.}/{1:0.}", currentChar.Hp, currentChar.hpStat);
		mpText.text = string.Format("MP {0:0.}/{1:0.}", currentChar.Mp, currentChar.mpStat);

		StringBuilder abilityList = new StringBuilder();
		foreach (RPGsys.Powers ability in currentChar.classInfo.classPowers)
		{
			abilityList.Append(ability.powName + '\n');
		}
		AbilitiesText.text = abilityList.ToString();

		UpdateEquipment();
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
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
		foreach(Transform child in equipmentPanel)
		{
			GameObject.Destroy(child.gameObject);
		}
		foreach (RPGItems.Item item in currentChar.Equipment)
		{
			GameObject obj = Instantiate(ItemBoxPrefab, equipmentPanel);
			EquipmentSlot box = obj.GetComponent<EquipmentSlot>();
			box.ContainedItem = item;
			box.draggable.dragArea = this.transform;
			box.character = currentChar;
		}
	}

	//HACK
	public void UnequipAllItems()
	{
		foreach(RPGItems.Item item in new List<RPGItems.Item>(currentChar.Equipment))
		{
			GameController.Instance.inventory.Unequip(item, currentChar);
		}
		SelectCharacter();
	}
}
