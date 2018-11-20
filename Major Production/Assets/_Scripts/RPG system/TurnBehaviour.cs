using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPGsys {
	public class TurnBehaviour : MonoBehaviour {
		public List<Powers> abilitiesThisRound;
		public List<Character> AvailablePlayers;
		public List<Character> AvailableEnemies;
		public List<TurnInfo> MovesThisRound;
		public int numOfTurns;
		public int numOfEnemyTurns;
		public StateManager manager;
		public ContinueUI contUi;

		[System.Serializable]
		public struct TurnInfo{
			public Powers ability;
			public Character player;
		}

		// Use this for initialization
		void Start() {
			manager = GetComponent<StateManager>();
			contUi = GetComponent<ContinueUI>();
		}

		public void Setup(List<Character> players, List<Character> enemies) {
			AvailablePlayers = players;
			AvailableEnemies = enemies;

			//find better solution won't work w/ mult button
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

				RemoveAttack(tmp);
				MovesThisRound.Add(tmp);

				//decal stuff
				manager.AddDecal(chara);
				contUi.SetInteractable();
			}
		}

		public void RemoveAttack(){
			numOfTurns++;
			MovesThisRound.RemoveAt(MovesThisRound.Count - 1);
			contUi.SetInteractable();
		}

		public void RemoveAttack(TurnInfo givenInfo) {
			//goes over full list and removes all abilities made by this player
			foreach(TurnInfo info in MovesThisRound) {
				if(info.player == givenInfo.player) {
					numOfTurns++;
					MovesThisRound.Remove(info);
					contUi.SetInteractable();
					break;
				}
			}
		}

		public void ResetTurnNumber(){
			numOfTurns = AvailablePlayers.Count;
			contUi.SetInteractable();
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

