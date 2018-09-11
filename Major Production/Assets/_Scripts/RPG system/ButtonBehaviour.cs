using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace RPGsys {
    [System.Serializable]
    public class ButtonBehaviourObjects
    {
        public GameObject canvas;
        public Transform menuPos;
        public Transform namePos;

        public Transform hpPosition;
        public Transform mpPosition;
        public Transform hpTextPos;
        public Transform mpTextPos;
        public Transform hoverTxtPos;
    }

	public class ButtonBehaviour : MonoBehaviour {

		float CharacterCurrentHP;
		float CharacterMaxHP;
		float CharacterCurrentMP;
		float CharacterMaxMP;
		Text HoverText;

		List<Powers> powerList;
		string charaName;


		public bool playerActivated;
		public bool undoMove = false;
		public List<Button> buttons;

		Canvas canvas;

		public Transform hoverTxtPos;

		GameObject bgPanel;
		Text charaNameText;
		Text hpTxt;
		Text mpTxt;
		Button goBackBtn;
		CharacterUI characterUI;


		private void Awake() {
			powerList = GetComponent<Character>().classInfo.classPowers;
			buttons = new List<Button>();
		}

		private void Start() {
			CharacterCurrentHP = GetComponent<Character>().Hp;
			CharacterMaxHP = GetComponent<Character>().hpStat;
			CharacterCurrentMP = GetComponent<Character>().Mp;
			CharacterMaxMP = GetComponent<Character>().mpStat;
		}

		private void Update() {
			CharacterCurrentHP = GetComponent<Character>().Hp;
			CharacterMaxHP = GetComponent<Character>().hpStat;
			CharacterCurrentMP = GetComponent<Character>().Mp;
			CharacterMaxMP = GetComponent<Character>().mpStat;
		}

		public void Setup(ButtonBehaviourObjects bboRefs, Button button, Text text,Image panel,List<CharacterUI> charaUIs, Canvas canvasUI) {
			int count = 0;
			playerActivated = false;
			charaName = transform.name;

			canvas = canvasUI;
			bgPanel = panel.gameObject;
			for(int i = 0; i < charaUIs.Count; i++) {
				if(charaUIs[i].name == transform.GetComponent<Character>().name) {
					characterUI = charaUIs[i];
				}
			}

			hoverTxtPos = bboRefs.hoverTxtPos;

			//player name
			GameObject tmpTxt = Instantiate(text.gameObject);
			charaNameText = tmpTxt.GetComponent<Text>();
			charaNameText.transform.SetParent(canvas.transform, false);
			charaNameText.transform.position = text.transform.position;
			charaNameText.text = transform.GetComponent<Character>().name;

			//setting up each power with a button
			foreach(Powers pow in powerList) {
				GameObject go = Instantiate(button.gameObject);
                Button buttonInstance = go.GetComponent<Button>();
				go.transform.SetParent(bgPanel.transform, false);
				go.name = pow.powName + "(" + (count + 1) + ")";
				go.GetComponentInChildren<Text>().text = pow.powName;
				buttons.Add(buttonInstance);

				//setup for hover textbox, set to inactive 
				//HoverButtonSetup(pow, buttonInstance);
				count++;
			}

			//back button setup
			if(transform.GetComponent<Character>().ChoiceOrder != 1) {
				GameObject tmp = Instantiate(button.gameObject, bgPanel.transform);

				goBackBtn = tmp.GetComponent<Button>();
				goBackBtn.transform.SetParent(bgPanel.transform, false);
				goBackBtn.name = "UNDO";
				goBackBtn.GetComponentInChildren<Text>().text = "UNDO";
				goBackBtn.onClick.AddListener(() => HandleClickBack());
			}

			//adding listeners to each button/settting active state to false
			for(int i = 0; i < buttons.Count; i++) {
				int capturedIndex = i;
				buttons[i].onClick.AddListener(() => HandleClick(capturedIndex));

				//hover button stuff (on enter/exit listeners added here)
				EventTrigger trigger = GetComponent<EventTrigger>();
				if(trigger == null) {
					trigger = buttons[i].gameObject.AddComponent<EventTrigger>();
				}

				EventTrigger.Entry pointerEnter = new EventTrigger.Entry();
				pointerEnter.eventID = EventTriggerType.PointerEnter;
				pointerEnter.callback.AddListener((data) => { OnPointerEnter(data, capturedIndex); });
				trigger.triggers.Add(pointerEnter);

				EventTrigger.Entry pointerExit = new EventTrigger.Entry();
				pointerExit.eventID = EventTriggerType.PointerExit;
				pointerExit.callback.AddListener((data) => { OnPointerExit(data, capturedIndex); });
				trigger.triggers.Add(pointerExit);
			}

			HideButtons();
		}

		public void ShowButtons() {
			bgPanel.SetActive(true);
			charaNameText.gameObject.SetActive(true);
			if(characterUI != null) {
				characterUI.gameObject.SetActive(true);
			}
			if(goBackBtn != null) {
				goBackBtn.gameObject.SetActive(true);
			}

			foreach(Button btn in buttons) {
				playerActivated = false;
				btn.gameObject.SetActive(true);
			}
		}

		public void HideButtons() {
			bgPanel.SetActive(false);
			charaNameText.gameObject.SetActive(false);
			if(characterUI != null) {
				characterUI.gameObject.SetActive(false);
			}
			if(goBackBtn != null) {
				goBackBtn.gameObject.SetActive(false);
			}


			foreach(Button btn in buttons) {
				btn.gameObject.SetActive(false);
			}

		}

		//when button clicked, does this
		public void HandleClick(int capturedIndex) {
			//adds power to a list of all powers being used this round based on button pressed
			for(int i = 0; i < buttons.Count; i++) {
				if(buttons[capturedIndex].GetComponentInChildren<Text>().text == powerList[i].powName) {
					FindObjectOfType<TurnBehaviour>().TurnAddAttack(powerList[i], transform.GetComponent<Character>());
					playerActivated = true;
				}
			}
		}

		//when UNDO button clicked, will return player to previous character turn screen
		public void HandleClickBack() {
			FindObjectOfType<TurnBehaviour>().RemoveAttack();
			undoMove = true;
		}

		//use to have button info pop up on screen/clear
		public void OnPointerEnter(BaseEventData data, int index) {
			//HoverText.gameObject.SetActive(true);
			//for(int i = 0; i < buttons.Count; i++) {
			//	if(buttons[index].GetComponentInChildren<Text>().text == powerList[i].powName) {
			//		string tmp = buttons[index].GetComponent<HoverTextStorage>().btnDescription;
			//		HoverText.text = buttons[index].GetComponent<HoverTextStorage>().btnDescription;
			//	}
			//}
		}

		public void OnPointerExit(BaseEventData data, int index) {
			//HoverText.gameObject.SetActive(false);
		}

        public void CleanUp()
        {
            //TODO 

            // Clear Buttons list
            buttons.Clear();
        }
	}
}
