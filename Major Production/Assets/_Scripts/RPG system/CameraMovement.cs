using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPGsys {
	public class CameraMovement : MonoBehaviour {
		public GameObject cameraMAIN;
		public GameObject cameraFace;
		public GameObject cameraAttack;

		public float faceOffset;

		Transform originalCameraTransform;
		Transform attackingCamAngle;
		Transform defendingCamAngle;

		// Use this for initialization
		void Start() {
			cameraAttack.SetActive(false);
			cameraFace.SetActive(false);

		}

		//looks at given characters front
		public void LookAtAttacker(Character attacker) {
			cameraMAIN.SetActive(false);
			cameraAttack.SetActive(false);
			cameraFace.SetActive(true);

			cameraFace.transform.position = new Vector3(0, 0, 0);
			cameraFace.transform.SetParent(attacker.transform);

			faceOffset = Mathf.Abs(faceOffset);

			//fix enemy not facing correct dir for camera
			CapsuleCollider tmp = attacker.GetComponent<CapsuleCollider>();
			if(attacker.tag == "Enemy") {
				faceOffset *= -1;
			}

			cameraFace.transform.position = new Vector3(attacker.transform.position.x + faceOffset, attacker.transform.position.y + tmp.height, attacker.transform.position.z);
			cameraFace.transform.LookAt(attacker.transform);
		}

		//looks at targeted characters from behind the attacker (good for attacks, bad for buffs/things targeting same team)
		public void LookAtTarget(Character attacker, Character target) {
			cameraMAIN.SetActive(false);
			cameraFace.SetActive(false);
			cameraAttack.SetActive(true);

			CapsuleCollider tmp = attacker.GetComponent<CapsuleCollider>();

			//cameraAttack.transform.SetParent(attacker.transform);
			cameraAttack.transform.position = new Vector3(attacker.transform.position.x - faceOffset, attacker.transform.position.y + tmp.height, attacker.transform.position.z);
			cameraAttack.transform.LookAt(target.transform);

		}

		//will look at whole group, centred around the average centre of them
		public void LookAtGroup(List<Character> targets) {
			cameraMAIN.SetActive(false);
			cameraFace.SetActive(false);
			cameraAttack.SetActive(true);

			faceOffset = Mathf.Abs(faceOffset);

			if(targets[0].tag == "Enemy") {
				faceOffset *= -1;
			}

			float xPositions = 0;
			foreach(Character chara in targets) {
				xPositions += chara.transform.position.x;
			}

			float averagePosition = xPositions / targets.Count;

			cameraAttack.transform.position = new Vector3(averagePosition - faceOffset, averagePosition + targets[0].GetComponent<CapsuleCollider>().height, targets[0].transform.position.z);
			cameraAttack.transform.LookAt(targets[0].transform);
		}

		//sets camera back to the main scene camera
		public void Reset() {
			Debug.Log("resetting camera");
			cameraMAIN.SetActive(true);
			cameraFace.SetActive(false);
			cameraAttack.SetActive(false);
		}
	}
}