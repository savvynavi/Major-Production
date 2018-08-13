using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO figure out lifecycle, make singleton

public class BattleManager : MonoBehaviour {

    public Transform playerTeam;
    public Transform enemyTeam; // Transform containing enemies to move into battle scene
    SceneLoader loader;

	RPGsys.StateManager stateManager;

	// Use this for initialization
	void Start () {
        loader = FindObjectOfType<SceneLoader>();
		GameObject.DontDestroyOnLoad(this.gameObject);

		//find state manager so we have access to character list
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void StartBattle(string sceneName, Transform enemies)
    {
        enemyTeam = enemies;

		// TODO any other setup for team

		loader.LoadBattle(sceneName);

	}

	public void EndBattle()
    {
		//TODO end of fight effects, cleanup, etc

		//finds the statemanager, loops over characters, removing effects
		stateManager = FindObjectOfType<RPGsys.StateManager>();

		foreach(RPGsys.Character chara in stateManager.characters) {
			foreach(RPGsys.Buff buff in chara.currentEffects) {
				buff.Remove(chara);
			}
		}

		playerTeam.gameObject.SetActive(false);
        
        loader.EndBattle();
    }
}
