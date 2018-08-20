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
		}

		if (Input.GetKeyDown(KeyCode.Tab))
		{
			//HACK
			if (inventoryScreen.gameObject.activeInHierarchy)
			{
				inventoryScreen.Close();
			} else
			{
				inventoryScreen.Open();
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

	}
}
