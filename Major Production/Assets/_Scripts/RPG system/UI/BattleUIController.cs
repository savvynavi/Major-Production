using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace RPGsys {
	public class BattleUIController : MonoBehaviour {
		public Button button;
		public Text font;
		public Canvas canvas;
		public GameObject MenuLayout;
		public Image Healthbar;
		public Image Magicbar;
		public ContinueUI ContinueUi;

		public CharacterListUI FloatingStats;
		public CharacterListUI MenuHp;

		Image ButtonPanel;
		Image Portrait;
		Image Container;
		Text NameText;
		//public GameObject TabBar;
		public MoveConfirmMenu moveConfirmMenu { get; private set; }
		StateManager stateManager;


		private void Awake() {
			//grab all the buttonBehaviours and store in a list
			stateManager = GetComponent<StateManager>();
			moveConfirmMenu = GetComponent<MoveConfirmMenu>();
			ContinueUi = GetComponent<ContinueUI>();
		}

		public void UISetup(List<Character> characters) {
			// HACK finds the button panel, change later to be less eeh
			// Maybe make a class for MenuBG that has these as fields?
			Transform[] children = MenuLayout.transform.GetComponentsInChildren<Transform>();
			foreach(Transform child in children) {
				if(child.name == "ButtonPanel") {
					ButtonPanel = child.gameObject.GetComponent<Image>();
				}
				if(child.name == "Portrait") {
					Portrait = child.gameObject.GetComponent<Image>();
				}
				if(child.name == "MP/MP Container") {
					Container = child.gameObject.GetComponent<Image>();
				}
				if(child.name == "Name") {
					NameText = child.gameObject.GetComponent<Text>();
				}
				//if(child.name == "Tab Bar") {
				//	TabBar = child.gameObject;
				//}
			}

			moveConfirmMenu.Setup(button, ButtonPanel, canvas);
			ContinueUi.Setup(ButtonPanel, canvas);

			//turning all unneeded assets off once passed into the button behaviours
			NameText.gameObject.SetActive(false);
			//ButtonPanel.gameObject.SetActive(false);
		}
	}

}