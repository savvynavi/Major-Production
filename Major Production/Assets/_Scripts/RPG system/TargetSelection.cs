﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPGsys {
	public class TargetSelection : MonoBehaviour {
		new Camera camera = null;
		Ray ray;
		Character target = null;

		public GameObject selector;

		// Use this for initialization
		private void Start() {
			camera = Camera.main;
		}

        public void Init(GameObject selector, Camera camera){
            this.selector = selector;
            this.camera = camera;
        }

        // Update is called once per frame
        void Update() {
			if(Input.GetMouseButton(0)) {
				ray = camera.ScreenPointToRay(Input.mousePosition);
				Debug.DrawRay(ray.origin, ray.direction * 10, Color.blue);
				RaycastHit hit;
				//if the ray hits something and the thing has a character component, move the yellow platform and set the current target to this
				if(Physics.Raycast(ray, out hit) && hit.transform.GetComponent<Character>()) {
					target = hit.transform.GetComponent<Character>();
					transform.GetComponent<Character>().target = hit.transform.gameObject;
				}
			}


			//selector movement code
			if(transform.GetComponent<Character>().target != null) {
				selector.GetComponent<StateManager>().selector.SetActive(true);
				Vector3 pos = new Vector3(transform.GetComponent<Character>().target.transform.position.x, selector.GetComponent<StateManager>().selector.transform.position.y, transform.GetComponent<Character>().target.transform.position.z);
				selector.GetComponent<StateManager>().selector.transform.position = pos;
			}

		}
	}

}
