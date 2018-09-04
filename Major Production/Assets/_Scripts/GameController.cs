using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using RPG.UI;
using RPG.Save;
using Newtonsoft.Json.Linq;

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

	// TODO something that calls Save, and serialized the JObject to a file

	public JObject Save()
	{
		// TODO save characters, inventory, sceneloader
		throw new System.NotImplementedException();
	}

	public void Load(JObject data)
	{
		// TODO load characters, inventory, sceneloader

		throw new System.NotImplementedException();
		state = EGameStates.Overworld;
	}
}
