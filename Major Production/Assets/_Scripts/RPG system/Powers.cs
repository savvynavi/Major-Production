using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPGsys {
	[CreateAssetMenu(fileName = "Ability", menuName = "RPG/Ability", order = 1)]
	public class Powers : ScriptableObject {
		//add in animation/sounds for moves later when they can be tested

		//abilities can either target a group 1 person, no limits on friendly fire
		public enum AreaOfEffect {
			Single,
			Group
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
		public AreaOfEffect areaOfEffect;
		public AbilityAnim anim;
		public string powName;
		public string description;
		public float duration;
		public List<Status> currentEffects;

		public string Description {
			get { return description; }
			set { value = description; }
		}

		//applies damage to target based on character stats + power used
		public void Apply(Character obj, Character target) {

            

            if (obj.Mp - manaCost >= 0) {
				float rand = Random.Range(1, 100);
				float MissRange = 10 + target.GetComponent<Character>().Agi - obj.GetComponent<Character>().Dex;
				float IncomingDmg = 0;

                //if the random number from 1-100 is less than the miss range, the attack hits
                if (rand >= MissRange)
                {
                    IncomingDmg = CalculateDamage(obj, target);
                    //loops over current effects on this power, applies them to the target
                    for (int i = 0; i < currentEffects.Count; i++)
                    {
                        currentEffects[i].Apply(target, duration);
                    }
                }

                //plays miss text if attack misses
                else if (rand < MissRange)
                {
                    GetScreenLoc tempLoc = new GetScreenLoc();
                    Vector2 location = tempLoc.getScreenPos(target.transform);
                    FloatingTextController.CreateMissText(("Miss!").ToString(), location);
                }


                //plays damage ammount animations if damage is delt
                if (IncomingDmg != 0)
                {
                    Debug.Log("Applying Damage");
                    GetScreenLoc tempLoc = new GetScreenLoc();
                    Vector2 location = tempLoc.getScreenPos(target.transform);
                    if (target.tag == "Enemy")
                        FloatingTextController.CreateDamageEnemyText((IncomingDmg).ToString(), location);
                    else if (target.tag == "Player")
                        FloatingTextController.CreateDamageAllyText((IncomingDmg).ToString(), location);
                }

                target.Hp -= IncomingDmg;
				obj.Mp -= manaCost;
			}
		}

		//used for potions/items
		public void Apply(Character character, RPGItems.Item item) {
			//loops over the items effects, adds to the character (duration = 0 is a one-off heal)
			for(int i = 0; i < currentEffects.Count; i++) {
				currentEffects[i].EquipApply(character, item);
			}
		}

		void setAnimName(Transform obj) {
			obj.GetComponent<Animator>().name = anim.ToString();
		}

		public float CalculateDamage(Character obj, Character target) {
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
			}else{
				attMod = 0;
			}


			float IncomingDmg = 0;
			float dmgReduction = 0;

			//decrease target hp by damage amount + the chatacters given stat
			if(obj.Mp - manaCost >= 0) {
				
				Debug.Log("HIT TARGET");

				//damage output
				IncomingDmg = damage + attMod;

				//if the attack type is either magic or physical it changes the mod
				if(dmgType == RPGStats.DmgType.Physical) {
					dmgReduction = IncomingDmg * (target.Def / 100);
				} else if(dmgType == RPGStats.DmgType.Magic) {
					dmgReduction = IncomingDmg * ((target.Int / 10)) / 100;
				}
				
				//get final damage output and subtract from target hp
				IncomingDmg -= dmgReduction;

               
            }

			return IncomingDmg;
		}

		// Checks powers are the same
		// Currently just checks powName, maybe should check name instead?
		public override bool Equals(object other)
		{
			if(other.GetType() == this.GetType())
			{
				Powers otherPower = other as Powers;
				return otherPower.powName == this.powName;
			}
			else
			{
				return false;
			}
		}
	}
}