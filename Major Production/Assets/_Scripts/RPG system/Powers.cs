using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPGsys
{
	[CreateAssetMenu(fileName = "Ability", menuName = "RPG/Ability", order = 1)]
	public class Powers : ScriptableObject
	{
		//add in animation/sounds for moves later when they can be tested

		//abilities can either target a group 1 person, no limits on friendly fire
		public enum Target{
			Group,
			Single
		}

		public enum AbilityAnim {
			RIGHT_PUNCH,
			KICK,
			SPELL,
			ORC_AXE,
			DEATH,
			SHOOT,
			PUNCH,
			ORC_AXE_2
		};

		public float manaCost;
		public float damage;
		//possibly change this to a list to have multi-type abilities (eg, firebolt is both magic and fire type)
		public RPGStats.DmgType dmgType;
		public RPGStats.Stats statType;
		public Target target;
		public AbilityAnim anim; 
		public string powName;
		public string description;
		public float duration;
		public List<Status> currentEffects;

		public string Description {
			get { return description; }
			set { value = description; }
		}

		public void Apply(Character obj ,Character target){

			float attMod;
			//get stat that is being affected, none applied if no damage set
			if(damage > 0) {
				switch(statType) {
				case RPGStats.Stats.Speed:
					attMod = obj.Speed;
					break;
				case RPGStats.Stats.Str:
					attMod = obj.Str;
					break;
				case RPGStats.Stats.Def:
					attMod = obj.Def;
					break;
				case RPGStats.Stats.Int:
					attMod = obj.Int;
					break;
				case RPGStats.Stats.Mind:
					attMod = obj.Mind;
					break;
				case RPGStats.Stats.Hp:
					attMod = obj.Hp;
					break;
				case RPGStats.Stats.Mp:
					attMod = obj.Mp;
					break;
				case RPGStats.Stats.Dex:
					attMod = obj.Dex;
					break;
				case RPGStats.Stats.Agi:
					attMod = obj.Agi;
					break;
				default:
					Debug.Log("no given attack mod type, adding zero to damage");
					attMod = 0;
					break;
				}
			} else {
				attMod = 0;
			}


			//decrease target hp by damage amount + the chatacters given stat
			if(obj.Mp - manaCost >= 0){
				//SHUFFLEBAG MIGHT FIX BAD VALUES
				Debug.Log(obj.name);
				float rand = Random.Range(0, 100);
				Debug.Log("Rand Number: " + rand);
				//float ToHit = rand + obj.GetComponent<Character>().Dex - target.GetComponent<Character>().Agi;
				//Debug.Log("To Hit Number: " + ToHit);

				float accuracy = obj.Dex * 0.1f;
				float dodge = target.Agi * 0.05f;
				float chance = ((accuracy - dodge) / accuracy) * 100;

				float evade = (255 - target.Agi) + 1;

				//float hit = (obj.Dex * 0.4f - evade) + 9;

				float hit = obj.Dex / (target.Agi + dodge);

				Debug.Log("Chance to hit: " + chance);
				float IncomingDmg = 0;
				float dmgReduction = 0;

				//if(hit >= rand){
					Debug.Log("HIT TARGET");
					//damage output
					IncomingDmg = damage + attMod;
					//Debug.Log("Incoming Damage: " + IncomingDmg);

					//if the attack type is either magic or physical it changes the mod
					if(dmgType == RPGStats.DmgType.Physical){
						dmgReduction = IncomingDmg * (target.Def / 100);
					} else if(dmgType == RPGStats.DmgType.Magic){
						dmgReduction = IncomingDmg * ((target.Int / 10)) / 100;
					}
				//}

				//get final damage output and subtract from target hp
				IncomingDmg -= dmgReduction;
				//Debug.Log("damage Taken: " + IncomingDmg);
				target.Hp -= IncomingDmg;
				//Debug.Log("Target HP: " + target.Hp);
				obj.Mp -= manaCost;
			}


			//loops over current effects on this power, applies them to the target
			for(int i = 0; i < currentEffects.Count; i++) {
				currentEffects[i].Apply(target, duration);
			}
		}

		void setAnimName(Transform obj) {
			obj.GetComponent<Animator>().name = anim.ToString();
		}
	}
}