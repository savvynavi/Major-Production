using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace RPGsys {
	public class BattleUIController : MonoBehaviour {
		public Button button;
		public Text font;
		public GameObject canvas;
		public GameObject MenuLayout;
		public Image Healthbar;
		public Image Magicbar;

		Image ButtonPanel;
		Text NameText;
		List<ButtonBehaviour> btnBehaviours;
		StateManager stateManager;

		private void Start() {
			//MenuLayout.SetActive(false);

			//grab all the buttonBehaviours and store in a list
			stateManager = GetComponent<StateManager>();
			foreach(Character chara in stateManager.characters) {
				btnBehaviours.Add(chara.GetComponent<ButtonBehaviour>());
			}

			//finds the button panel, change later to be less eeh
			Transform[] children = MenuLayout.transform.GetComponentsInChildren<Transform>();
			GameObject tmp = null;
			foreach(Transform child in children) {
				if(child.name == "ButtonPanel") {
					tmp = child.gameObject;
				}
			}
			ButtonPanel = tmp.GetComponent<Image>();

			foreach(Transform child in children) {
				if(child.name == "Name") {
					tmp = child.gameObject;
				}
			}
			NameText = tmp.GetComponent<Text>();
			UISetup();
		}

		public void Instantiate() {

		}

		private void UISetup() {
			foreach(ButtonBehaviour btnBehav in btnBehaviours) {
				btnBehav.Setup(button, NameText, Healthbar, Magicbar, ButtonPanel);
			}
		}


	}

}