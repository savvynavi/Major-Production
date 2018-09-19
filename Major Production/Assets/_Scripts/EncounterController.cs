using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SceneController))]
public class EncounterController : MonoBehaviour {
	public float Probability;
	public float minEncounterTime;
	public float maxEncounterTime;
	float mod;
	[SerializeField]float nextTimeEncounter;	// so we can see it tick down

	[SerializeField] RPG.Encounter encounter;
	[SerializeField] string battleScene;
	public SceneController controller { get; private set; }
	public bool ticking;

	// Use this for initialization
	void Start () {
		SetRandomTime();
		controller = GetComponent<SceneController>();
		ticking = true;
	}

	private void Update()
	{
		// TODO only tick down while player moving?
		if (ticking)
		{
			nextTimeEncounter -= Time.deltaTime;
			if (nextTimeEncounter < 0.0f)
			{
				RandomEncounter();
				SetRandomTime();
			}
		}
	}

	// Update is called once per frame
	public void RandomEncounter () {

		//currently doesn't work
		float rand = Random.value;
		if(rand <= Probability)
		{
			StartEncounter();
		}

	}

	public float CurveRandomNumber(float rand, AnimationCurve curve){
		return curve.Evaluate(rand);
	}

	 void SetRandomTime()
	{
		nextTimeEncounter = Random.Range(Mathf.Min(minEncounterTime, maxEncounterTime), Mathf.Max(minEncounterTime, maxEncounterTime));
	}

	protected void StartEncounter()
	{
		GameObject go = encounter.InstantiateEnemyTeam();
		BattleManager.Instance.StartBattle(battleScene, go.transform);
	}
}
