using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestEndFight : MonoBehaviour {

	// Use this for initialization
	void Start () {
        SceneLoader loader = FindObjectOfType<SceneLoader>();
        GetComponent<Button>().onClick.AddListener(loader.EndBattle);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
