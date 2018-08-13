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

        protected void AttackRandomFallback(List<Character> targets, List<Character> allies)
        {
            int targetIndex = Random.Range(0, targets.Count);
            Character target = targets[targetIndex];
            chara.target = target.gameObject;
            int powerIndex = Random.Range(0, chara.classInfo.classPowers.Count);
            Powers powers = chara.classInfo.classPowers[powerIndex];
            turnBehav.turnAddAttackEnemy(powers, chara);
        }
    }

}