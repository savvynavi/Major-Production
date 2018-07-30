using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestEndFight : MonoBehaviour {

	// Use this for initialization
	void Start () {
        BattleManager manager = FindObjectOfType<BattleManager>();
        GetComponent<Button>().onClick.AddListener(manager.EndBattle);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
