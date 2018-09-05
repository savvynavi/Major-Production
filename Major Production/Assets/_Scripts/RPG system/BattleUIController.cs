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
		Text NameText;
		List<ButtonBehaviour> btnBehaviours;
		StateManager stateManager;

		private void Start() {
			//MenuLayout.SetActive(false);

			//grab all the buttonBehaviours and store in a list

			btnBehaviours = new List<ButtonBehaviour>();
			
			//UISetup();
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


			foreach(ButtonBehaviour btnBehav in btnBehaviours) {
				btnBehav.Setup(buttonBehaviourObjects, button, NameText, Healthbar, Magicbar, ButtonPanel, canvas);
			}
		}


	}

}