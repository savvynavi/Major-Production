using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RPGsys {

	[RequireComponent(typeof(Character))]
	public abstract class EnemyBehaviour : MonoBehaviour {
		protected Character chara = null;
		protected TurnBehaviour turnBehav = null;

		public Character GetChara{
			get { return chara; }
		}
			
		protected void Awake() {
			chara = GetComponent<Character>();
			turnBehav = FindObjectOfType<TurnBehaviour>();
		}

        public abstract void AddAttack(List<Character> targets, List<Character> allies);
	}

}