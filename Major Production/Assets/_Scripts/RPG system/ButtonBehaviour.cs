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
		public Button button;
		public Text font;
		public GameObject canvas;
		public Transform menuPos;
		public Image menuBG;
        GameObject menuBackground;
		public Transform namePos;

		public Image hpImage;
		public Image mpImage;
		public Transform hpPosition;
		public Transform mpPosition;
		public Transform hpTextPos;
		public Transform mpTextPos;
		public Image hpBackground;

		public Transform hoverTxtPos;

		Text charaNameText;
		Image hp;
		Image mp;
		Image hpBg;
		Image mpBg;
		Text hpTxt;
		Text mpTxt;
		Button goBackBtn;


		private void Awake() {
			// TODO get ActiveAbilities instead
			powerList= new List<Powers>();
			buttons = new List<Button>();
		}

		private void Start() {

			hp.type = Image.Type.Filled;
			hp.fillMethod = Image.FillMethod.Horizontal;
			mp.type = Image.Type.Filled;
			mp.fillMethod = Image.FillMethod.Horizontal;

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

		public void Setup(ButtonBehaviourObjects bboRefs) {
			int count = 0;
			playerActivated = false;
			charaName = transform.name;


			canvas = bboRefs.canvas;
			menuPos = bboRefs.menuPos;
			namePos = bboRefs.namePos;

			hpPosition = bboRefs.hpPosition;
			mpPosition = bboRefs.mpPosition;
			hpTextPos = bboRefs.hpTextPos;
			mpTextPos = bboRefs.mpTextPos;
			hoverTxtPos = bboRefs.hoverTxtPos;

			//setting up menu items in correct positions	

			//CHEKC IF INSTANTIATING DIFFERENTLY WILL SAVE SETUP TIME//

			//menu bg
			menuBackground = Instantiate(menuBG.gameObject);
			menuBackground.transform.SetParent(canvas.transform, false);
			menuBackground.transform.position = menuPos.transform.position;

			//hp/mp bars/bg
			GameObject tmpbg1 = Instantiate(hpBackground.gameObject);
			hpBg = tmpbg1.GetComponent<Image>();
			hpBg.transform.SetParent(canvas.transform, false);
			hpBg.transform.position = hpPosition.transform.position;

			GameObject tmpHp = Instantiate(hpImage.gameObject);
			hp = tmpHp.GetComponent<Image>();
			hp.transform.SetParent(canvas.transform, false);
			hp.transform.position = hpPosition.transform.position;

			GameObject tmpbg2 = Instantiate(hpBackground.gameObject);
			mpBg = tmpbg2.GetComponent<Image>();
			mpBg.transform.SetParent(canvas.transform, false);
			mpBg.transform.position = mpPosition.transform.position;

			GameObject tmpMp = Instantiate(mpImage.gameObject);
			mp = tmpMp.GetComponent<Image>();
			mp.transform.SetParent(canvas.transform, false);
			mp.transform.position = mpPosition.transform.position;

			//hp/mp text
			GameObject tmpHpTxt = Instantiate(font.gameObject);
			hpTxt = tmpHpTxt.GetComponent<Text>();
			hpTxt.transform.SetParent(canvas.transform, false);
			hpTxt.transform.position = hpTextPos.transform.position;
			hpTxt.text = CharacterCurrentHP.ToString() +  "/" + CharacterMaxHP.ToString();

			GameObject tmpMpTxt = Instantiate(font.gameObject);
			mpTxt = tmpMpTxt.GetComponent<Text>();
			mpTxt.transform.SetParent(canvas.transform, false);
			mpTxt.transform.position = mpTextPos.transform.position;
			mpTxt.text = CharacterCurrentMP.ToString() + "/" + CharacterMaxHP.ToString();

			//player name
			GameObject tmpTxt = Instantiate(font.gameObject);
			charaNameText = tmpTxt.GetComponent<Text>();
			charaNameText.transform.SetParent(canvas.transform, false);
			charaNameText.transform.position = namePos.transform.position;
			charaNameText.text = transform.GetComponent<Character>().name;

			// Get power list again
			powerList.Clear();
			powerList.AddRange(GetComponent<Character>().ActivePowers);
			// HACK should just get character once and store as member

			//setting up each power with a button
			foreach (Powers pow in powerList) {
				GameObject go = Instantiate(button.gameObject);
                Button buttonInstance = go.GetComponent<Button>();
				go.transform.SetParent(menuBackground.transform, false);
				go.name = pow.powName + "(" + (count + 1) + ")";
				go.GetComponentInChildren<Text>().text = pow.powName;
				buttons.Add(buttonInstance);

				//setup for hover textbox, set to inactive 
				HoverButtonSetup(pow, buttonInstance);
				count++;
			}

			//back button setup
			if(transform.GetComponent<Character>().ChoiceOrder != 1) {
				GameObject tmp = Instantiate(button.gameObject, menuBackground.transform);

				goBackBtn = tmp.GetComponent<Button>();
				//goBackBtn.transform.SetParent(menuBG.transform, false);
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

		public void HoverButtonSetup(Powers pow, Button btn) {
			GameObject hoverTxt = Instantiate(hpTxt.gameObject);
			HoverText = hoverTxt.GetComponent<Text>();
			HoverText.transform.SetParent(canvas.transform, false);
			pow.Description = pow.description + "\nDamage: " + (pow.damage) + "\nUses: " + pow.statType + "\nMana Cost: " + pow.manaCost.ToString();
			btn.GetComponent<HoverTextStorage>().btnDescription = pow.description + "\nDamage: " + (pow.damage) + "\nUses: " + pow.statType + "\nMana Cost: " + pow.manaCost.ToString();
			HoverText.name = pow.powName + "hovertext";
			HoverText.gameObject.SetActive(false);
			HoverText.gameObject.transform.position = hoverTxtPos.position;
		}

		public void ShowButtons() {
			menuBackground.SetActive(true);
			charaNameText.gameObject.SetActive(true);
			hp.gameObject.SetActive(true);
			float hpScale = Mathf.Clamp01(CharacterCurrentHP / CharacterMaxHP);
			hp.fillAmount = hpScale;

			mp.gameObject.SetActive(true);
			float mpScale = Mathf.Clamp01(CharacterCurrentMP / CharacterMaxMP);
			mp.fillAmount = mpScale;

			mpBg.gameObject.SetActive(true);
			hpBg.gameObject.SetActive(true);
			hpTxt.gameObject.SetActive(true);
			hpTxt.text = CharacterCurrentHP.ToString() + "/" + CharacterMaxHP.ToString();
			mpTxt.gameObject.SetActive(true);
			mpTxt.text = CharacterCurrentMP.ToString() + "/" + CharacterMaxMP.ToString();


			foreach(Button btn in buttons) {
				playerActivated = false;
				btn.gameObject.SetActive(true);
			}
		}

		public void HideButtons() {
			menuBackground.SetActive(false);
			charaNameText.gameObject.SetActive(false);
			hp.gameObject.SetActive(false);
			mp.gameObject.SetActive(false);
			mpBg.gameObject.SetActive(false);
			hpBg.gameObject.SetActive(false);
			hpTxt.gameObject.SetActive(false);
			mpTxt.gameObject.SetActive(false);
			HoverText.gameObject.SetActive(false);

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
			//playerActivated = true;
			FindObjectOfType<TurnBehaviour>().RemoveAttack();
			undoMove = true;
		}

		//use to have button info pop up on screen/clear
		public void OnPointerEnter(BaseEventData data, int index) {
			HoverText.gameObject.SetActive(true);
			for(int i = 0; i < buttons.Count; i++) {
				if(buttons[index].GetComponentInChildren<Text>().text == powerList[i].powName) {
					string tmp = buttons[index].GetComponent<HoverTextStorage>().btnDescription;
					HoverText.text = buttons[index].GetComponent<HoverTextStorage>().btnDescription;
				}
			}
		}

		public void OnPointerExit(BaseEventData data, int index) {
			HoverText.gameObject.SetActive(false);
		}

        public void CleanUp()
        {
            //TODO 

            // Clear Buttons list
            buttons.Clear();
        }
	}
}
