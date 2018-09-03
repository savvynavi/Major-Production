using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace RPG.UI
{
	// Shows effect of unequipping all items when hovered over
	public class UnequipButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
	{
		CharacterScreen screen;

		public void OnPointerEnter(PointerEventData eventData)
		{
			RPGsys.Character c = screen.CurrentChar;
			StatDisplay.StatChangeData statChange = new StatDisplay.StatChangeData();
			foreach (RPGItems.Item item in c.Equipment)
			{
				statChange.ApplyItemEffects(item, true);
			}
			screen.DisplayCharacter(statChange);
		}

		public void OnPointerExit(PointerEventData eventData)
		{
			screen.DisplayCharacter();
		}

		private void Awake()
		{
			screen = GetComponentInParent<CharacterScreen>();
		}

	}
}