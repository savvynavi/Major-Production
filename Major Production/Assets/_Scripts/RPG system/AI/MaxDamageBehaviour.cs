using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPGsys
{
    public class MaxDamageBehaviour : EnemyBehaviour
    {
        public override void AddAttack(List<Character> targets, List<Character> allies)
        {
            float maxDamage = float.MinValue;
            Powers bestPower = null;
            Character bestTarget = null;
            foreach(Character target in targets)
            {
				foreach(Powers power in chara.classInfo.classPowers) {
					float damage = power.CalculateDamage(chara, target);
					if(damage > maxDamage) {
						maxDamage = damage;
						bestPower = power;
						bestTarget = target;
					}
				}
			}
            chara.target = bestTarget.gameObject;
			GetTurnBehaviour().turnAddAttackEnemy(bestPower, chara);
        }
    }
}