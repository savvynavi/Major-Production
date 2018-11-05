using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using RPGsys;
using RPG;
using RPG.UI;
using RPG.Save;


// TODO have some OnStateChange function/event (ie to allow menu to close itself)
public class GameController : MonoBehaviour, ISaveable {

	public enum EGameStates{
		Menu,
		Overworld,
		Battle
	}

	public static GameController Instance = null;

	public EGameStates state;		// HACK make private, use a field and ChangeState function for access
	public bool Paused { get; private set; }

	// HACK maybe have initial values etc somewhere else? Like in serialized object?
	[SerializeField] List<Character> initialPlayerTeam;
	[SerializeField] List<RPGItems.Item> initialInventory;
	public GameObject playerTeam;
	public RPGItems.InventoryManager inventory;
	[SerializeField] MenuManager menus;
	public LoadScreen loadScreen;
	public CharacterPrefabList prefabList; // Might be needed to force its loading? should test in build with and without

	[Header("Save System")]
	[SerializeField]
	SaveManager saveManager;
	JObject autosaveData;
	[SerializeField]
	TextAsset newGameSaveData;


	public RPGsys.Character[] Characters { get { return playerTeam.GetComponentsInChildren<RPGsys.Character>(true); } }
	

	private void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
		} else if (Instance != this)
		{
			GameObject.Destroy(gameObject);
			return;
		}
		DontDestroyOnLoad(gameObject);
		inventory = GetComponent<RPGItems.InventoryManager>();
		menus = GetComponentInChildren<MenuManager>(true);
		Paused = false;
		state = EGameStates.Menu;   //HACK assumes game starts at main menu!

		saveManager.Init();
	}
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			//HACK for now, until we put in a proper pause menu
			QuitToTitle();
		}

		if (Input.GetKeyDown(KeyCode.Tab) && state == EGameStates.Overworld)
		{
			if (menus.Open)
			{
				menus.CloseMenus();
			} else
			{
				menus.OpenMenus();
			}
		}
	}

	public void InitializeGame()
	{
		if(playerTeam != null)
		{
			GameObject.Destroy(playerTeam);
		}
		playerTeam = new GameObject("Player Team");
		playerTeam.transform.parent = this.transform;
		foreach(Character prefab in initialPlayerTeam)
		{
			//GameObject.Instantiate(prefab, playerTeam.transform);
			Utility.InstantiateSameName<Character>(prefab, playerTeam.transform);
		}
		playerTeam.SetActive(false);
		BattleManager.Instance.playerTeam = playerTeam.transform;

		SceneLoader.Instance.Init();
		inventory.Initialize(initialInventory);

		state = EGameStates.Overworld;
	}

	public void StartNewGame()
	{
		// TODO additional stuff around this
		StartCoroutine(saveManager.LoadFromText(newGameSaveData));
	}

	public void Pause()
	{
		Time.timeScale = 0;
		Paused = true;
	}

	public void Unpause()
	{
		Time.timeScale = 1;
		Paused = false;
	}

	public void QuitToTitle()
	{
		if(playerTeam != null)
		{
			playerTeam.SetActive(false);
		}
		SceneManager.LoadScene("Main Menu");
		menus.CloseMenuImmediate();
		state = EGameStates.Menu;
	}

	public void SaveGame()
	{
		StartCoroutine(saveManager.SaveToFile(Application.persistentDataPath + "/savegame.json"));
	}

	public void LoadGame()
	{
		StartCoroutine(saveManager.LoadFromFile(Application.persistentDataPath + "/savegame.json"));
	}

	// Saves to autosaveData. Note this does not save to a file
	public void Autosave()
	{
		autosaveData = Save();
	}

	// Loads from autosaveData
	public void Autoload()
	{
		Load(autosaveData);
	}

	// This is the top level save for the game, building a JObject containing the save 
	// objects for everything in the game that needs saving
	public JObject Save()
	{
		List<Character> team = new List<Character>(playerTeam.GetComponentsInChildren<Character>(true));
		JArray CharacterSave = new JArray(from c in team
										  select c.Save());

		JObject InventorySave = inventory.Save();
		JObject SceneSave = SceneLoader.Instance.Save();

		return new JObject(
			new JProperty("playerTeam", CharacterSave),
			new JProperty("inventory", InventorySave),
			new JProperty("scene", SceneSave));
	}

	// This loads the JObject into everything needed for the game
	// It doesn't check validity, this is done by the SaveManager
	// in the LoadOperation worker object
	public void Load(JObject data)
	{
		GameObject loadedTeam = null;

		menus.CloseMenuImmediate();

		try
		{
			loadedTeam = CharacterFactory.CreatePlayerTeam((JArray)data["playerTeam"]);
			loadedTeam.transform.SetParent(this.transform);
			loadedTeam.SetActive(false);
			loadedTeam.name = "Player Team";
			if (playerTeam != null)
			{
				GameObject.Destroy(playerTeam);
			}
			playerTeam = loadedTeam;

			BattleManager.Instance.playerTeam = playerTeam.transform;

			inventory.Load((JObject)data["inventory"]);
			SceneLoader.Instance.Load((JObject)data["scene"]);

			state = EGameStates.Overworld;
			
		}catch(System.Exception e)
		{
			// If something goes wrong, reset everything and go back to main menu
			if(loadedTeam != null)
			{
				GameObject.Destroy(loadedTeam);
			}
			if (playerTeam != null)
			{
				GameObject.Destroy(playerTeam);
				playerTeam = null;
			}
			BattleManager.Instance.playerTeam = null;
			inventory.Clear();
			state = EGameStates.Menu;
			SceneManager.LoadScene("Main Menu");
			// HACK would be better to show an error to the player
			Debug.LogWarning("Exception on loading file: " + e.Message);
		}
	}
}
