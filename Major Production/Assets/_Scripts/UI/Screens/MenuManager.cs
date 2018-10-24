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
			tooltip.enabled = false;
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
			tooltip.enabled = false;
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
			tooltip.enabled = false;
			yield return scroll.CloseScroll();
			MenuCloseInternal();
			scrollMoving = false;
		}

		private void MenuOpenInternal()
		{
			GameController.Instance.Pause();
			scroll.gameObject.SetActive(true);
			currentScreen.Open();
			commonElements.SetActive(true);
			open = true;
			tooltip.enabled = true;
		}

		private void MenuCloseInternal()
		{
			currentScreen.Close();
			commonElements.SetActive(false);
			scroll.gameObject.SetActive(false);
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
			tooltip.enabled = false;
			yield return scroll.CloseScroll(true);
			currentScreen.Close();
			newScreen.Open();
			currentScreen = newScreen;
			yield return scroll.OpenScroll();
			tooltip.enabled = true;
			scrollMoving = false;
		}
	}
}
