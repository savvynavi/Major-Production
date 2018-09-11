using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
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
	public bool Paused { get; private set; }

	// HACK maybe have initial values etc somewhere else? Like in serialized object?
	[SerializeField] GameObject initialPlayerTeam;
	[SerializeField] List<RPGItems.Item> initialInventory;
	public GameObject playerTeam;
	public RPGItems.InventoryManager inventory;
	[SerializeField] MenuManager menus;
	public CharacterPrefabList prefabList; // Might be needed to force its loading? should test in build with and without

	[Header("Save System")]
	[SerializeField]
	SaveManager saveManager;

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
		Paused = true;
	}

	public void Unpause()
	{
		Time.timeScale = 1;
		Paused = false;
	}

	public void QuitToTitle()
	{
		if(state == EGameStates.Battle && playerTeam != null)
		{
			playerTeam.SetActive(false);
		}
		SceneManager.LoadScene("Main Menu");
		menus.CloseMenus();
		state = EGameStates.Menu;
	}

	public void SaveGame()
	{
		saveManager.SaveToFile(Application.persistentDataPath + "/savegame.json");
	}

	public void LoadGame()
	{
		saveManager.LoadFromFile(Application.persistentDataPath + "/savegame.json");
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
		GameObject loadedTeam = null;

		menus.CloseMenus();

		try
		{
			//HACK should probably check that JTokens are correct type before attemting to cast
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
			state = EGameStates.Overworld;
			SceneManager.LoadScene("Main Menu");
			Debug.LogWarning("Exception on loading file: " + e.Message);
		}
	}

	// might not use this
	public static bool DataValid(JObject data)
	{
		JToken team;
		JToken inventorySave;
		JToken sceneSave;
		if(data.TryGetValue("playerTeam", out team) && team is JArray && team.HasValues)
		{
			foreach(JToken child in (JArray)team)
			{
				if(child is JObject && Character.DataValid((JObject) child))
				{
					continue;
				} else
				{
					return false;
				}
			}
		} else
		{
			return false;
		}
		if(data.TryGetValue("inventory", out inventorySave) && inventorySave is JObject)
		{
			if (!RPGItems.InventoryManager.DataValid((JObject)inventorySave))
			{
				return false;
			}
		}
		if(data.TryGetValue("scene", out sceneSave) && sceneSave is JObject)
		{
			if(!SceneLoader.DataValid((JObject)sceneSave))
			{
				return false;
			}
		}
		return true;
	}
}
