using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG
{
	public class FootstepEffect : MonoBehaviour
	{
		public AudioClip defaultFootstep;
		public float volume = 1;
		[Header("Raycast Settings")]
		[SerializeField] LayerMask footstepLayers = LayerMask.NameToLayer("Terrain");
		[SerializeField] float maxDistance = 2.0f;
		// Use this for initialization
		void Start()
		{

		}

		// Update is called once per frame
		void Update()
		{

		}

		/// <summary>
		/// Do effects for 
		/// </summary>
		/// <param name="step">0 for right, 1 for left</param>
		public void Footstep(int step)
		{
			RaycastHit hit;
			if(Physics.Raycast(new Ray(transform.position, Vector3.down),out hit, maxDistance,footstepLayers)){
				Audio.FootstepSounds footstepSounds = hit.collider.gameObject.GetComponent<Audio.FootstepSounds>();
				AudioClip footstep = defaultFootstep;
				Vector3 footfallPoint = hit.point;
				if (footstepSounds != null)
				{
					AudioClip groundClip = footstepSounds.GetFootstep(footfallPoint);
					if(groundClip != null)
					{
						footstep = groundClip;
					}
				}
				AudioSource.PlayClipAtPoint(footstep, footfallPoint, volume);
			}
		}
	}
}