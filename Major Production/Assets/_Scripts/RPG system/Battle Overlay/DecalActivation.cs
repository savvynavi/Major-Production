using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecalActivation : MonoBehaviour {

	GameObject character;
	List<Projector> projectors;

	public void ActivateDecal(GameObject obj, RPGsys.TurnBehaviour turnInfo) {
		projectors = new List<Projector>();
		character = obj;
		foreach(Transform tr in character.transform) {
			if(tr.GetComponentInChildren<Projector>() != null) {
				projectors.Add(tr.GetComponentInChildren<Projector>());
			}
		}



		int count = 0;
		//activate decals dependant on how many moves in list are targeting this currently
		for(int i = 0; i < turnInfo.MovesThisRound.Count; i++) {
			if(turnInfo.MovesThisRound[i].player.target == obj.GetComponent<RPGsys.Character>()) {
				count++;
			}
		}

		for(int i = 0; i < projectors.Count; i++) {
			if(i <= count) {
				projectors[i].gameObject.SetActive(true);
			} else {
				projectors[i].gameObject.SetActive(false);
			}
			
		}
	}
}
