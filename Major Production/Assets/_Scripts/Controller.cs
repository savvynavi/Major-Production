using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour {
	public float speed;

	CharacterController characterController;
	Animator anim;
	Vector3 moveDir;

	// Use this for initialization
	void Start () {
		characterController = GetComponent<CharacterController>();
		anim = GetComponent<Animator>();
		moveDir = Vector3.zero;

		anim.SetBool("isWalking", false);
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		//if(characterController.isGrounded){

		//basic blend tree stuff
		float x = Input.GetAxis("Horizontal");
		float y = Input.GetAxis("Vertical");
		Move(x, y);

		//movement code
		//moveDir = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
		//	moveDir = transform.TransformDirection(moveDir);
		//	//moveDir *= speed;

		//	characterController.Move(moveDir * Time.deltaTime);
		//	Debug.Log("moving chara");
		//}
	}

	//does animation movement 
	private void Move(float x, float y){
		anim.SetFloat("velX", x);
		anim.SetFloat("velY", y);

	}
}
