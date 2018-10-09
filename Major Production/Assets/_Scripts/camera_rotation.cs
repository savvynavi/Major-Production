using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Utility;

public class camera_rotation : MonoBehaviour {

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

	void OnTriggerEnter (Collider collider){
		if (collider.tag == "Player") {
			smoothFollow.height = 10;
			Debug.Log ("Height changed to 10");
		}
	}
}
