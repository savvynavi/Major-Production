using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPGsys {
	public class DecalUI : MonoBehaviour {

		public List<GameObject> projector;
		public GameObject arrowProjectors;

		public List<DecalInfo> decalInfo;

		[System.Serializable]
		public struct DecalInfo {
			public Character player;
			public List<Character> targets;
			public List<GameObject> projectors;
			public GameObject arrow;
		}
		
		public void InstantiateDecal(int count, Character character, List<Character> target) {
			if(target.Count < 0) {
				int element = 0;
				for(int i = 0; i < count; i++) {
					if(target == decalInfo[i].targets) {
						element++;
					}
				}

				DecalInfo tmpInfo = new DecalInfo();


				//disc spawning
				GameObject tmpObj = Instantiate(projector[count - 1]);
				tmpObj.transform.position = new Vector3(character.target.transform.position.x, tmpObj.transform.position.y, character.target.transform.position.z);
				tmpInfo.projectors.Add(tmpObj);

				//wont spawn an arrow if it's a self/team targeting move, as the arrows look super janked if they do
				if(character.target.tag != "Player") {
					//arrow instantiating, spawns an arrow then rotates it towards the enemy from the character
					//TODO add in arrow delete parts, figure out how to colour the rings
					GameObject tmpArrow = Instantiate(tmpInfo.arrow);
					Vector3 midPoint = (character.transform.position + (character.target.transform.position - character.transform.position) / 2);
					tmpArrow.transform.position = new Vector3(midPoint.x, tmpObj.transform.position.y, midPoint.z);

					//scales the arrow so it fits between the characters
					float distance = Vector3.Distance(character.transform.position, character.target.transform.position);
					tmpArrow.GetComponent<Projector>().orthographicSize = distance / 2;
					tmpArrow.GetComponent<Projector>().aspectRatio = 1 / Mathf.Pow(tmpArrow.GetComponent<Projector>().orthographicSize, 2);

					tmpArrow.transform.LookAt(character.target.transform);
					tmpArrow.transform.eulerAngles = new Vector3(90, tmpArrow.transform.eulerAngles.y, tmpArrow.transform.eulerAngles.z);

					tmpInfo.arrow = tmpArrow;
					
				}

				//storing the decal info into a list
				decalInfo.Add(tmpInfo);

			}
		}

	}

}