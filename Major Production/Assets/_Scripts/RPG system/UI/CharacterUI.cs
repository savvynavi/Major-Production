using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using RPG.UI;

namespace RPGsys {
	public class CharacterUI : MonoBehaviour {

		public Character character;

		public Image Portrait;
		public Text nameTag;
		public MeterBar HPBar;
		public MeterBar HPRegenBar;
		public MeterBar MPBar;
		public MeterBar MPRegenBar;
		public Image Halo;
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
					nameTag.text = character.characterName;
				}

				//snaps the hp bar to current hp

				if(HPBar != null) {
					HPBar.Init(character.Hp, character.hpStat);
				}
				if(HPRegenBar != null)
				{
					HPRegenBar.Init(0, character.hpStat);
				}
				if(MPBar != null) {
					MPBar.Init(character.Mp, character.mpStat);
				}
				if(MPRegenBar != null)
				{
					MPRegenBar.Init(0,character.mpStat);
				}
				if(Halo != null) {
					Halo.gameObject.SetActive(false);
				}

			} else {
				//todo clear info if no character
			}
		}

		void Update() {

			if(character != null) {
				HPBar.setValues(character.Hp, character.hpStat);
				MPBar.setValues(character.Mp, character.mpStat);
			}
		}
	}
}