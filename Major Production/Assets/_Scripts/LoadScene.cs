using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour {
	public Button start;
	public Button controls;
	public Image controlImage;
	public Button back;
	public Button quit;

	GameObject controlUI;

	// Use this for initialization
	void Start () {
		controlUI = GameObject.Find("controlUI");
		start.onClick.AddListener(() => HandleClick(start));
		controls.onClick.AddListener(() => HandleClick(controls));
		back.onClick.AddListener(() => HandleClick(back));
		quit.onClick.AddListener(() => HandleClick(quit));
		controlUI.SetActive(false);
	}

	public void HandleClick(Button btn) {
		if(btn.GetComponentInChildren<Text>().text == "Start") {
			SceneManager.LoadScene("Testing", LoadSceneMode.Single);
		} else if(btn.GetComponentInChildren<Text>().text == "Controls") {
			Debug.Log(btn.GetComponentInChildren<Text>().text);
			controlUI.SetActive(true);
		}else if(btn.GetComponentInChildren<Text>().text == "Back") {
			controlUI.SetActive(false);
		}else if(btn.GetComponentInChildren<Text>().text == "Quit") {
			Application.Quit();
		}
	}
}
