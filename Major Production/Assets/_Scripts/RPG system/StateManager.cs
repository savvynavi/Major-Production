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
		public List<Character> characters;
		public float speed;
		public Button MainMenu;
		public Button Quit;
		public GameObject selector;
		public GameObject uiCanvas; //HACK
		public CharacterButtonList characterButtonList;
		public List<GameObject> projector;
		public GameObject arrowProjector;

		int rand;
		List<Character> enemies;
		List<Character> deadCharactersREVIVE;
		List<Character> storeTargets;
		TurnBehaviour turnBehaviour;
		EnemyBehaviour[] enemyBehav;
		Quaternion originalRotation;
		GameObject GameOverUI;
		GameObject GameOverTextLose;
		GameObject GameOverTextWin;
		MoveConfirmMenu confirmMenu;
		CameraMovement camMovement;
		BattleUIController battleUIController;
		List<GameObject> projectors;
		List<GameObject> arrowProjectors;

		[SerializeField] List<Transform> playerPositions;
        [SerializeField] List<Transform> enemyPositions;
        [SerializeField] new Camera camera;

		public TurnBehaviour GetTurnBehaviour() { return turnBehaviour; }

		// Use this for initialization
		void Start() {

            //turn on feedback scripts
            FloatingTextController.DamageEnemy();
            FloatingTextController.DamageAlly();
            FloatingTextController.Miss();
            FloatingTextController.HealEnemy();
            FloatingTextController.HealAlly();

			BattleManager battleManager = BattleManager.Instance;
			battleManager.stateManager = this;
			turnBehaviour = GetComponent<TurnBehaviour>();
			confirmMenu = GetComponent<MoveConfirmMenu>();

			camMovement = GetComponent<CameraMovement>();
			storeTargets = new List<Character>();

			if(camera == null){
                camera = Camera.main;
            }

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
			battleUIController = GetComponent<BattleUIController>();
			battleUIController.UISetup(characters);
			
			//setting all characters to be inactive
			foreach(Character chara in characters) {
				chara.ActivePlayer = false;
			}

			//TODO place characters in scene positions based on this order (ie List<Transform> playerPositions and List<Transform> enemyPositions)

			enemies.AddRange(battleManager.enemyTeam.GetComponentsInChildren<Character>(true));

			//////PLACES CHARACTERS TO PRE-SET LOCATIONS
			foreach(Character chara in characters) {
				//chara.GetComponent<ButtonBehaviour>().Setup(buttonBehaviourObjects);
                chara.GetComponent<TargetSelection>().Init(this.gameObject, camera);
            }

            // place player team in set positions
            for(int i = 0; i<playerPositions.Count && i < characters.Count; ++i)
            {
                characters[i].transform.position = playerPositions[i].position;
                characters[i].transform.rotation = playerPositions[i].rotation;
            }
			//////

			List<Character> tmp = new List<Character>();
			//when battle reentered, forces any dead characters to act like it
			foreach(Character chara in characters) {
				Debug.Log(chara.name + "'s HP: " + chara.Hp);
				if(chara.Hp <= 0) {
					tmp.Add(chara);
					Death(chara, tmp);
				}
			}

			//moving enemies into position
			for(int i = 0; i < enemyPositions.Count && i < enemies.Count; ++i) {
				enemies[i].transform.position = enemyPositions[i].position;
				enemies[i].transform.rotation = enemyPositions[i].rotation;
			}

			//shows enemy ui
			foreach(Character enemy in enemies) {
                enemy.GetComponent<EnemyUI>().enemyUISetup(uiCanvas);
            }
			foreach(Character enemy in enemies) {
				enemy.GetComponent<EnemyUI>().ShowUI();
			}

			turnBehaviour.Setup(characters, enemies);
			//confirmMenu.Setup();
			projectors = new List<GameObject>();
			arrowProjectors = new List<GameObject>();

			//starting game loops
			StartCoroutine(GameLoop());
		}

		//while at least 1 player is alive, will loop the gamestates starting with the player
		private IEnumerator GameLoop() {

			// loop while players alive
			while(!BattleOver()){
				yield return PlayerTurn();
				while(confirmMoves == false){
					yield return LockInMoves();
				}
				yield return EnemyTurn();
				yield return ApplyMoves();
			}

			Debug.Log("peope are dead now");

			if(BattleOver() == true) {
				GameOverUI.SetActive(true);
				if(Alive() == true) {
					// Do experience stuff
					int battleXP = 0;
					foreach(Character enemy in enemies)
					{
						RPG.XP.XPSource xp = enemy.GetComponent<RPG.XP.XPSource>();
						if(xp != null)
						{
							battleXP += xp.XP;
						}
					}

					foreach(Character player in characters)
					{
						// maybe check character is alive?
						if(player.experience != null)
						{
							player.experience.AddXp(battleXP);
						}
					}

					GameOverTextWin.SetActive(true);
				} else if(EnemyAlive() == true) {
					// Do Game Over stuff

					GameOverTextLose.SetActive(true);
				}
			}
		}

		//Pauses while each character can choose a target + power to use
		private IEnumerator PlayerTurn() {
			yield return new WaitForEndOfFrame();
			redoTurn = false;
			confirmMoves = false;
			//REMOVE ANY INSTANCE OF THIS GARBAGE, REDO DEATH CHECK WITH A BOOL OR SOMETHING
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
			Debug.Log("Crashing before the loop starts");

			////TODO set it up so that right clicking characters sets them as active/deactivates current active character (will let player chooose turn order)
			////ALSO make it so it doesn't auto-move onto the next round when all moves are set(replace lockin moves menu with a thing that waits until a done button is pressed(if no move selected do random one as test))
			//loop through characters and wait until input to move to next one
			//for(int i = 0; i < characters.Count; i++) {

			//this allows the player to pick the turn order, fix it though as it Crashes at the slighest thing (mostly with decals?)
			while(turnBehaviour.numOfTurns > 0) {
				Debug.Log("Crashing after the loop starts");

				for(int i = 0; i < characters.Count; i++) {
					if(characters[i].ActivePlayer == true) {
						characterButtonList.uis[i].ShowPowerButtons();
						battleUIController.MenuHp.UpdateInfo(characters[i]);
						foreach(Character chara2 in characters) {
							chara2.GetComponent<TargetSelection>().enabled = false;
						}
						characters[i].GetComponent<TargetSelection>().enabled = true;

						//selector only visible if the target isn't null
						if(characters[i].target != null) {
							selector.gameObject.SetActive(true);
							selector.transform.position = characters[i].target.transform.position;
						} else {
							selector.gameObject.SetActive(false);
						}



						int currentPlayer = i;
						int previousPlayer = i - 1;

						//shows the current players buttons, will only move on currently if power selected or undo button pressed
						while(characters[i].ActivePlayer == true) {
							if(characterButtonList.uis[i].UndoMove == true) {
								characterButtonList.uis[i].UndoMove = false;
								//characters[i].ActivePlayer = true;
								currentPlayer = previousPlayer;
								////decal stuff, deletes last one set if move undone
								GameObject lastObj = projectors.Last();
								projectors.Remove(lastObj);
								Destroy(lastObj);

								GameObject lastArrowObj = arrowProjectors.Last();
								arrowProjectors.Remove(lastArrowObj);
								Destroy(lastArrowObj);
							}
							yield return null;
						}


						//if undo button hit, sets previous player to idle anim, hides buttons of current, removes the last set move and sets i to be 1 less than prev(does this as on next loop will auto i++)
						//if(currentPlayer == previousPlayer) {
						//	characters[currentPlayer].GetComponent<Animator>().SetBool("IdleTransition", true);
						//	characterButtonList.uis[i].HidePowerButtons();

						//	i = previousPlayer - 1;

						//} else {

							//decal stuff
							if(characters[i].target != null) {
								int count = 0;
								for(int j = 0; j < turnBehaviour.MovesThisRound.Count; j++) {
									if(turnBehaviour.MovesThisRound[j].player.target == characters[i].target) {
										count++;
									}
								}
								//disc spawning
								GameObject tmpObj = Instantiate(projector[count - 1]);
								tmpObj.transform.position = new Vector3(characters[i].target.transform.position.x, tmpObj.transform.position.y, characters[i].target.transform.position.z);
								projectors.Add(tmpObj);

								//wont spawn an arrow if it's a self/team targeting move, as the arrows look super janked if they do
								if(characters[i].target.tag != "Player") {
									//arrow instantiating, spawns an arrow then rotates it towards the enemy from the character
									//TODO add in arrow delete parts, figure out how to colour the rings
									GameObject tmpArrow = Instantiate(arrowProjector);
									Vector3 midPoint = (characters[i].transform.position + (characters[i].target.transform.position - characters[i].transform.position) / 2);
									tmpArrow.transform.position = new Vector3(midPoint.x, tmpObj.transform.position.y, midPoint.z);

									//scales the arrow so it fits between the characters
									float distance = Vector3.Distance(characters[i].transform.position, characters[i].target.transform.position);
									tmpArrow.GetComponent<Projector>().orthographicSize = distance / 2;
									tmpArrow.GetComponent<Projector>().aspectRatio = 1 / Mathf.Pow(tmpArrow.GetComponent<Projector>().orthographicSize, 2);

									tmpArrow.transform.LookAt(characters[i].target.transform);
									tmpArrow.transform.eulerAngles = new Vector3(90, tmpArrow.transform.eulerAngles.y, tmpArrow.transform.eulerAngles.z);

									arrowProjectors.Add(tmpArrow);
								}

							}

							//sets them out of idle state, hides their power buttons
							characters[i].GetComponent<Animator>().SetBool("IdleTransition", false);
							characterButtonList.uis[i].HidePowerButtons();
						//}
					} else {
						//put something here to stop it crashing :p
						characters[i].ActivePlayer = true;
					}
				}


				
			}
		}

		public IEnumerator LockInMoves() {
			battleUIController.moveConfirmMenu.ShowMenu();
			while(confirmMoves != true) {
				yield return null;
			}
			battleUIController.moveConfirmMenu.HideMenu();
			if(redoTurn == true) {
				turnBehaviour.MovesThisRound.Clear();
				turnBehaviour.ResetTurnNumber();

				//clearing projections
				for(int i = projectors.Count - 1; i >= 0; i--) {
					Destroy(projectors[i]);
				}
				projectors.Clear();

				for(int i = arrowProjectors.Count - 1; i >= 0; i--) {
					Destroy(arrowProjectors[i]);
				}
				arrowProjectors.Clear();

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

			for(int i = 0; i < characterButtonList.uis.Count; i++) {
				characterButtonList.uis[i].HidePowerButtons();
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
                        enemyBehav[j].AddAttack(characters, enemies);
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

			//destroying all the projector objects in the lists and then clearing lists
			for(int i = projectors.Count - 1; i >= 0; i--) {
				Destroy(projectors[i]);
			}
			projectors.Clear();

			for(int i = arrowProjectors.Count - 1; i >= 0; i--) {
				Destroy(arrowProjectors[i]);
			}
			arrowProjectors.Clear();

			//each loop is a players turn
			foreach(TurnBehaviour.TurnInfo info in turnBehaviour.MovesThisRound) {
				originalRotation = info.player.transform.rotation;
				

				info.player.Timer();
				//died due to effect SET UP BETTER DEATH CHECK SYSTEM THIS IS GETTING SILLY
				if(info.player.Hp <= 0) {
					List<Character> tmp = new List<Character>();
					tmp.Add(info.player);
					Death(info.player, tmp);
				}

				if(info.player.Hp > 0) {
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

						//check to see if self targeting powers are targeting self, if not set it to be so
						if(info.ability.areaOfEffect == Powers.AreaOfEffect.Self) {
							info.player.target = info.player.gameObject;
						}

						info.player.transform.LookAt(info.player.target.transform);
						camMovement.LookAtAttacker(info.player);
						yield return new WaitForSeconds(0.5f);


						//does damage/animations
						if(info.ability.areaOfEffect == Powers.AreaOfEffect.Group) {

							if(info.player.target.tag == "Player") {
								AddToStoredList(characters);
							} else {
								AddToStoredList(enemies);
							}
							camMovement.LookAtGroup(storeTargets);

							if(info.player.target.tag == "Player") {
								GroupAttack(info, characters, storeTargets);
							} else {
								GroupAttack(info, enemies, storeTargets);
							}


						} else if(info.ability.areaOfEffect == Powers.AreaOfEffect.Single || info.ability.areaOfEffect == Powers.AreaOfEffect.Self) {

							//if same team, use facecam, else single out enemy target
							if(info.player.tag == info.player.target.tag) {
								camMovement.LookAtAttacker(info.player.target.GetComponent<Character>());
							} else {
								camMovement.LookAtTarget(info.player, info.player.target.GetComponent<Character>());
							}


							info.ability.Apply(info.player, info.player.target.GetComponent<Character>());
							string name = info.ability.anim.ToString();
							info.player.GetComponent<Animator>().Play(name);



							info.player.target.GetComponent<Animator>().Play("TAKE_DAMAGE");
							//if player character, will allow them to go back to isle anim 
							if(info.player.tag != "Enemy") {
								info.player.GetComponent<Animator>().SetBool("IdleTransition", true);
							}
							storeTargets.Add(info.player.target.GetComponent<Character>());
						} else {
							storeTargets = null;
						}

						//foreach(Character chara in characters) {
						//	if(chara != info.player && chara != info.player.target.GetComponent) {
						//		chara.gameObject.SetActive(false);
						//	}
						//}

						//foreach(Character enem in enemies) {
						//	if(enem != info.player && enem != info.player.target) {
						//		enem.gameObject.SetActive(false);
						//	}
						//}

						yield return new WaitForSeconds(1.5f);



						//reset player rotation
						float step = speed * Time.deltaTime;

						//waits for attack anim to finish before spinning character back towards front
						yield return new WaitForSeconds(info.player.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length - info.player.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime);
						info.player.transform.rotation = Quaternion.Slerp(info.player.transform.rotation, originalRotation, speed);
						camMovement.Reset();

						//foreach(Character chara in characters) {
						//	if(chara != info.player || chara != info.player.target) {
						//		chara.gameObject.SetActive(true);
						//	}
						//}

						//foreach(Character enem in enemies) {
						//	if(enem != info.player || enem != info.player.target) {
						//		enem.gameObject.SetActive(true);
						//	}
						//}
					}
				}

				yield return new WaitForSeconds(info.player.GetComponent<Animator>().GetCurrentAnimatorStateInfo(1).length + 1.5f);
				
				if(storeTargets != null) {
					Death(info.player.target.GetComponent<Character>(), storeTargets);
				}
				//if either side dead already, end fight
				if(BattleOver() == true) {
					yield return EndBattle();
					break;
				}
			}

			turnBehaviour.MovesThisRound.Clear();
			turnBehaviour.numOfTurns = turnBehaviour.AvailablePlayers.Count;
			turnBehaviour.numOfEnemyTurns = turnBehaviour.AvailableEnemies.Count;

			yield return new WaitForSeconds(0.5f);
		}

		public IEnumerator EndBattle() {
			turnBehaviour.MovesThisRound.Clear();

			foreach(Character chara in characters) {
				chara.GetComponent<Animator>().SetBool("IdleTransition", true);
			}

			GameOverUI.SetActive(true);
			if(Alive() == true) {
				GameOverTextWin.SetActive(true);
			} else if(EnemyAlive() == true) {
				GameOverTextLose.SetActive(true);
			}
			yield return new WaitForSeconds(0.5f);
		}

		public void Death(Character attackerTarget, List<Character> targets) {
			//if no list given, do 1 target, else loop over all targets
			if(targets != null) {
				foreach(Character target in targets) {
					if(target.Hp <= 0) {
						target.Hp = 0;
						target.GetComponent<Animator>().Play("DEAD");

						foreach(Buff buff in target.currentEffects) {
							buff.UpdateEffect(target);
						}

						if(target.gameObject.tag == "Enemy") {
							target.GetComponent<EnemyUI>().HideUI();
							target.GetComponent<Collider>().enabled = false;

						}
						//if it's a player it is added to the deadCharacter list, will be used for revives in battle
						if(target.gameObject.tag == "Player") {
							deadCharactersREVIVE.Add(target);
						}
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

		public void AddToStoredList(List<Character> targets) {
			foreach(Character chara in targets) {
				storeTargets.Add(chara);
			}
		}

		public void GroupAttack(TurnBehaviour.TurnInfo info, List<Character> targets, List<Character> storeTargets) {
			foreach(Character chara in targets) {
				info.ability.Apply(info.player, chara);
				string name = info.ability.anim.ToString();
				info.player.GetComponent<Animator>().Play(name);
				chara.GetComponent<Animator>().Play("TAKE_DAMAGE");
				//storeTargets.Add(chara);
			}

			//if player character, will allow them to go back to isle anim 
			if(info.player.tag != "Enemy") {
				info.player.GetComponent<Animator>().SetBool("IdleTransition", true);
			}
		}

		public bool BattleOver() {
			if((Alive() == true && EnemyAlive() == false) || (Alive() == false && EnemyAlive() == true)) {
				return true;
			}
			return false;
		}
	}
}