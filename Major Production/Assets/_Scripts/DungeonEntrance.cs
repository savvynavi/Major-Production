using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonEntrance : MonoBehaviour {

    SceneLoader loader;
    [SerializeField] string dungeonScene;
    bool triggered = false;

	// Use this for initialization
	void Start () {
        loader = FindObjectOfType<SceneLoader>();
	}

    // Update is called once per frame
    void Update()
    {
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
            loader.LoadScene(dungeonScene);
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
