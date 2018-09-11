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
		public CharacterListUI FloatingStats;
		public CharacterListUI MenuHp;

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

		public void UISetup(List<Character> characters) {
			//storing buttons for each characterERROR OVERSTEPPING ARRAY
			foreach(Character chara in characters) {
				ButtonBehaviour btnTmp = chara.GetComponent<ButtonBehaviour>();
				btnBehaviours.Add(btnTmp);
			}

			//finds the button panel, change later to be less eeh
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
			}

		//	for(int i = 0; i < characters.Count; i++) {
				foreach(ButtonBehaviour btnBehav in btnBehaviours) {
					btnBehav.Setup(buttonBehaviourObjects, button, NameText, ButtonPanel, MenuHp.uis, canvas);
				}

			//}

			moveConfirmMenu.Setup(button, ButtonPanel, canvas);

			//turning all unneeded assets off once passed into the button behaviours
			NameText.gameObject.SetActive(false);
			ButtonPanel.gameObject.SetActive(false);
		}
	}

}