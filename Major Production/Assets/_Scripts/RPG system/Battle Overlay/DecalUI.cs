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
			public Character targets;
			public List<GameObject> projectors;
			public GameObject arrow;
		}

		public void InstantiateDecal(int i, Character character, Character target, TurnBehaviour turnBehaviour) {
			if(target != null) {
				int count = 0;
				for(int j = 0; j < turnBehaviour.MovesThisRound.Count; j++) {
					if(turnBehaviour.MovesThisRound[j].player.target == character.target) {
						count++;
					}
				}

				DecalInfo tmpInfo = new DecalInfo();

				tmpInfo.player = character;
				tmpInfo.targets = target;

				//disc spawning
				if(count > 0) {
					GameObject tmpObj = Instantiate(projector[count - 1]);
					tmpObj.transform.position = new Vector3(character.target.transform.position.x, tmpObj.transform.position.y, character.target.transform.position.z);
					List<GameObject> tmpList = new List<GameObject>();
					tmpList.Add(tmpObj);

					tmpInfo.projectors = tmpList;

					//wont spawn an arrow if it's a self/team targeting move, as the arrows look super janked if they do
					if(character.target.tag != "Player") {
						//arrow instantiating, spawns an arrow then rotates it towards the enemy from the character
						GameObject tmpArrow = Instantiate(arrowProjectors);
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

		//removes a specified decal
		public void RemoveDecal(DecalInfo givenInfo) {
			for(int i = 0; i < decalInfo.Count; i++) {
				if(decalInfo[i].player == givenInfo.player) {
					for(int k = 0; k < decalInfo[i].projectors.Count; k++) {
						Destroy(decalInfo[i].projectors[k]);

					}
					decalInfo[i].projectors.Clear();
					Destroy(decalInfo[i].arrow);

					decalInfo.Remove(decalInfo[i]);
					break;
				}
			}
		}

		//destroys all decals and clears the list
		public void ClearAll() {
			for(int i = 0; i < decalInfo.Count; i++) {
				for(int k = decalInfo[i].projectors.Count - 1; k >= 0; k--) {
					Destroy(decalInfo[i].projectors[k]);
				}
				Destroy(decalInfo[i].arrow);

				decalInfo[i].projectors.Clear();
			}

			decalInfo.Clear();
		}

	}

}