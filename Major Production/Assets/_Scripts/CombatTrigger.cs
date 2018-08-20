using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//HACK currently just for testing scene changes

public class CombatTrigger : MonoBehaviour {
	
    [SerializeField] string battleScene;
    [SerializeField] GameObject enemies;
    bool triggered = false;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        //HACK reset
        if (triggered)
        {
            StartCoroutine(resetTrigger());
        }
	}

    private void OnTriggerEnter(Collider other)
    {
        if (!triggered && other.CompareTag("Player"))
        {
            GameObject go = GameObject.Instantiate(enemies.gameObject);
            go.SetActive(false);
            BattleManager.Instance.StartBattle(battleScene, go.transform);
            triggered = true;
        }
    }

    //HACK
    IEnumerator resetTrigger()
    {
        yield return new WaitForSeconds(1);
        triggered = false;
    }
}
