using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPGsys {
	public class TurnBehaviour : MonoBehaviour {
		ButtonBehaviour button = null;

		public List<Powers> abilitiesThisRound;
		public List<Character> AvailablePlayers;
		public List<Character> AvailableEnemies;
		public List<TurnInfo> MovesThisRound;
		public int numOfTurns;
		public int numOfEnemyTurns;

		[System.Serializable]
		public struct TurnInfo{
			public Powers ability;
			public Character player;
		}

		// Use this for initialization
		void Start() {
			
		}

		public void Setup(List<Character> players, List<Character> enemies) {
			AvailablePlayers = players;
			AvailableEnemies = enemies;

			//find better solution won't work w/ mult button
			button = FindObjectOfType<ButtonBehaviour>().GetComponent<ButtonBehaviour>();
			numOfTurns = AvailablePlayers.Count;
			numOfEnemyTurns = AvailableEnemies.Count;
		}

		//creates temp struct to hold passed in values then adds this to the list of moves
		public void TurnAddAttack(Powers pow, Character chara) {
			if(numOfTurns > 0) {
				TurnInfo tmp;
				tmp.ability = pow;
				tmp.player = chara;
				numOfTurns--;
				MovesThisRound.Add(tmp);
			}
		}

		//seperates out enemy movement turns from player
		public void turnAddAttackEnemy(Powers pow, Character chara) {
			if(numOfEnemyTurns > 0) {
				TurnInfo tmp;
				tmp.ability = pow;
				tmp.player = chara;
				numOfEnemyTurns--;
				MovesThisRound.Add(tmp);
			}
		}
	}
}

