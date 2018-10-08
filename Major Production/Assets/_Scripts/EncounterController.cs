using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace RPG {
	[System.Serializable]
	public struct WeightedEncounter
	{
		public Encounter Option;
		public float Weight;
	}

	[RequireComponent(typeof(SceneController))]
	public class EncounterController : MonoBehaviour {
		[Tooltip("Chance of random encounter")] public float Probability;
		public float minEncounterTime;
		public float maxEncounterTime;
		float mod;
		[SerializeField] float nextTimeEncounter;   // so we can see it tick down
		
		[SerializeField] List<WeightedEncounter> encounterList;
		[Tooltip("Modifier for last encounter")] public float repeatModifier = 1;
		Encounter lastEncounter = null;
		[SerializeField] string battleScene;
		public SceneController controller { get; private set; }
		public bool ticking;

		// Use this for initialization
		void Start() {
			SetRandomTime();
			controller = GetComponent<SceneController>();
			ticking = true;
		}

		private void Update()
		{
			// TODO only tick down while player moving?
			if (ticking && controller.player.IsMoving)
			{
				nextTimeEncounter -= Time.deltaTime;
				if (nextTimeEncounter < 0.0f)
				{
					RandomEncounter();
					SetRandomTime();
				}
			}
		}

		public void RandomEncounter() {

			//currently doesn't work
			float rand = Random.value;
			if (rand <= Probability)
			{
				StartEncounter(SelectRandomEncounter());
			}

		}

		public float CurveRandomNumber(float rand, AnimationCurve curve) {
			return curve.Evaluate(rand);
		}

		void SetRandomTime()
		{
			nextTimeEncounter = Random.Range(Mathf.Min(minEncounterTime, maxEncounterTime), Mathf.Max(minEncounterTime, maxEncounterTime));
		}

		protected void StartEncounter(Encounter e)
		{
			lastEncounter = e;
			GameObject go = e.InstantiateEnemyTeam();
			BattleManager.Instance.StartBattle(battleScene, go.transform);
		}

		protected Encounter SelectRandomEncounter()
		{
			List<WeightedEncounter> modifiedList = new List<WeightedEncounter>(encounterList);
			for(int i = 0; i < modifiedList.Count; ++i)
			{
				if (modifiedList[i].Option == lastEncounter)
				{
					WeightedEncounter modifiedEncounter = modifiedList[i];
					modifiedEncounter.Weight = modifiedEncounter.Weight * repeatModifier;
					modifiedList[i] = modifiedEncounter;
				}
			}
			float totalWeight = modifiedList.Sum(o => o.Weight);
			float value = UnityEngine.Random.Range(0, totalWeight);
			foreach (WeightedEncounter e in modifiedList)
			{
				value -= e.Weight;
				if (value <= 0)
				{
					return e.Option;
				}
			}
			// in case of rounding error, return last value
			return modifiedList.Last().Option;
		}
	}
}