using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using RPG.XP;

namespace RPGsys {
	public enum EBattleResult
	{
		Win,
		Loss,
		Flee
	}

	public class StateManager : MonoBehaviour {
		public bool confirmMoves = false;
		public bool redoTurn = false;
		public List<Character> characters;
		public float speed;
		public Button Quit;
		public Button Continue;
		public Button GameOverNext;
		public Text GameOverInfo;
		public GameObject selector;
		public GameObject uiCanvas; //HACK
		public CharacterButtonList characterButtonList;
		public bool PlayerTurnOver = false;

		//int rand;
		List<Character> enemies;
		List<Character> deadCharactersREVIVE;
		List<Character> storeTargets;
		TurnBehaviour turnBehaviour;
		DecalUI decalUI;
		EnemyBehaviour[] enemyBehav;
		Quaternion originalRotation;
		GameObject GameOverUI;
		GameObject GameOverTextLose;
		GameObject GameOverTextWin;

		MoveConfirmMenu confirmMenu;
		CameraMovement camMovement;
		BattleUIController battleUIController;

		[SerializeField] List<Transform> playerPositions;
        [SerializeField] List<Transform> enemyPositions;
        [SerializeField] new Camera camera;

		public EBattleResult result { get; private set; }

		public TurnBehaviour GetTurnBehaviour() { return turnBehaviour; }

		// Use this for initialization
		void Start() {

            //turn on feedback scripts
            FloatingTextController.DamageEnemy();
            FloatingTextController.DamageAlly();
            FloatingTextController.Miss();
            FloatingTextController.HealEnemy();
            FloatingTextController.HealAlly();

			RPG.BattleManager battleManager = RPG.BattleManager.Instance;
			battleManager.stateManager = this;
			turnBehaviour = GetComponent<TurnBehaviour>();
			decalUI = GetComponent<DecalUI>();
			confirmMenu = GetComponent<MoveConfirmMenu>();

			camMovement = GetComponent<CameraMovement>();
			storeTargets = new List<Character>();

			if(camera == null){
                camera = Camera.main;
            }

			characters = new List<Character>();
			enemies = new List<Character>();
			deadCharactersREVIVE = new List<Character>();

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
			GameOverNext.gameObject.SetActive(false);
			GameOverInfo.gameObject.SetActive(false);
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

			result = EBattleResult.Flee;

			//starting game loops
			StartCoroutine(GameLoop());
		}

		//while at least 1 player is alive, will loop the gamestates starting with the player
		private IEnumerator GameLoop() {

			// loop while players alive
			while(!BattleOver()){
				yield return PlayerTurn();
				//while(confirmMoves == false){
				//	yield return LockInMoves();
				//}
				yield return EnemyTurn();
				yield return ApplyMoves();
			}

			Debug.Log("peope are dead now");

			if(BattleOver() == true) {
				if(Alive() == true) {
					yield return WinRoutine();
				} else if(EnemyAlive() == true) {
					yield return LoseRoutine();
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

			//loop through characters and wait until input to move to next one
			//this allows the player to pick the turn order until there are no moves left
			while(PlayerTurnOver == false) {
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

						//remove last power for this character if you reselect them
						foreach(TurnBehaviour.TurnInfo info in turnBehaviour.MovesThisRound) {
							if(info.player == characters[i]) {
								turnBehaviour.RemoveAttack(info);
								break;
							}
						}

						//remove decals from this character if reselected
						foreach(DecalUI.DecalInfo info in decalUI.decalInfo) {
							if(info.player == characters[i]) {
								decalUI.RemoveDecal(info);
								break;
							}
						}

						//shows the current players buttons, will only move on currently if power selected or undo button pressed
						while(characters[i].ActivePlayer == true) {
							yield return null;
						}

						//sets them out of idle state, hides their power buttons
						MoveSetCheck(i);
						characterButtonList.uis[i].HidePowerButtons();
					}else {
						//put something here to stop it crashing :p
						characters[i].ActivePlayer = true;
					}
				}
				
			}
		}


		public void AddDecal(Character chara) {

			foreach(DecalUI.DecalInfo info in decalUI.decalInfo) {
				if(info.player == chara) {
					decalUI.RemoveDecal(info);
					break;
				}
			}
			//if no target selected, will select one for you
			foreach(TurnBehaviour.TurnInfo info in turnBehaviour.MovesThisRound) {
				RandomTargetSelect(info);
			}

			if(chara.target != null) {
				decalUI.InstantiateDecal(GetIndex(chara), chara, chara.target.GetComponent<Character>(), turnBehaviour);

			}
			MoveSetCheck(GetIndex(chara));
		}

		public int GetIndex(Character chara) {
			for(int i = 0; i < characters.Count; i++) {
				if(characters[i] == chara) {
					return i;
				}
			}
			return -1;
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
				decalUI.ClearAll();
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
			PlayerTurnOver = true;

			//sort move list by speed
			List<TurnBehaviour.TurnInfo> sortedList = turnBehaviour.MovesThisRound.OrderByDescending(o => o.player.Speed).ToList();
			turnBehaviour.MovesThisRound = sortedList;

			//destroying all the projector objects in the lists and then clearing lists
			decalUI.ClearAll();

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
					RandomTargetSelect(info);

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

			//GameOverUI.SetActive(true);
			//if(Alive() == true) {
			//	GameOverTextWin.SetActive(true);
			//} else if(EnemyAlive() == true) {
			//	GameOverTextLose.SetActive(true);
			//}
			yield return new WaitForSeconds(0.5f);
		}

		private IEnumerator WinRoutine()
		{
			GameOverUI.SetActive(true);
			result = EBattleResult.Win;

			// Do experience stuff
			int battleXP = 0;
			Dictionary<Character, XPEvent> xpEvents = new Dictionary<Character, XPEvent>();
			foreach (Character enemy in enemies)
			{
				XPSource xp = enemy.GetComponent<XPSource>();
				if (xp != null)
				{
					battleXP += xp.XP;
				}
			}

			foreach (Character player in characters)
			{
				// maybe check character is alive?
				if (player.experience != null)
				{
					XPEvent xpEvent = player.experience.AddXp(battleXP);
					xpEvents.Add(player, xpEvent);
				}
			}
			GameOverTextWin.SetActive(true);
			Quit.gameObject.SetActive(false);
			GameOverNext.gameObject.SetActive(true);
			GameOverInfo.gameObject.SetActive(true);

			// have Next button control when coroutine proceeds
			bool wait = true;
			UnityEngine.Events.UnityAction continueAction = () => wait = false;
			System.Func<bool> waitP = () => { return wait; };
			GameOverNext.onClick.AddListener(continueAction);

			GameOverInfo.text = string.Format("You gained {0} XP", battleXP);
			//TODO more juicy XP gain UI
			foreach(KeyValuePair<Character, XPEvent> eventPair in xpEvents)
			{
				foreach (LevelUpEvent levelUp in eventPair.Value.levelUps) {
					yield return new WaitWhile(waitP);
					wait = true;
					GameOverInfo.text = LevelUpInfo(eventPair.Key, levelUp);
				}
			}

			GameOverNext.onClick.RemoveListener(continueAction);
			GameOverNext.gameObject.SetActive(false);
			Quit.gameObject.SetActive(true);
			yield return new WaitForEndOfFrame(); //HACK
		}

		private IEnumerator LoseRoutine()
		{
			GameOverUI.SetActive(true);
			GameOverTextLose.SetActive(true);
			result = EBattleResult.Loss;
			yield return new WaitForEndOfFrame(); //HACK
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

		//if the player has no valid target, selects one for it
		public void RandomTargetSelect(TurnBehaviour.TurnInfo info) {
			if(info.player.target == null) {
				int rand = Random.Range(0, enemies.Count);
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
		}

		//checks if character has an attack queued up, if so changes it's anim to the attack state
		public void MoveSetCheck(int i) {
			bool MoveSet = false;
			foreach(TurnBehaviour.TurnInfo info in turnBehaviour.MovesThisRound) {
				if(info.player == characters[i]) {
					MoveSet = true;
					break;
				}
			}
			if(MoveSet == true) {
				characters[i].GetComponent<Animator>().SetBool("IdleTransition", false);
			} else {
				characters[i].GetComponent<Animator>().SetBool("IdleTransition", true);
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

		private string LevelUpInfo(Character character, LevelUpEvent levelUp)
		{
			if (levelUp)
			{
				System.Text.StringBuilder infoBuilder = new System.Text.StringBuilder();
				infoBuilder.AppendFormat("{0} reached level {1}!\n", character.name, levelUp.levelRank);
				// TODO show stat increases
				// TODO show powers learned
				return infoBuilder.ToString();
			} else
			{
				return "Error: LevelUp failed";
			}
		}
	}
}