using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RPGsys {

	[RequireComponent(typeof(Character))]
	public class EnemyBehaviour : MonoBehaviour {
		Character chara = null;
		TurnBehaviour turnBehav = null;
		int rand;

		public Character GetChara{
			get { return chara; }
		}
			
		private void Awake() {
			chara = GetComponent<Character>();
			turnBehav = FindObjectOfType<TurnBehaviour>();
		}

		// Update is called once per frame
		public void AddEnemyAttackRand(Character target) {
			chara.target = target.gameObject;
			rand = Random.Range(0, chara.classInfo.classPowers.Count);
			turnBehav.turnAddAttackEnemy(chara.classInfo.classPowers[rand], chara);
		}
	}

}