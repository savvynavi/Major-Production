using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPGsys {
	public class CameraMovement : MonoBehaviour {
		public Camera camera;

		Transform originalCameraTransform;
		Transform attackingCamAngle;
		Transform defendingCamAngle;

		// Use this for initialization
		void Start() {
			//originalCameraTransform.rotation = camera.transform.rotation;
			originalCameraTransform = camera.transform;
		}

		public void SetTransforms(TurnBehaviour.TurnInfo info) {
			attackingCamAngle = info.player.transform;
			defendingCamAngle = info.player.target.transform;
		}

		public void LookAtAttacker() {
			camera.transform.LookAt(attackingCamAngle);
		}

		public void LookAtTarget() {
			camera.transform.LookAt(defendingCamAngle);
		}

		public void Reset() {
			//camera.transform.rotation = originalCameraTransform.rotation;
			camera.transform.rotation = originalCameraTransform.rotation;
			
		}

	}
}