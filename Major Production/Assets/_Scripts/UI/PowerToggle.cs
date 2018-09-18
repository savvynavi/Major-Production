using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using RPGsys;

namespace RPG.UI
{
	[RequireComponent(typeof(Button))]
	public class PowerToggle : MonoBehaviour
	{
		public Character character { get; private set; }
		public Powers power { get; private set; }
		public bool isPowerOn { get; private set; }

		Button button;
		Text buttonText;

		private void Awake()
		{
			button = GetComponent<Button>();
			buttonText = GetComponentInChildren<Text>();
		}

		// Use this for initialization
		void Start()
		{
			button.onClick.AddListener(ToggleActive);
		}

		// Update is called once per frame
		void Update()
		{

		}

		public void SetContents(Character c, Powers p)
		{
			if (!c.classInfo.classPowers.Contains(p))
			{
				throw new System.ArgumentException("Power " + p.powName + " not found in character " + c.name);
			}
			character = c;
			power = p;
			buttonText.text = power.powName;
			RefreshState();
		}

		public void RefreshState()
		{
			isPowerOn = character.ActivePowers.Contains(power);
			if (isPowerOn)
			{
				buttonText.fontStyle = FontStyle.Bold;
			} else
			{
				buttonText.fontStyle = FontStyle.Normal;
			}
			// HACK replace with a MaxActive thing?

			button.interactable = true;
			if(character.ActivePowers.Count == 4)
			{
				if (!isPowerOn)
				{
					button.interactable = false;
				} 
			} else if (character.ActivePowers.Count == 1)
			{
				if (isPowerOn)
				{
					button.interactable = false;
				}
			}
		}

		void ToggleActive()
		{
			bool success;
			if (isPowerOn)
			{
				success = character.DeactivatePower(power);
			}
			else
			{
				success = character.ActivatePower(power);
			}
			if (success)
			{
				// TODO tell parent to refresh state of all toggles
			}
		}
	}
}