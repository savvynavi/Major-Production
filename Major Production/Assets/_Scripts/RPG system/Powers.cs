﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPGsys {
	[CreateAssetMenu(fileName = "Ability", menuName = "RPG/Ability", order = 1)]
	public class Powers : ScriptableObject {
		//add in animation/sounds for moves later when they can be tested

		//abilities can either target a group 1 person, no limits on friendly fire
		public enum AreaOfEffect {
			Single,
			Group,
			Self
		}

		public enum AbilityAnim {
            Strike,
            LuteSlowPlaying,
            LutePlaying,
            MumbleRap,
            Sweep,
            Guard,
            WarCry,
            CallVolly,
            Stab,
            Hide,
            ThrowObject,
            SpellSingle,
            SpellMulti,
            GetDebuff,
            GetBuff,
            Death1,
            BleedOut,

            RIGHT_PUNCH,
			KICK,
			SPELL,
			ORC_AXE,
			DEATH,
			SHOOT,
			PUNCH,
			ORC_AXE_2
		};

		public enum EffectPosition {
			ROOT,
			HEAD,
			TORSO,
			LEFT_HAND,
			RIGHT_HAND,
			FEET
		};

		[Header("Power Details")]
		public string powName;
		public string description;
		public Sprite icon;
		public float damage;
		public AbilityAnim anim;

		[Header("Effects power does/its pos on character/target")]
		public GameObject EffectPlayer;
		public GameObject EffectTarget;
		public EffectPosition effectPosCharacter;
		public EffectPosition effectPosTarget;

		[Header("Damage Scale")]
		public RPGStats.DmgType dmgType;
		public RPGStats.Stats statType;
		public float ScalePercentage;

		[Header("Mana Cost")]
		public float manaCost;

		[Header("Area of Effect")]
		public AreaOfEffect areaOfEffect;
		[Header("Number of turns effects last")]
		public float duration;
		[Header("List of effects power does")]
		public List<Status> currentEffects;

		protected GameObject gameObjInstancePlayer;
		protected GameObject gameObjInstanceTarget;

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

				attMod *= ScalePercentage;

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

		//spawns a game object on the player at a given spot, or else at its feet
		void SpawnEffect(Character chara, Character target) {
			if(EffectPlayer != null) {
				gameObjInstancePlayer = Instantiate(EffectPlayer);
				if(chara.CharaBodyparts[effectPosCharacter] != null) {
					gameObjInstancePlayer.transform.parent = chara.transform;
					gameObjInstancePlayer.transform.position = chara.CharaBodyparts[effectPosCharacter].transform.position;

				} else {
					gameObjInstancePlayer.transform.parent = chara.transform;
					gameObjInstancePlayer.transform.localPosition = Vector3.zero;
				}

			}
		}

		void SendEffectToEnemy(Character target) {
			if(EffectTarget != null) {
				gameObjInstancePlayer = Instantiate(EffectTarget);
				if(target.CharaBodyparts[effectPosCharacter] != null) {
					gameObjInstancePlayer.transform.parent = target.transform;
					gameObjInstancePlayer.transform.position = target.CharaBodyparts[effectPosCharacter].transform.position;

				} else {
					gameObjInstancePlayer.transform.parent = target.transform;
					gameObjInstancePlayer.transform.localPosition = Vector3.zero;
				}

			}
		}

		// Checks powers are the same
		// Currently just checks powName, maybe should check name instead?
		public override bool Equals(object other)
		{
			if(other.GetType() == this.GetType())
			{
				Powers otherPower = other as Powers;
				return otherPower.powName ==  this.powName;
			}
			else
			{
				return false;
			}
		}

		public override int GetHashCode()
		{
			var hashCode = 1716941078;
			hashCode = hashCode * -1521134295 + base.GetHashCode();
			hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(powName);
			return hashCode;
		}

		public string GetPowerDetails()
		{ 
			// TODO output more details about the power
			// maybe make a version which estimates effect based on user and target?
			return description;
		}
	}
}