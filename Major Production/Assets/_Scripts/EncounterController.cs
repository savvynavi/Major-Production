using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EncounterController : MonoBehaviour {
	public float Probability;
	float mod;
	float nextTimeEncounter;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	public void RandomEncounter () {

		//currently doesn't work
		float rand = Random.Range(0, 100);
		nextTimeEncounter = Time.time + rand;
		Debug.Log("Next Time: " + nextTimeEncounter);
		Debug.Log("Time: " + Time.time);
		if(Time.time > nextTimeEncounter) {
			Debug.Log("Random Encounter!");
			Debug.Log("Random: " + rand);

		}
		//if(CurveRandomNumber(rand, curve) >= Probability) {
		//	Debug.Log("Random Encounter");
		//	Debug.Log(rand);
		//}

	}

	public float CurveRandomNumber(float rand, AnimationCurve curve){
		return curve.Evaluate(rand);
	}
}
