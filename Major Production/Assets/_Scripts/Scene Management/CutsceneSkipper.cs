using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CutsceneSkipper : MonoBehaviour {

	public Text skipText;
	public float minumumSceneTime;
	bool allowSkip = false;

	// Use this for initialization
	void Start () {
		skipText.text = "";
		allowSkip = false;
		StartCoroutine(AllowSkipRoutine());
	}
	
	// Update is called once per frame
	void Update () {
		if(SceneLoader.Instance.SceneReady && allowSkip)
		{
			skipText.text = "Press spacebar to skip...";
			if (Input.GetKeyDown(KeyCode.Space)){
				SceneLoader.Instance.AllowSceneActivation();
			}
		}
	}

	IEnumerator AllowSkipRoutine()
	{
		yield return new WaitForSecondsRealtime(minumumSceneTime);
		allowSkip = true;
	}
}
