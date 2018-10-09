using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG
{
	[RequireComponent(typeof(Collider))]
	public class InteractionUser : MonoBehaviour
	{
		// TODO reference to UI element showing item action

		Interactable selected;

		bool CanInteract = true;

		// Use this for initialization
		void Start()
		{

		}

		// Update is called once per frame
		void Update()
		{
			if(CanInteract &&
				selected != null &&
				Input.GetButtonDown("Interact"))
			{
				selected.Interact();
			}
		}

		public void Select(Interactable interactable)
		{
			selected = interactable;
			interactable.Hilight();
			//TODO show UI thing
			Debug.Log(interactable.ActionDescription + " selected");
		}

		public void Unselect(Interactable interactable)
		{
			selected = null;
			interactable.Unhilight();
			// TODO hide UI thing
			Debug.Log(interactable.ActionDescription + " unselected");
		}
	}
}