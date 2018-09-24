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
		public ScrollMask scroll;

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
			scroll.gameObject.SetActive(false);
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
			scroll.gameObject.SetActive(true);
			scroll.SnapClosed();
			StartCoroutine(scroll.OpenScroll());
		}

		public void CloseMenus()
		{
			StartCoroutine(CloseMenuRoutine());
		}

		private IEnumerator CloseMenuRoutine()
		{
			yield return scroll.CloseScroll();
			currentScreen.Close();
			scroll.gameObject.SetActive(false);
			commonElements.SetActive(false);
			open = false;
			GameController.Instance.Unpause();
		}

		public void SwitchMenu(MenuScreen newScreen)
		{
			if (newScreen != currentScreen)
			{
				if (open)
				{
					// TODO scroll wipe effect
					currentScreen.Close();
					newScreen.Open();
				}
				currentScreen = newScreen;
			}
		}


	}
}
