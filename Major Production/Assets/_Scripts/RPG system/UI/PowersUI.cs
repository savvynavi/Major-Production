using System.Collections;
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

		public void SetPower(Powers pow) {
			power = pow;

			if(power != null) {
				if(Icon != null) {
					Icon.sprite = power.icon;
				}
				if(Name != null) {
					Name.text = power.powName;
				}
			}
		}

	}

}