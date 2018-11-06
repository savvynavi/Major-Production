using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine;

namespace RPGsys {
	public class PlayerSelect : MonoBehaviour {

		new Camera camera = null;
		Ray ray;
		GameObject playerObject;
		StateManager manager;

		static List<RaycastResult> hits = new List<RaycastResult>();

		private void Start() {
			camera = Camera.main;
			manager = GetComponent<StateManager>();
		}

		// Update is called once per frame
		void Update() {
			//if the right-mouse button is clicked on a player character, it sets their status to active
			if(Input.GetMouseButton(1)) {
				Debug.Log("mouse hit right");
				ray = camera.ScreenPointToRay(Input.mousePosition);
				RaycastHit hit;
				if(Physics.Raycast(ray, out hit) && hit.transform.gameObject.tag == "Player" && hit.transform.GetComponent<Character>()) {
					foreach(Character chara in manager.characters) {
						chara.ActivePlayer = false;
					}
					hit.transform.GetComponent<Character>().ActivePlayer = true;
					//makes it so that the player transitions into correct animation the second they're clicked
					for(int i = 0; i < manager.characters.Count; i++) {
						manager.MoveSetCheck(i);
					}
				}
			}

			//portrait clicks change character 
			if(Input.GetMouseButton(0)) {
				HudCharacterClick();
			}
		}

		//lets you right click the portraits to change the active player too
		private void HudCharacterClick() {
			Debug.Log("hud called");
			PointerEventData pointer = new PointerEventData(EventSystem.current);
			pointer.position = Input.mousePosition;
			EventSystem.current.RaycastAll(pointer, hits);
			//loops over all hits, if the gameobject has a charaUI component it will set only that character to active/change the animations
			foreach(RaycastResult hit in hits) {
				CharacterUI charaUi = hit.gameObject.GetComponentInParent<CharacterUI>();
				if(charaUi != null ) {
					Debug.Log("in loop");
					foreach(Character chara in manager.characters) {
						chara.ActivePlayer = false;
					}
					hit.gameObject.GetComponentInParent<CharacterUI>().character.ActivePlayer = true;
					for(int i = 0; i < manager.characters.Count; i++) {
						manager.MoveSetCheck(i);
					}
				}
			}
		}
	}
}