using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterScreen : MenuScreen {

	[SerializeField] Dropdown characterDropdown;
	List<RPGsys.Character> characters;
	RPGsys.Character currentChar;

	//HACK 
	[SerializeField] Text displayText;

	public override void Close()
	{
		gameObject.SetActive(false);
		//todo
	}

	public override void Open()
	{
		gameObject.SetActive(true);
		PopulateDropdownOptions();
		SelectCharacter();
	}

	void PopulateDropdownOptions()
	{
		characterDropdown.options.Clear();
		characters = new List<RPGsys.Character>(GameController.Instance.playerTeam.GetComponentsInChildren<RPGsys.Character>());
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
		//hack
		displayText.text = JsonUtility.ToJson(currentChar,true);
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
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
