using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//currently no listeners added to buttons
namespace RPGsys {
	public class CharacterButtonUI : MonoBehaviour {

		public Character character;
		public PowersUI PowerUI;

		public Button Button;
		
		//public Image Panel;

		public void SetButtons(Character chara) {
			character = chara;

			if(character != null) {
				//if there is both a button and a backing panel, it loops over character and instantiates a button for each power
				if(Button != null) {
					int count = 1;
					foreach(Powers pow in character.classInfo.classPowers) {
						//call go = getComponent powerUI and that should do it
						GameObject go = Instantiate(Button.gameObject);
						PowerUI = go.GetComponent<PowersUI>();

						go.name = PowerUI.Name.text + "(" + count + ")";
						go.GetComponentInChildren<Text>().text = PowerUI.Name.text;

						//Button buttonInstance = go.GetComponent<Button>();
						////go.transform.SetParent(transform, false);
						//go.name = pow.name + "(" + count + ")";
						//go.GetComponentInChildren<Text>().text = pow.powName;
					}
				}
			}

		}
	}
}