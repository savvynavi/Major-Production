using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraWipe : MonoBehaviour {

	bool wiping;
	public Material wipeMaterial;
	
	public Texture WipeTexture { set { wipeMaterial.SetTexture("_TransitionTex", value); } }
	public float Cutoff {
		get { return wipeMaterial.GetFloat("_Cutoff"); }
		set { wipeMaterial.SetFloat("_Cutoff", value); }
	}

	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		if (wipeMaterial != null)
		{
			Graphics.Blit(source, destination, wipeMaterial);
		}
	}
}
