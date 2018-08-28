using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPGsys
{
    public class RandomBehaviour : EnemyBehaviour
    {
        private void Awake()
        {
            AwakeInit();
        }

        public override void AddAttack(List<Character> targets, List<Character> allies)
        {
            int targetIndex = Random.Range(0, targets.Count);
            Character target = targets[targetIndex];
            chara.target = target.gameObject;
            int powerIndex = Random.Range(0,chara.classInfo.classPowers.Count);
            Powers powers = chara.classInfo.classPowers[powerIndex];
            turnBehav.turnAddAttackEnemy(powers, chara);
        }
    }
}