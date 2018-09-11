using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RPGsys {
	public class CharacterUI : MonoBehaviour {

		public Character character;

		public Image Portrait;
		public Text nameTag;
		public Image HPBar;
		public Image MPBar;

		public GameObject GetContainer{ get; private set; }

		//check if things are null before setting them up
		public void SetCharacter(Character ch) {

			character = ch;

			if(Portrait != null) {
				Portrait.sprite = character.Portrait;
			}
			if(nameTag != null) {
				nameTag.text = character.name;
			}
		}

		void Update() {

			//SetCharacter(character); // hack

			if(character != null) {
				if(HPBar != null) {
					HPBar.fillAmount = character.Hp / character.hpStat;
				}
				if(MPBar != null) {
					HPBar.fillAmount = character.Mp / character.mpStat;
				}
			}
		}
	}
}