using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using RPGsys;

namespace RPG.UI
{
	[RequireComponent(typeof(Button))]
	public class PowerToggle : MonoBehaviour, ITooltipTarget
	{
		public Character character { get; private set; }
		public Powers power { get; private set; }
		public bool isPowerOn { get; private set; }

		public Color ActiveColor;
		public Color InactiveColor;

		public string TooltipText
		{
			get
			{
				if(power != null)
				{
					return power.description;
				} else
				{
					return null;
				}
			}
		}

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
			character = c;
			if (p != null)
			{
				if (!c.classInfo.classPowers.Contains(p))
				{
					throw new System.ArgumentException("Power " + p.powName + " not found in character " + c.name);
				}
				power = p;
				buttonText.text = power.powName;
			} else
			{
				power = null;
				buttonText.text = "X";
				ShowPowerInactive();
			}
			RefreshState();
		}

		public void RefreshState()
		{
			if (power != null)
			{
				isPowerOn = character.ActivePowers.Contains(power);
				if (isPowerOn)
				{

					ShowPowerActive();
				}
				else
				{
					ShowPowerInactive();	
				}

				button.interactable = true;
				if (character.ActivePowers.Count == Character.maxActivePowers)
				{
					if (!isPowerOn)
					{
						button.interactable = false;
					}
				}
				else if (character.ActivePowers.Count == 1)
				{
					if (isPowerOn)
					{
						button.interactable = false;
					}
				}
			} else
			{
				button.interactable = false;
				isPowerOn = false;
			}
		}

		void ToggleActive()
		{
			if (power != null) {
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
					// HACK
					GetComponentInParent<CharacterScreen>().RefreshAllPowerToggles();
				}
			}
		}

		void ShowPowerActive()
		{
			buttonText.fontStyle = FontStyle.Bold;
			buttonText.color = ActiveColor;
		}

		void ShowPowerInactive()
		{
			buttonText.fontStyle = FontStyle.Normal;
			buttonText.color = InactiveColor;
		}
	}
}