using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.UI
{
	public class MenuManager : MonoBehaviour
	{

		public InventoryScreen inventoryMenu;
		public CharacterScreen characterMenu;
		public GameObject commonElements;

		bool open;
		public bool Open { get { return open; } }

		MenuScreen currentScreen;

		// Use this for initialization
		void Start()
		{
			currentScreen = inventoryMenu;
			inventoryMenu.Close();
			characterMenu.Close();
			commonElements.SetActive(false);
			open = false;
		}

		// Update is called once per frame
		void Update()
		{

		}

		public void OpenMenus()
		{
			GameController.Instance.Pause();
			currentScreen.Open();
			commonElements.SetActive(true);
			open = true;
		}

		public void CloseMenus()
		{
			GameController.Instance.Unpause();
			currentScreen.Close();
			commonElements.SetActive(false);
			open = false;
		}

		public void SwitchMenu(MenuScreen newScreen)
		{
			if (newScreen != currentScreen)
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
}
