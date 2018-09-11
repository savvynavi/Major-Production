using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPGsys 
{
	public class CharacterListUI : MonoBehaviour {

		public StateManager manager;
		public CharacterUI prefab;
		public List<CharacterUI> uis;

		// Use this for initialization
		void Start() {

		}

		// Update is called once per frame
		void Update() {
			// make the uis if we havent done so yet, after the manager has called Start()
			if(uis.Count == 0) {
				uis = new List<CharacterUI>();
				for(int i=0; i< manager.characters.Count; i++) {
					GameObject go =Instantiate(prefab.gameObject, transform);
					uis[i] = go.GetComponent<CharacterUI>();
					uis[i].SetCharacter(manager.characters[i]);
				}

			}
		}
	}
}
