using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour {
	public float speed;
	public GameObject randCount;
	public float EncounterTimer; 

	CharacterController characterController;
	EncounterController encounters;
	Animator anim;
	Vector3 moveDir;


	// Use this for initialization
	void Start () {

		characterController = GetComponent<CharacterController>();
		anim = GetComponent<Animator>();
		moveDir = Vector3.zero;

		encounters = randCount.GetComponent<EncounterController>();
	}
	
	//currently no isGrounded check
	void FixedUpdate () {
		//basic blend tree stuff
		float x = Input.GetAxis("Horizontal");
		float y = Input.GetAxis("Vertical");
		
		Move(x, y);
	}

	//does animation movement 
	private void Move(float x, float y){
		anim.SetFloat("velX", x);
		anim.SetFloat("velY", y);
		if(Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0) {
			encounters.RandomEncounter();
		}
	}
}
