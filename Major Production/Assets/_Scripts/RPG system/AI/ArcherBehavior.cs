using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPGsys
{
    public class ArcherBehavior : EnemyBehaviour
    {
        [SerializeField] string reloadName;      //hack
        [SerializeField] string arrowName;      //hack
        bool loaded;

        // Use this for initialization
        void Start()
        {
            loaded = true;
            //TODO maybe should check valid powers at start?
        }

        public override void AddAttack(List<Character> targets, List<Character> allies)
        {
            //HACK maybe drag powers into behaviour?
            Powers reloadPower = chara.classInfo.classPowers.Find((Powers p) => { return p.powName == reloadName; });
            Powers arrowPower = chara.classInfo.classPowers.Find((Powers p) => { return p.powName == arrowName; });
            if(reloadPower == null || arrowPower == null)
            {
                Debug.LogWarning(chara.name + " does not have required powers for ArcherBehaviour");
                AttackRandomFallback(targets, allies);
            } else
            {
                int targetIndex = Random.Range(0, targets.Count);
                Character target = targets[targetIndex];
                chara.target = target.gameObject;
                if (loaded)
                {
                    GetTurnBehaviour().turnAddAttackEnemy(arrowPower, chara);
                    loaded = false;
                } else
                {
                    GetTurnBehaviour().turnAddAttackEnemy(reloadPower, chara);
                    loaded = true;
                }
            }
        }

    }
}