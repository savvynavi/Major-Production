using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestEndFight : MonoBehaviour {

	// Use this for initialization
	void Start () {
        GetComponent<Button>().onClick.AddListener(RPG.BattleManager.Instance.EndBattle);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
