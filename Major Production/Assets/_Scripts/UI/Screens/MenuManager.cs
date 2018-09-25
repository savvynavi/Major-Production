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
		public Tooltip tooltip;

		bool open;
		public bool Open { get { return open; } }

		bool scrollMoving;

		MenuScreen currentScreen;

		// Use this for initialization
		void Start()
		{
			currentScreen = inventoryMenu;
			inventoryMenu.Close();
			characterMenu.Close();
			commonElements.SetActive(false);
			tooltip.gameObject.SetActive(false);
			scroll.gameObject.SetActive(false);
			open = false;
			scrollMoving = false;
		}

		// Update is called once per frame
		void Update()
		{

		}

		public void OpenMenus()
		{
			if (!scrollMoving)
			{
				StartCoroutine(OpenMenuRoutine());
			}
		}

		public void CloseMenus()
		{
			if (!scrollMoving)
			{
				StartCoroutine(CloseMenuRoutine());
			}
		}

		public void CloseMenuImmediate()
		{
			StopAllCoroutines();
			scrollMoving = false;
			MenuCloseInternal();
		}

		private IEnumerator OpenMenuRoutine()
		{
			scrollMoving = true;
			MenuOpenInternal();
			scroll.SnapClosed();
			yield return scroll.OpenScroll();
			scrollMoving = false;
		}

		private IEnumerator CloseMenuRoutine()
		{
			scrollMoving = true;
			yield return scroll.CloseScroll();
			MenuCloseInternal();
			scrollMoving = false;
		}

		private void MenuOpenInternal()
		{
			GameController.Instance.Pause();
			currentScreen.Open();
			commonElements.SetActive(true);
			open = true;
			scroll.gameObject.SetActive(true);
		}

		private void MenuCloseInternal()
		{
			currentScreen.Close();
			scroll.gameObject.SetActive(false);
			commonElements.SetActive(false);
			tooltip.gameObject.SetActive(false);
			open = false;
			GameController.Instance.Unpause();
		}

		public void SwitchMenu(MenuScreen newScreen)
		{
			if (newScreen != currentScreen)
			{
				if (open)
				{
					if (!scrollMoving)
					{
						StartCoroutine(SwitchMenuRoutine(newScreen));
					}
				}
				else
				{
					currentScreen = newScreen;
				}
			}
		}

		private IEnumerator SwitchMenuRoutine(MenuScreen newScreen)
		{
			scrollMoving = true;
			yield return scroll.CloseScroll(true);
			currentScreen.Close();
			newScreen.Open();
			currentScreen = newScreen;
			yield return scroll.OpenScroll();
			scrollMoving = false;
		}
	}
}
