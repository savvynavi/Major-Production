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

		Button goBackButton;
		public bool UndoMove = false;
		//public Image Panel;

		public void SetButtons(Character chara) {
			character = chara;

			if(character != null) {
				//if there is both a button and a backing panel, it loops over character and instantiates a button for each power
				if(Button != null) {
					foreach(Powers pow in character.classInfo.classPowers) {
						//call go = getComponent powerUI and that should do it
						GameObject go = Instantiate(Button.gameObject);
						go.transform.SetParent(transform);
						go.transform.localScale = Vector3.one;

						PowerUI = go.GetComponent<PowersUI>();
						PowerUI.SetPower(pow, go, character);
						go.name = PowerUI.Name.text + "(" + character.name + ")";
					}

					//undo button setup
					if(character.ChoiceOrder != 1) {
						GameObject tmp = Instantiate(Button.gameObject);
						goBackButton = tmp.GetComponent<Button>();
						tmp.transform.SetParent(transform, false);
						goBackButton.name = "UNDO";
						goBackButton.GetComponentInChildren<Text>().text = "UNDO";
						goBackButton.onClick.AddListener(() => HandleClickBack());

					}
				}
			}

			HidePowerButtons();
		}

		public void ShowPowerButtons() {
			transform.gameObject.SetActive(true);
			character.ActivePlayer = false;
		}

		public void HidePowerButtons() {
			transform.gameObject.SetActive(false);
		}

		public void HandleClickBack() {
			FindObjectOfType<TurnBehaviour>().RemoveAttack();
			UndoMove = true;
		}
	}
}