using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO figure out lifecycle

public class BattleManager : MonoBehaviour {

    public Transform playerTeam;
    public Transform enemyTeam;
    SceneLoader loader;

	// Use this for initialization
	void Start () {
        loader = FindObjectOfType<SceneLoader>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void StartBattle(string sceneName, Transform enemies)
    {
        // May have to decide if this just clones enemies?
        enemyTeam = enemies;

        // TODO any other setup for team

        loader.LoadScene(sceneName);
    }

    public void EndBattle()
    {
        //TODO end of fight effects, cleanup, etc
        
        playerTeam.gameObject.SetActive(false);
        
        loader.EndScene();
    }
}
