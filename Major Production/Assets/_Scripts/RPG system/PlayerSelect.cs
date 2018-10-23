using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPGsys {
	public class PlayerSelect : MonoBehaviour {

		new Camera camera = null;
		Ray ray;
		GameObject playerObject;
		StateManager manager;

		private void Start() {
			camera = Camera.main;
			manager = GetComponent<StateManager>();
		}

		// Update is called once per frame
		void Update() {
			//if the right-mouse button is clicked on a player character, it sets their status to active
			if(Input.GetMouseButton(1)) {
				ray = camera.ScreenPointToRay(Input.mousePosition);
				RaycastHit hit;
				if(Physics.Raycast(ray, out hit) && hit.transform.gameObject.tag == "Player" && hit.transform.GetComponent<Character>()) {
					foreach(Character chara in manager.characters) {
						chara.ActivePlayer = false;
					}
					hit.transform.GetComponent<Character>().ActivePlayer = true;
				}
			}
		}
	}

}