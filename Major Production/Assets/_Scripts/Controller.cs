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
//		float sprint = Input.GetAxis ("Fire3");

		Move(x, y);
	}

	//does animation movement 
	private void Move(float x, float y){
        //HACK fix this to use relative camera direction
        Vector3 move = new Vector3(y, 0, -x); 
        
        // Project movement onto character axes
		anim.SetFloat("velX", Vector3.Dot(transform.right,move));
		anim.SetFloat("velY", Vector3.Dot(transform.forward,move));
		if(Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0) {
			encounters.RandomEncounter();
		}
		if (Input.GetAxis ("Fire3") != 0) {
			anim.SetBool ("isSprinting", true);
		} 
		else {
			anim.SetBool ("isSprinting", false);
		}
	}
}
