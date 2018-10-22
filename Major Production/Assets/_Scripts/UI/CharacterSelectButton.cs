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
		public Button button { get; private set; }

		private void Awake()
		{
			button = GetComponent<Button>();
		}

		public void SetCharacter(RPGsys.Character character)
		{
			this.character = character;
			portrait.sprite = character.ButtonPortrait;
		}

		// Updates interactability based on character selected
		public void CharacterSelected(RPGsys.Character selectedCharacter)
		{
			if(character == selectedCharacter)
			{
				button.interactable = false;
			}
			else
			{
				button.interactable = true;
			}
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