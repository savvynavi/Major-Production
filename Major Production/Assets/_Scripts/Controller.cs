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
		float right = Input.GetAxis("Horizontal");
		float forward = Input.GetAxis("Vertical");
        anim.SetBool("isSprinting", Input.GetButton("Fire3"));

		Move(forward, right);

        if(Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0) {
			//encounters.RandomEncounter();
		}
	}

	//does animation movement 
	private void Move(float forward, float right){
        Transform cam = Camera.main.transform;
        // HACK won't work properly if camera isn't facing down
        Vector3 forwardAxis = Vector3.ProjectOnPlane(cam.up, Vector3.up);
        Vector3 rightAxis = Vector3.ProjectOnPlane(cam.right, Vector3.up);
        Vector3.OrthoNormalize(ref forwardAxis,ref rightAxis);

        Vector3 move = forward * forwardAxis + right * rightAxis;
        
        // Project movement onto character axes
		anim.SetFloat("velX", Vector3.Dot(transform.right,move));
		anim.SetFloat("velY", Vector3.Dot(transform.forward,move));
		
	}
}
