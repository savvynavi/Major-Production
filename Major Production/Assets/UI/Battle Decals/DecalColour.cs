using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecalColour : MonoBehaviour {
	Shader shader;

	public Color colour;

	private void Start() {
		shader = GetComponent<Shader>();
		if(shader == null) {
			Debug.Log("no shader");
		}

	}

}
