using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace RPGsys {
	public class StateManager : MonoBehaviour {
		public bool confirmMoves = false;
		public bool redoTurn = false;
		WaitForSeconds endWait;
		List<Character> characters;
		List<Character> enemies;
		TurnBehaviour turnBehaviour;
		EnemyBehaviour[] enemyBehav;
		int rand;
		Quaternion originalRotation;

		GameObject GameOverUI;
		GameObject GameOverTextLose;
		GameObject GameOverTextWin;
		MoveConfirmMenu confirmMenu;

		public float speed;
		public Button MainMenu;
		public Button Quit;
		public GameObject selector;
		public float startDelay;
		public float endDelay;

        BattleManager battleManager;
        public ButtonBehaviourObjects buttonBehaviourObjects;
        public GameObject uiCanvas; //HACK

        [SerializeField] List<Transform> playerPositions;
        [SerializeField] List<Transform> enemyPositions;
        [SerializeField] Camera camera;

		// Use this for initialization
		void Start() {
			turnBehaviour = GetComponent<TurnBehaviour>();
			confirmMenu = GetComponent<MoveConfirmMenu>();
            battleManager = FindObjectOfType<BattleManager>();

            if(camera == null){
                camera = Camera.main;
            }

			endWait = new WaitForSeconds(endDelay);
			characters = new List<Character>();
			enemies = new List<Character>();

            // Activate own and enemy team from battleManager, and move enemy team into this scene
            battleManager.playerTeam.gameObject.SetActive(true);
            battleManager.enemyTeam.gameObject.SetActive(true);
            SceneManager.MoveGameObjectToScene(battleManager.enemyTeam.gameObject, gameObject.scene);

            enemyBehav = battleManager.enemyTeam.GetComponentsInChildren<EnemyBehaviour>(true);

			GameObject go = Instantiate(selector);
			selector = go;
			selector.SetActive(false);

            // move Selector into this scene
            SceneManager.MoveGameObjectToScene(selector, this.gameObject.scene);


			//game over menu stuff
			GameOverUI = GameObject.Find("GameOverMenu");
			MainMenu.onClick.AddListener(() => HandleClick(MainMenu));
			Quit.onClick.AddListener(() => HandleClick(Quit));

			GameOverTextLose = GameObject.Find("LoseText");
			GameOverTextWin = GameObject.Find("WinText");

			GameOverUI.SetActive(false);
			GameOverTextLose.SetActive(false);
			GameOverTextWin.SetActive(false);

			//grabbing players/enemies from the scene to fill lists
            characters.AddRange(battleManager.playerTeam.GetComponentsInChildren<Character>(true));


			//sort player list based on their choiceOrder number(so you can make it that the closest one to the screen picks first ect)
			List<Character> sortedList = characters.OrderBy(o => o.ChoiceOrder).ToList();
			characters = sortedList;
            //TODO place characters in scene positions based on this order (ie List<Transform> playerPositions and List<Transform> enemyPositions)

            enemies.AddRange(battleManager.enemyTeam.GetComponentsInChildren<Character>(true));

			foreach(Character chara in characters) {
				chara.GetComponent<ButtonBehaviour>().Setup(buttonBehaviourObjects);
                chara.GetComponent<TargetSelection>().Init(this.gameObject, camera);
            }

            // place player team in set positions
            for(int i = 0; i<playerPositions.Count; ++i)
            {
                characters[i].transform.position = playerPositions[i].position;
                characters[i].transform.rotation = playerPositions[i].rotation;
            }

			//shows enemy ui
			foreach(Character enemy in enemies) {
                enemy.GetComponent<EnemyUI>().enemyUISetup(uiCanvas);
            }
			foreach(Character enemy in enemies) {
				enemy.GetComponent<EnemyUI>().ShowUI();
			}

            for (int i = 0; i < enemyPositions.Count; ++i)
            {
                enemies[i].transform.position = enemyPositions[i].position;
                enemies[i].transform.rotation = enemyPositions[i].rotation;
            }


            turnBehaviour.Setup(characters, enemies);
			confirmMenu.Setup();

			//starting game loops
			StartCoroutine(GameLoop());
		}

		//while at least 1 player is alive, will loop the gamestates starting with the player
		private IEnumerator GameLoop() {


			yield return PlayerTurn();
			while(confirmMoves == false){
				yield return LockInMoves();
			}
			yield return EnemyTurn();
			yield return ApplyMoves();

			//checking if alive to keep looping
			if(!BattleOver()) {
				yield return GameLoop();
			}

			Debug.Log("peope are dead now");

			if(BattleOver() == true) {

                // Cleanup button behaviours
                List<ButtonBehaviour> buttonBehaviours = new List<ButtonBehaviour>();
                battleManager.playerTeam.GetComponentsInChildren<RPGsys.ButtonBehaviour>(buttonBehaviours);
                foreach (ButtonBehaviour bb in buttonBehaviours)
                {
                    bb.CleanUp();
                }

                Debug.Log("menu popup");
				GameOverUI.SetActive(true);
				if(Alive() == true) {
					GameOverTextWin.SetActive(true);
					Debug.Log("you won");
				} else if(EnemyAlive() == true) {
					Debug.Log("you lost");
					GameOverTextLose.SetActive(true);
				}
			}
		}

		//Pauses while each character can choose a target + power to use
		private IEnumerator PlayerTurn() {
			yield return new WaitForEndOfFrame();
			redoTurn = false;
			confirmMoves = false;
			List<Character> deadCharacters = new List<Character>();
			//if dead, remove from list
			foreach(Character chara in characters) {
				if(chara.Hp <= 0) {
					Debug.Log(chara.name + " is dead");
					chara.Hp = 0;
					deadCharacters.Add(chara);
				}
			}
			if(deadCharacters.Count > 0) {
				foreach(Character dead in deadCharacters) {
					characters.Remove(dead);
				}
			}
			
			int tmp = 0;
			foreach(Character chara in characters) {
				tmp += turnBehaviour.numOfTurns;
				chara.GetComponent<Animator>().SetBool("IdleTransition", true);
			}

			//loop through characters and wait until input to move to next one
			//foreach(Character chara in characters) {
			//	chara.GetComponent<ButtonBehaviour>().ShowButtons();


			//	foreach(Character chara2 in characters) {
			//		chara2.GetComponent<TargetSelection>().enabled = false;
			//	}
			//	chara.GetComponent<TargetSelection>().enabled = true;

			//	if(chara.target != null) {
			//		selector.transform.position = chara.target.transform.position;

			//	}
			//	while(chara.GetComponent<ButtonBehaviour>().playerActivated == false) {
			//		yield return null;
			//	}
			//	//sets character to animation to indicate that their move has passed
			//	chara.GetComponent<Animator>().SetBool("IdleTransition", false);
			//	chara.GetComponent<ButtonBehaviour>().HideButtons();
			//}

			for(int i = 0; i < characters.Count; i++) {
				characters[i].GetComponent<ButtonBehaviour>().ShowButtons();
				foreach(Character chara2 in characters) {
					chara2.GetComponent<TargetSelection>().enabled = false;
				}
				characters[i].GetComponent<TargetSelection>().enabled = true;

				if(characters[i].target != null) {
					selector.transform.position = characters[i].target.transform.position;

				}


				int currentPlayer = i;
				int previousPlayer = i - 1;
				while(characters[currentPlayer].GetComponent<ButtonBehaviour>().playerActivated == false) {
					//if undo button hit, sets current player to previous, sets undo to false
					if(characters[currentPlayer].GetComponent<ButtonBehaviour>().undoMove == true) {
						characters[currentPlayer].GetComponent<ButtonBehaviour>().undoMove = false;
						characters[currentPlayer].GetComponent<ButtonBehaviour>().playerActivated = true;
						Debug.Log("Undoing Action");
						currentPlayer = previousPlayer;
					}
					yield return null;
				}

				//if undo button hit, sets previous player to idle anim, hides buttons of current, removes the last set move and sets i to be 1 less than prev(does this as on next loop will auto i++)
				if(currentPlayer == previousPlayer) {
					characters[currentPlayer].GetComponent<Animator>().SetBool("IdleTransition", true);
					characters[i].GetComponent<ButtonBehaviour>().HideButtons();
					//turnBehaviour.MovesThisRound.RemoveAt(turnBehaviour.MovesThisRound.Count - 1);
					i = previousPlayer - 1;

				} else {
					characters[i].GetComponent<Animator>().SetBool("IdleTransition", false);
					characters[i].GetComponent<ButtonBehaviour>().HideButtons();
				}

			}
		}

		public IEnumerator LockInMoves() {
			confirmMenu.ShowMenu();
			while(confirmMoves != true) {
				yield return null;
			}
			confirmMenu.HideMenu();
			if(redoTurn == true) {
				Debug.Log("in the yield now going to player turn again");
				turnBehaviour.MovesThisRound.Clear();
				turnBehaviour.ResetTurnNumber();
				yield return PlayerTurn();
			}
			yield return true;
		}

		//clear menu away, rand select move
		public IEnumerator EnemyTurn() {
			yield return new WaitForEndOfFrame();

			selector.SetActive(false);
			foreach(Character chara in characters) {
				chara.GetComponent<TargetSelection>().enabled = false;
			}

			foreach(Character chara in characters) {
				chara.GetComponent<ButtonBehaviour>().HideButtons();
			}

			List<Character> deadEnemies = new List<Character>();
			foreach(Character enemy in enemies) {
				if(enemy.Hp <= 0) {
					Debug.Log(enemy.name + " is dead");
					enemy.Hp = 0;
					deadEnemies.Add(enemy);
					enemy.GetComponent<EnemyUI>().HideUI();

				}
			}
			if(deadEnemies.Count > 0) {
				foreach(Character dead in deadEnemies) {
					enemies.Remove(dead);
				}
			}

			for(int i = 0; i < enemies.Count; i++) {
				for(int j = 0; j < enemyBehav.Count(); j++) {
					if(enemies[i] == enemyBehav[j].GetChara) {
						rand = Random.Range(0, characters.Count);
						enemies[i].target = characters[rand].gameObject;
						enemyBehav[j].AddEnemyAttackRand(characters[rand]);
						break;
					}
				}
			}

			yield return new WaitForSeconds(0.5f);

		}

		//loop through moves on a delay, apply to targets
		public IEnumerator ApplyMoves() {

			//sort move list by speed
			List<TurnBehaviour.TurnInfo> sortedList = turnBehaviour.MovesThisRound.OrderByDescending(o => o.player.Speed).ToList();
			turnBehaviour.MovesThisRound = sortedList;

			foreach(TurnBehaviour.TurnInfo info in turnBehaviour.MovesThisRound) {
				originalRotation = info.player.transform.rotation;
				if(info.player.Hp > 0) {
					info.player.Timer();
					if(info.player.target == null) {
						rand = Random.Range(0, enemies.Count);
						info.player.target = enemies[rand].gameObject;
					}
					if(info.player.target.GetComponent<Character>().Hp <= 0) {
						//add check if target is dead, if so randomly selects enemy to fight
						for(int i = 0; i < enemies.Count; i++) {
							if(enemies[i].GetComponent<Character>().Hp > 0) {
								info.player.target = enemies[i].gameObject;
								break;
							} else {
								info.player.target = null;
							}
						}
					}

					if(info.player.target != null) {
						//turn player towards target
						info.player.transform.LookAt(info.player.target.transform);

						//does damage/animations
						info.ability.Apply(info.player, info.player.target.GetComponent<Character>());
						string name = info.ability.anim.ToString();
						info.player.GetComponent<Animator>().Play(name);
						info.player.target.GetComponent<Animator>().Play("TAKE_DAMAGE");
						//if player character, will allow them to go back to isle anim 
						if(info.player.tag != "Enemy") {
							info.player.GetComponent<Animator>().SetBool("IdleTransition", true);
						}
						//reset player rotation
						float step = speed * Time.deltaTime;

						//waits for attack anim to finish before spinning character back towards front
						yield return new WaitForSeconds(info.player.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length - info.player.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime);
						info.player.transform.rotation = Quaternion.Slerp(info.player.transform.rotation, originalRotation, speed);
					}
				}

				yield return new WaitForSeconds(info.player.GetComponent<Animator>().GetCurrentAnimatorStateInfo(1).length + 1.5f);
				if(info.player.target != null) {
					Death(info);
				}
			}

			turnBehaviour.MovesThisRound.Clear();
			turnBehaviour.numOfTurns = turnBehaviour.AvailablePlayers.Count;
			turnBehaviour.numOfEnemyTurns = turnBehaviour.AvailableEnemies.Count;

			yield return new WaitForSeconds(0.5f);
		}

		public void Death(TurnBehaviour.TurnInfo attackerInfo) {
			Character attackerTarget = attackerInfo.player.target.GetComponent<Character>();

			if(attackerTarget != null) {
				if(attackerTarget.Hp <= 0) {
					attackerTarget.Hp = 0;
					attackerTarget.GetComponent<Animator>().Play("DEAD");

					//remove buff effects on death
					foreach(Buff buff in attackerTarget.currentEffects) {
						buff.UpdateEffect(attackerTarget);
					}

					if(attackerTarget.gameObject.tag == "Enemy") {
						attackerTarget.GetComponent<EnemyUI>().HideUI();
						attackerTarget.GetComponent<Collider>().enabled = false;
					}
				}
			}
		}

		//if player is alive returns true, otherwise false
		public bool Alive() {
			foreach(Character chara in characters) {
				if(chara.Hp > 0) {
					return true;
				}
			}
			return false;
		}

		public bool EnemyAlive() {
			foreach(Character enemy in enemies) {
				if(enemy.Hp > 0) {
					return true;
				}
			}
			return false;
		}

		public bool BattleOver() {
			if((Alive() == true && EnemyAlive() == false) || (Alive() == false && EnemyAlive() == true)) {
				return true;
			}
			return false;
		}

		public void HandleClick(Button btn) {
			if(btn.GetComponentInChildren<Text>().text == "Main Menu") {
				SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
			}else if(btn.GetComponentInChildren<Text>().text == "Quit") {
				Application.Quit();
			}
		}
	}
}