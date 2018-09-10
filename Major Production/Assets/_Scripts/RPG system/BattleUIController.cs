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
		//REMOVE ONCE BBOBJ BECOME OBSOLETE 
		public ButtonBehaviourObjects buttonBehaviourObjects;

		Image ButtonPanel;
		Image Portrait;
		Image Container;
		Text NameText;
		List<ButtonBehaviour> btnBehaviours;
		public MoveConfirmMenu moveConfirmMenu { get; private set; }
		StateManager stateManager;

		private void Awake() {
			//grab all the buttonBehaviours and store in a list
			btnBehaviours = new List<ButtonBehaviour>();
			stateManager = GetComponent<StateManager>();
			moveConfirmMenu = GetComponent<MoveConfirmMenu>();
		}

		public void Instantiate() {
			//if() {

			//}
		}

		public void UISetup(List<Character> characters) {

			
			foreach(Character chara in characters) {
				ButtonBehaviour btnTmp = chara.GetComponent<ButtonBehaviour>();
				btnBehaviours.Add(btnTmp);
			}

			//finds the button panel, change later to be less eeh
			Transform[] children = MenuLayout.transform.GetComponentsInChildren<Transform>();
			GameObject tmp = null;
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
			}

			foreach(ButtonBehaviour btnBehav in btnBehaviours) {
				btnBehav.Setup(buttonBehaviourObjects, button, NameText, Healthbar, Magicbar, ButtonPanel, Portrait, Container, canvas);
			}

			moveConfirmMenu.Setup(button, ButtonPanel, canvas);

			//turning all unneeded assets off once passed into the button behaviours
			Healthbar.gameObject.SetActive(false);
			Magicbar.gameObject.SetActive(false);
			Portrait.gameObject.SetActive(false);
			NameText.gameObject.SetActive(false);
			ButtonPanel.gameObject.SetActive(false);
		}
	}

}