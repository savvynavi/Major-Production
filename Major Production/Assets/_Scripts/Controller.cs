using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour {
	public float speed;
	public float rotation;
	public GameObject randCount;
	public float EncounterTimer; 
	public bool IsMoving { get { return characterController.isGrounded && characterController.velocity != Vector3.zero; } }
	bool frozen = false;

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
	void FixedUpdate()
	{
		//basic blend tree stuff
		float right = Input.GetAxis("Horizontal");
		float forward = Input.GetAxis("Vertical");
		//anim.SetBool("isSprinting", Input.GetButton("Sprint"));
		if (!frozen)
		{
			Move(forward, right);
		}
		else
		{
			anim.SetBool("isWalking", false);
		}
	}

	//does animation movement 
	private void Move(float forward, float right)
	{
		Transform cam = Camera.main.transform;
		// HACK won't work properly if camera isn't facing down
		Vector3 forwardAxis = Vector3.ProjectOnPlane(cam.up, Vector3.up);
		Vector3 rightAxis = Vector3.ProjectOnPlane(cam.right, Vector3.up);
		Vector3.OrthoNormalize(ref forwardAxis, ref rightAxis);

		// HACK just to force onto ground will write this later
		Vector3 gravity = 10 * Time.fixedDeltaTime * Vector3.down;

		Vector3 move = speed * Time.fixedDeltaTime * (forward * forwardAxis + right * rightAxis);
		characterController.Move(move + gravity);

		Vector3 horizontalVelocity = characterController.velocity;
		horizontalVelocity.y = 0;
		if (horizontalVelocity.magnitude > 0)
		{
			// HACK make this nicer
			transform.forward = Vector3.RotateTowards(transform.forward, horizontalVelocity.normalized, rotation * Time.fixedDeltaTime, 0);
			anim.SetBool("isWalking", true);
		}
		else
		{
			anim.SetBool("isWalking", false);
		}
	}

	public void Freeze()
	{
		frozen = true;
	}

	public void Unfreeze()
	{
		frozen = false;
	}
}
