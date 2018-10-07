using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadScreen : MonoBehaviour {

	[SerializeField] Text infoText;
	[SerializeField] Slider progressBar;

	// Use this for initialization
	void Start () {
		gameObject.SetActive(false);
	}
	
	public void BeginLoading(string scene)
	{
		gameObject.SetActive(true);
		infoText.text = "Loading " + scene;
		progressBar.value = 0;
	}

	public void UpdateProgress(float progress)
	{
		progressBar.value = progress / 0.9f;
	}

	public void FinishLoading()
	{
		gameObject.SetActive(false);
	}
}
