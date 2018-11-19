using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPGsys {
	public class CharacterButtonList : MonoBehaviour {

		public StateManager manager;
		public CharacterButtonUI prefab;
		public List<CharacterButtonUI> uis;


		// Update is called once per frame
		//void Update() {
		//	//if no uis, creats one
		//	if(uis.Count == 0) {
		//		uis = new List<CharacterButtonUI>();
		//		for(int i = 0; i < manager.characters.Count; i++) {
		//			GameObject go = Instantiate(prefab.gameObject, transform);
					
		//			CharacterButtonUI tmp = go.GetComponent<CharacterButtonUI>();

		//			(go.transform as RectTransform).anchoredPosition = Vector3.zero;
		//			uis.Add(tmp);
		//			tmp.SetButtons(manager.characters[i]);
		//		}
		//	}
		//}

		private void Awake(){
			uis = new List<CharacterButtonUI>();
		}

		public void PopulateList(List<Character> characters){
			foreach(CharacterButtonUI ui in uis){
				GameObject.Destroy(ui.gameObject);
			}
			uis.Clear();
			for (int i = 0; i < characters.Count; i++){
				GameObject go = Instantiate(prefab.gameObject, transform);

				CharacterButtonUI tmp = go.GetComponent<CharacterButtonUI>();

				(go.transform as RectTransform).anchoredPosition = Vector3.zero;
				uis.Add(tmp);
				tmp.SetButtons(characters[i], manager);
			}
		}
	}

}