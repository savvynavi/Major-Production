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
		public Transform tmpPos;

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
		}

		public void SetInteractable() {
			if(turnBehaviour.numOfTurns <= 0) {
				btn.GetComponent<Button>().interactable = true;
			} else {
				btn.GetComponent<Button>().interactable = false;
			}
		}

		public void HandleClick() {
			manager.PlayerTurnOver = true;
		}

	}

}