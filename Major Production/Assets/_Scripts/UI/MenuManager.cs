using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour {

	public InventoryScreen inventoryMenu;
	public CharacterScreen characterMenu;

	bool open;
	public bool Open { get { return open; } }

	MenuScreen currentScreen;

	// Use this for initialization
	void Start () {
		currentScreen = inventoryMenu;
		inventoryMenu.Close();
		characterMenu.Close();
		open = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void OpenMenus()
	{
		currentScreen.Open();
		open = true;
	}

	public void CloseMenus()
	{
		currentScreen.Close();
		open = false;
	}

	public void SwitchMenu(MenuScreen newScreen)
	{
		if(newScreen != currentScreen)
		{
			if (open)
			{
				currentScreen.Close();
				newScreen.Open();
			}
			currentScreen = newScreen;
		}
	}
}
