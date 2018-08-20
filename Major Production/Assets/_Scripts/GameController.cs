using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameController : MonoBehaviour {

	public enum EGameStates{
		Menu,
		Overworld,
		Battle
	}

	public static GameController Instance = null;

	public EGameStates state;

	// HACK maybe have initial values etc somewhere else? Like in serialized object?
	[SerializeField] GameObject initialPlayerTeam;
	[SerializeField] List<RPGItems.Item> initialInventory;
	public GameObject playerTeam;
	public RPGItems.InventoryManager inventory;
	[SerializeField] InventoryScreen inventoryScreen;	// HACK might go elsewhere?

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
		inventoryScreen = GetComponentInChildren<InventoryScreen>(true);
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
			SceneManager.LoadScene("Main Menu");
			state = EGameStates.Menu;
		}

		if (Input.GetKeyDown(KeyCode.Tab))
		{
			//HACK
			if (inventoryScreen.gameObject.activeInHierarchy)
			{
				inventoryScreen.Close();
				Time.timeScale = 1;
				// HACK do pause/unpause elsewhere (on opening any menu?)

			} else
			{
				inventoryScreen.Open();
				Time.timeScale = 0;
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
}
