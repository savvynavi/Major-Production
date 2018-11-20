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

					//setting colour to character specific colour
					Material tmpMat = new Material(tmpObj.GetComponent<Projector>().material);
					tmpMat.SetColor("_Color", character.uiColour);
					tmpObj.GetComponent<Projector>().material = tmpMat;

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

						//setting colour to character specific colour
						Material tmpMatArr = new Material(tmpArrow.GetComponent<Projector>().material);
						tmpMatArr.SetColor("_Color", character.uiColour);
						tmpArrow.GetComponent<Projector>().material = tmpMatArr;

						tmpInfo.arrow = tmpArrow;

					}
					//storing the decal info into a list
					decalInfo.Add(tmpInfo);
				}
			}
		}

		//removes a specified decal
		public void RemoveDecal(DecalInfo givenInfo, TurnBehaviour turnBehav) {
			List<Character> tmpList = new List<Character>();
			for(int i = decalInfo.Count - 1; i >= 0; i--) {
				if(decalInfo[i].targets == givenInfo.targets) {
					for(int k = 0; k < decalInfo[i].projectors.Count; k++) {
						Destroy(decalInfo[i].projectors[k]);

					}
					decalInfo[i].projectors.Clear();
					Destroy(decalInfo[i].arrow);

					if(decalInfo[i].player != givenInfo.player) {
						tmpList.Add(decalInfo[i].player);
					}

					decalInfo.Remove(decalInfo[i]);
				}
			}


			int count = decalInfo.Count;
			//respawns all other decals that should be on this target to fill in any gaps removing the other one left
			for(int i = 0; i < tmpList.Count; i++) {
				RedoDecal(i, tmpList[i], tmpList[i].target.GetComponent<Character>(), turnBehav, tmpList.Count);
			}
		}

		//using th eother one for redoing the decals didn't work and editing it was too difficult so here this is, a slightly modded version of the instantiate code that uses a passed in number to grab the ring decal element
		public void RedoDecal(int i, Character character, Character target, TurnBehaviour turnBehaviour, int iter) {
			if(target != null) {
				DecalInfo tmpInfo = new DecalInfo();

				tmpInfo.player = character;
				tmpInfo.targets = target;

				//disc spawning
				if(iter > 0) {
					GameObject tmpObj = Instantiate(projector[i]);
					tmpObj.transform.position = new Vector3(character.target.transform.position.x, tmpObj.transform.position.y, character.target.transform.position.z);

					//setting colour to character specific colour
					Material tmpMat = new Material(tmpObj.GetComponent<Projector>().material);
					tmpMat.SetColor("_Color", character.uiColour);
					tmpObj.GetComponent<Projector>().material = tmpMat;
					//tmpObj.GetComponent<Projector>().material.SetColor("_Color", character.uiColour);
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

						//setting colour to character specific colour
						Material tmpMatArr = new Material(tmpArrow.GetComponent<Projector>().material);
						tmpMatArr.SetColor("_Color", character.uiColour);
						tmpArrow.GetComponent<Projector>().material = tmpMatArr;
						//tmpArrow.GetComponent<Projector>().material.SetColor("_Color", character.uiColour);



						tmpInfo.arrow = tmpArrow;

					}
					//storing the decal info into a list
					decalInfo.Add(tmpInfo);
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