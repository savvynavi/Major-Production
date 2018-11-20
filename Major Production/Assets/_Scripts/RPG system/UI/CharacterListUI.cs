using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPGsys 
{
	public class CharacterListUI : MonoBehaviour {

		public StateManager manager;
		public CharacterUI prefab;
		public List<CharacterUI> uis;

		// Update is called once per frame
		void Update() {
			// make the uis if we havent done so yet, after the manager has called Start()
			if(uis.Count == 0) {
				uis = new List<CharacterUI>();
				if(prefab.GetComponent<CharacterUI>().FloatingMenu == true) {
					for(int i = 0; i < manager.characters.Count; i++) {
						GameObject go = Instantiate(prefab.gameObject, transform);
						CharacterUI tmp = go.GetComponent<CharacterUI>();
						uis.Add(tmp);
						tmp.SetCharacter(manager.characters[i]);
					}
				} else {
					//creates only 1 for the menu
					GameObject go = Instantiate(prefab.gameObject, transform);
					CharacterUI tmp = go.GetComponent<CharacterUI>();
					uis.Add(tmp);
					tmp.SetCharacter(manager.characters[0]);
				}
				
			}
		}

		public void UpdateHalos() {
			foreach(CharacterUI ui in uis) {
				if(ui.Halo != null) {
					ui.Halo.gameObject.SetActive(false);
				}
			}
		}

		public void ActivateHalo(Character chara) {
			foreach(CharacterUI ui in uis) {
				if(ui.Halo != null && ui.character == chara) {
					ui.Halo.gameObject.SetActive(true);
				}
			}
		}

		//updates the character info to given character (used for menu portrait)
		public void UpdateInfo(Character chara) {
			uis[0].SetCharacter(chara);
		}
	}
}
