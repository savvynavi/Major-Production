using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using RPG.UI;

//stores all info needed to create a single power, will be called by buttonUI and needs to be on the button prefab passed in there
namespace RPGsys {
	// TODO instead of being a TooltipTarget, make this an IPointerEnterHandler and fill out some text object with the power details
	public class PowersUI : MonoBehaviour, ITooltipTarget {
		public Character character;
		public Powers power;

		public Image Icon;
		public Text Name;

		public Button Btn;

		public string TooltipText
		{
			get
			{
				if (power != null)
				{
					return power.description;
				} else
				{
					return "";
				}
			}
		}

		public void SetPower(Powers pow, GameObject button, Character chara) {
			power = pow;
			character = chara;
			Btn = button.GetComponent<Button>();
			Name = button.GetComponentInChildren<Text>();

			if(power != null && Btn != null) {
				if(Icon != null) {
					Icon.sprite = power.icon;
				}
				if(Name != null) {
					Name.text = power.powName;
					if(Btn.GetComponentInChildren<Text>().text != null) {
					}
				}

				//hooking up button to event
				Btn.onClick.AddListener(() => HandleClick());
				
			}
		}

		public void HandleClick() {
			Debug.Log("power button clicked: " + power.powName);
			FindObjectOfType<TurnBehaviour>().TurnAddAttack(power, character);

			//character.ActivePlayer = true;
		}

	}

}