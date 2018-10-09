using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace RPGsys {
	public class MoveConfirmMenu : MonoBehaviour {

		StateManager stateManager;

		Button confirmButton;
		Canvas canvas;
		Image menuBG;

		Button goBackTurn;

		void Start() {
			stateManager = GetComponent<StateManager>();
		}

		public void Setup(Button button, Image panel, Canvas canvas) {

			GameObject tmpPaper = Instantiate(panel.gameObject);
			menuBG = tmpPaper.GetComponent<Image>();
			menuBG.transform.SetParent(canvas.transform, false);
			menuBG.transform.position = panel.transform.position;


			GameObject go = Instantiate(button.gameObject);
			confirmButton = go.GetComponent<Button>();
			confirmButton.transform.SetParent(menuBG.transform, false);
			confirmButton.GetComponentInChildren<Text>().text = "Confirm Moves";

			go = Instantiate(button.gameObject);
			goBackTurn = go.GetComponent<Button>();
			goBackTurn.transform.SetParent(menuBG.transform, false);
			goBackTurn.GetComponentInChildren<Text>().text = "Go Back?";

			confirmButton.onClick.AddListener(() => HandleClick(button));
			goBackTurn.onClick.AddListener(() => HandleClick(goBackTurn));
			HideMenu();
		}

		public void ShowMenu() {
			menuBG.gameObject.SetActive(true);
			confirmButton.gameObject.SetActive(true);
			goBackTurn.gameObject.SetActive(true);
		}

		public void HideMenu() {
			menuBG.gameObject.SetActive(false);
			confirmButton.gameObject.SetActive(false);
			goBackTurn.gameObject.SetActive(false);

		}

		//when button clicked, does this SET UP SO IT CAN DO DIFFERENT IENUM THINGS
		public void HandleClick(Button btn) {
			stateManager.confirmMoves = true;
			if(btn.GetComponentInChildren<Text>().text == "Go Back?") {
				stateManager.redoTurn = true;
			}
		}
	}
}
