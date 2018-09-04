using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPGsys {
	public class CameraMovement : MonoBehaviour {
		public GameObject cameraMAIN;
		public GameObject cameraFace;
		public GameObject cameraAttack;

		public float faceOffset;
		public float attackOffset;

		Transform originalCameraTransform;
		Transform attackingCamAngle;
		Transform defendingCamAngle;

		public Camera CurrentCamera { get; private set; }

		// Use this for initialization
		void Start() {
			cameraAttack.SetActive(false);
			cameraFace.SetActive(false);
			CurrentCamera = cameraMAIN.GetComponent<Camera>();
		}

		//looks at given characters front
		public void LookAtAttacker(Character attacker) {
			cameraMAIN.SetActive(false);
			cameraAttack.SetActive(false);
			cameraFace.SetActive(true);

			CurrentCamera = cameraFace.GetComponent<Camera>();

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

			CurrentCamera = cameraAttack.GetComponent<Camera>();


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

			CurrentCamera = cameraAttack.GetComponent<Camera>();


			attackOffset = Mathf.Abs(attackOffset);

			if(targets[0].tag == "Enemy") {
				attackOffset *= -1;
			}

			//get average position vector of group
			List<Vector3> positions = new List<Vector3>();
			foreach(Character chara in targets) {
				positions.Add(chara.transform.position);
			}

			Vector3 meanPos = AveragePosition(positions);

			//set the camera to look at the group from the front from average position
			cameraAttack.transform.position = new Vector3(meanPos.x + attackOffset, targets[0].GetComponent<CapsuleCollider>().height, meanPos.z);
			cameraAttack.transform.LookAt(meanPos);
		}

		//sets camera back to the main scene camera
		public void Reset() {
			Debug.Log("resetting camera");
			cameraMAIN.SetActive(true);
			cameraFace.SetActive(false);
			cameraAttack.SetActive(false);

			CurrentCamera = cameraMAIN.GetComponent<Camera>();

		}

		//returns the averave position vector of given list of positions
		private Vector3 AveragePosition(List<Vector3> positions) {
			if(positions.Count == 0) {
				return Vector3.zero;
			}

			Vector3 meanVector = Vector3.zero;
			foreach(Vector3 pos in positions) {
				meanVector += pos;
			}

			return (meanVector / positions.Count);
		}
	}
}