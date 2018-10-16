using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI
{
	[RequireComponent(typeof(Button))]
	public class CharacterSelectButton : MonoBehaviour, ITooltipTarget
	{
		public RPGsys.Character character { get; private set; }
		[SerializeField] Image portrait;

		public void SetCharacter(RPGsys.Character character)
		{
			this.character = character;
			portrait.sprite = character.Portrait;
		}

		public string TooltipText
		{
			get
			{
				if (character != null)
				{
					return character.name;
				} else
				{
					return null;
				}
			}
		}
	}
}