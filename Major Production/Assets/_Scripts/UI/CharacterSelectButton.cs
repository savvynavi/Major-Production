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
		Image background;
		public Button button;
		public Color selectedBGColor;
		public Color defaultBGColor;

		private void Awake()
		{
			background = GetComponent<Image>();
		}

		private void Update()
		{
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
				background.color = selectedBGColor;
			}
			else
			{
				button.interactable = true;
				background.color = defaultBGColor;
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