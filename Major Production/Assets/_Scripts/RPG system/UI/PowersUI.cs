﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//stores all info needed to create a single power, will be called by buttonUI and needs to be on the button prefab passed in there
namespace RPGsys {
	public class PowersUI : MonoBehaviour {
		public Character character;
		public Powers power;

		public Image Icon;
		public Text Name;

		public Button Btn;

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
						Debug.Log("button text not null");
					}
				}

				//hooking up button to event
				Btn.onClick.AddListener(() => HandleClick());
				
			}
		}

		public void HandleClick() {
			Debug.Log("power button clicked: " + power.powName);
			FindObjectOfType<TurnBehaviour>().TurnAddAttack(power, character);
			character.ActivePlayer = true;
		}

	}

}