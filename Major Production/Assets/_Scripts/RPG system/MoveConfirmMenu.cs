using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace RPGsys {
	public class MoveConfirmMenu : MonoBehaviour {

		StateManager stateManager;

		public Button button;
		public GameObject canvas;
		public Transform menuPos;
		public Image menuBG;

		Button goBackTurn;

		void Start() {
			stateManager = GetComponent<StateManager>();
		}

		public void Setup() {
			GameObject tmpPaper = Instantiate(menuBG.gameObject);
			menuBG = tmpPaper.GetComponent<Image>();
			menuBG.transform.SetParent(canvas.transform, false);
			menuBG.transform.position = menuPos.transform.position;


			GameObject go = Instantiate(button.gameObject);
			button = go.GetComponent<Button>();
			button.transform.SetParent(menuBG.transform, false);
			button.GetComponentInChildren<Text>().text = "Confirm Moves";

			go = Instantiate(button.gameObject);
			goBackTurn = go.GetComponent<Button>();
			goBackTurn.transform.SetParent(menuBG.transform, false);
			goBackTurn.GetComponentInChildren<Text>().text = "Go Back?";

			button.onClick.AddListener(() => HandleClick(button));
			goBackTurn.onClick.AddListener(() => HandleClick(goBackTurn));
			HideMenu();
		}

		public void ShowMenu() {
			menuBG.gameObject.SetActive(true);
			button.gameObject.SetActive(true);
			goBackTurn.gameObject.SetActive(true);
		}

		public void HideMenu() {
			menuBG.gameObject.SetActive(false);
			button.gameObject.SetActive(false);
			goBackTurn.gameObject.SetActive(false);

		}

		//when button clicked, does this SET UP SO IT CAN DO DIFFERENT IENUM THINGS
		public void HandleClick(Button btn) {
			stateManager.confirmMoves = true;
			if(btn.GetComponentInChildren<Text>().text == "Go Back?") {
				stateManager.redoTurn = true;
				Debug.Log("redoing turn");
			}
		}
	}
}
