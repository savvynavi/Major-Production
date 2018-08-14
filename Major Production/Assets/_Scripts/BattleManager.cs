using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Starts and ends battles, holding the player and enemy teams 
/// </summary>
public class BattleManager : MonoBehaviour {

	public static BattleManager Instance;

    public Transform playerTeam;
    public Transform enemyTeam; // Transform containing enemies to move into battle scene

	RPGsys.StateManager stateManager;

	private void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
		}
		else if (Instance != this)
		{
			Destroy(gameObject);
		}
		GameObject.DontDestroyOnLoad(this.gameObject);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void StartBattle(string sceneName, Transform enemies)
    {
        enemyTeam = enemies;

		// TODO any other setup for team

		SceneLoader.Instance.LoadBattle(sceneName);

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
        
        SceneLoader.Instance.EndBattle();
    }
}
