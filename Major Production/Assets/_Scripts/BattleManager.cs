﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


namespace RPG
{
	/// <summary>
	/// Starts and ends battles, holding the player and enemy teams 
	/// </summary>
	public class BattleManager : MonoBehaviour
	{

		public static BattleManager Instance;

		public Encounter encounter;
		public Transform playerTeam;
		public Transform enemyTeam; // Transform containing enemies to move into battle scene

		public RPGsys.StateManager stateManager;

		UnityEvent OnWin;
		UnityEvent OnLoss;

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

		public RPGsys.StateManager GetStateManager() { return stateManager; }

		// Update is called once per frame
		void Update()
		{

		}

		public void StartBattle(string sceneName, Encounter e, UnityEvent winEvent = null, UnityEvent lossEvent = null)
		{
			OnWin = winEvent;
			OnLoss = lossEvent;
			encounter = e;
			enemyTeam = encounter.InstantiateEnemyTeam().transform;

			// If this takes up time maybe move to AyncBattleLoad?
			GameController.Instance.Autosave();

			// TODO any other setup for team

			SceneLoader.Instance.LoadBattle(sceneName);

		}

		public void EndBattle()
		{
			//TODO end of fight effects, cleanup, etc

			//finds the statemanager, loops over characters, removing effects
			//stateManager = FindObjectOfType<RPGsys.StateManager>();


			List<RPGsys.Buff> deadlist = new List<RPGsys.Buff>();
			foreach (RPGsys.Character chara in stateManager.characters)
			{
				foreach (RPGsys.Buff buff in chara.currentEffects)
				{
					if (buff.equipable == RPGsys.Status.Equipable.False)
					{
						//chara.currentEffects.Remove(buff);
						deadlist.Add(buff);
						buff.Remove(chara);
					}
				}
				if (deadlist.Count > 0)
				{
					foreach (RPGsys.Buff buff in deadlist)
					{
						chara.currentEffects.Remove(buff);
					}
					deadlist.Clear();
				}

			}

			playerTeam.gameObject.SetActive(false);

			if (stateManager.Alive())
			{
				SceneLoader.Instance.EndBattle();
				OnWin.Invoke();
			}
			else
			{
				// TODO check if lose effect set, and do instead
				if (OnLoss != null)
				{
					OnLoss.Invoke();
				}
				else
				{
					GameController.Instance.Autoload();
				}
			}
			stateManager = null;
		}
	}
}