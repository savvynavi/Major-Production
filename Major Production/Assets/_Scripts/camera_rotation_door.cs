using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Utility;

public class camera_rotation_door : MonoBehaviour {

	[SerializeField]
	public GameObject gameObject;

	private SmoothFollow smoothFollow;
	// Use this for initialization
	void Start () {
		smoothFollow = gameObject.GetComponent<SmoothFollow>();
	}

	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerStay (Collider collider){
		if (collider.tag == "Player") {
			smoothFollow.height = 2;
			Debug.Log ("Height changed to 2");
		}
	}
	void OnTriggerExit (Collider collider){
		if (collider.tag == "Player") {
			smoothFollow.height = 9;
			Debug.Log ("Height changed to 9");
		}
	}
}
