using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG
{
	[RequireComponent(typeof(Collider))]
	public abstract class Interactable : MonoBehaviour
	{
		public string ActionDescription;

		public abstract void Interact();

		public abstract void Hilight();

		public abstract void Unhilight();

		private void OnTriggerEnter(Collider other)
		{
			InteractionUser user = other.GetComponent<InteractionUser>();
			if(user != null && IsFacingMe(other.transform))
			{
				user.Select(this);
			}
		}

		private void OnTriggerStay(Collider other)
		{
			InteractionUser user = other.GetComponent<InteractionUser>();
			if (user != null)
			{
				if (IsFacingMe(other.transform))
				{
					user.Select(this);
				}else
				{
					user.Unselect(this);
				}
			}
		}

		private void OnTriggerExit(Collider other)
		{
			InteractionUser user = other.GetComponent<InteractionUser>();
			if (user != null)
			{
				user.Unselect(this);
			}
		}

		private bool IsFacingMe(Transform other)
		{
			// Maybe only worry about x-z plane?
			Vector3 displacement = transform.position - other.position;
			return Vector3.Dot(other.forward, displacement) > 0;
		}
	}
}