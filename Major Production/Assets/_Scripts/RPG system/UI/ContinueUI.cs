using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace RPGsys {
	public class ContinueUI : MonoBehaviour {
		public Button btn;
		public StateManager manager;
		public TurnBehaviour turnBehaviour;

		Button continueButton;

		// Use this for initialization
		void Start() {
			manager = GetComponent<StateManager>();
			turnBehaviour = GetComponent<TurnBehaviour>();
			
		}

		public void Setup(Image panel, Canvas canvas) {
			GameObject go = Instantiate(btn.gameObject);
			continueButton = go.GetComponent<Button>();
			continueButton.transform.SetParent(panel.transform, false);

			continueButton.onClick.AddListener(() => HandleClick());
		}

		//once all characters have a move set the button can be clicked
		public void SetInteractable() {
			if(turnBehaviour.numOfTurns <= 0) {
				continueButton.GetComponent<Button>().interactable = true;
			} else {
				continueButton.GetComponent<Button>().interactable = false;
			}
		}

		public void HandleClick() {
			manager.PlayerTurnOver = true;
		}

		//if pass in true, activates the button in the hierarchy, if false deactivates it
		public void Active(bool active) {
			continueButton.gameObject.SetActive(active);
		}

	}

}