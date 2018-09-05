using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using Newtonsoft.Json.Linq;
using RPGsys;
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

	// HACK maybe have initial values etc somewhere else? Like in serialized object?
	[SerializeField] GameObject initialPlayerTeam;
	[SerializeField] List<RPGItems.Item> initialInventory;
	public GameObject playerTeam;
	public RPGItems.InventoryManager inventory;
	[SerializeField] MenuManager menus;

	public RPGsys.Character[] Characters { get { return playerTeam.GetComponentsInChildren<RPGsys.Character>(true); } }

	private void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
		} else if (Instance != this)
		{
			GameObject.Destroy(gameObject);
		}
		DontDestroyOnLoad(gameObject);
		inventory = GetComponent<RPGItems.InventoryManager>();
		menus = GetComponentInChildren<MenuManager>(true);
		state = EGameStates.Menu;	//HACK assumes game starts at main menu!
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
		playerTeam = GameObject.Instantiate(initialPlayerTeam, this.transform);
		playerTeam.SetActive(false);
		BattleManager.Instance.playerTeam = playerTeam.transform;

		SceneLoader.Instance.Init();
		inventory.Initialize(initialInventory);

		state = EGameStates.Overworld;
	}

	public void Pause()
	{
		Time.timeScale = 0;
	}

	public void Unpause()
	{
		Time.timeScale = 1;
	}

	public void QuitToTitle()
	{
		if(state == EGameStates.Battle)
		{
			playerTeam.SetActive(false);
		}
		SceneManager.LoadScene("Main Menu");
		menus.CloseMenus();
		state = EGameStates.Menu;
	}

	// TODO saving to specific file
	public void SaveGame()
	{
		// TODO use a coroutine or other asynchronous operation to do this
		File.WriteAllText(Application.persistentDataPath +  "/savegame.json", Save().ToString());
	}

	public JObject Save()
	{
		// TODO save characters, inventory, sceneloader
		// TODO error handling (just put in try/catch block and dump error somewhere?)
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

	public void Load(JObject data)
	{
		// TODO load characters, inventory, sceneloader

		throw new System.NotImplementedException();
		state = EGameStates.Overworld;
	}
}
