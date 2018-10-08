using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour {
	public float speed;
	public GameObject randCount;
	public float EncounterTimer; 
	public bool IsMoving { get { return characterController.velocity != Vector3.zero; } }

	CharacterController characterController;
	Animator anim;
	Vector3 moveDir;

	// TODO events for moving or not moving?

	// Use this for initialization
	void Start () {

		characterController = GetComponent<CharacterController>();
		anim = GetComponent<Animator>();
		moveDir = Vector3.zero;
	}
	
	//currently no isGrounded check
	void FixedUpdate () {
		//basic blend tree stuff
		float right = Input.GetAxis("Horizontal");
		float forward = Input.GetAxis("Vertical");
        anim.SetBool("isSprinting", Input.GetButton("Fire3"));

		Move(forward, right);
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
