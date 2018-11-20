using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinScreenController : MonoBehaviour {

	// Use this for initialization
	void Start () {
		GameController.Instance.state = GameController.EGameStates.Cutscene;
	}
	
	// Update is called once per frame
	void Update () {
		// TODO have minimum watch time?
		if (Input.GetKeyDown(KeyCode.Space))
		{
			GameController.Instance.QuitToTitle();
		}
	}
}
