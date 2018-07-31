using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO figure out lifecycle

public class BattleManager : MonoBehaviour {

    public Transform playerTeam;
    public Transform enemyTeam; // Transform containing enemies to move into battle scene
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
