using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameController : MonoBehaviour {

	public static GameController Instance = null;

	// HACK maybe have initial values etc somewhere else? Like in serialized object?
	[SerializeField] GameObject initialPlayerTeam;
	public GameObject playerTeam;

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
	}
}
