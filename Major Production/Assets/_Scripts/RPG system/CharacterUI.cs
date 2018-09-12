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
		public bool FloatingMenu;

		public GameObject GetContainer{ get; private set; }

		//check if things are null before setting them up
		public void SetCharacter(Character ch) {

			character = ch;
			if(character != null) {
				if(Portrait != null) {
					Portrait.sprite = character.Portrait;
				}
				if(nameTag != null) {
					nameTag.text = character.name;
				}

				//snaps the hp bar to current hp

				if(HPBar != null) {
					HPBar.fillAmount = character.Hp / character.hpStat;
				}
				if(MPBar != null) {
					HPBar.fillAmount = character.Mp / character.mpStat;
				}
			} else {
				//todo clear info if no character
			}
		}

		void Update() {

			//SetCharacter(character); // hack

			if(character != null) {
				if(HPBar != null) {
					float hp = character.Hp / character.hpStat;
					// animate towards the true value of the character's hp
					HPBar.fillAmount = Mathf.MoveTowards(HPBar.fillAmount, hp, Time.deltaTime);
				}
				if(MPBar != null) {
					float mp = character.Mp / character.mpStat;
					MPBar.fillAmount = Mathf.MoveTowards(HPBar.fillAmount, mp, Time.deltaTime);
				}
			}
		}
	}
}