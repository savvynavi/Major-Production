using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPGsys {
	[CreateAssetMenu(fileName = "StatusEffect", menuName = "RPG/StatusEffect", order = 2)]
	public class Buff : Status {
		public StatusEffect StatusEffects;

		public override void Apply(Character target, float duration) {
			if(target != null) {
				Buff tmp = Instantiate(this);
				// make as copy of the particles
				tmp.Clone(target);
				tmp.timer = duration;
				//adding instance of buff to the charaEffects list
				target.currentEffects.Add(tmp);
				SetStats(target);
			}
		}

		public override void Remove(Character target) {
			ResetStats(target);
			base.Remove(target);
		}

		void SetStats(Character target) {

			//RPGStats.Stats tmp = 0;
			switch(StatusEffects.effect) {
			case StatusEffectType.Buff: {
					//target.Str += StatusEffects.amount;
					RPGStats.Stats tmp = FindStatModified(StatusEffects.statBuff, target);
					target.CharaStats[tmp] += StatusEffects.amount;
					break;
				}
			case StatusEffectType.Debuff: {
					RPGStats.Stats tmp = FindStatModified(StatusEffects.statBuff, target);
					target.CharaStats[tmp] -= StatusEffects.amount;
					break;
				}
			case StatusEffectType.Heal: {
					target.Hp += StatusEffects.amount;
					break;
				}
			default:
				Debug.Log("error");
				break;
			}
		}

		void ResetStats(Character target) {
			//does effect here, fix later(not sustainable)
			switch(StatusEffects.effect) {
			case StatusEffectType.Buff: {
					//target.Str += StatusEffects.amount;
					RPGStats.Stats tmp = FindStatModified(StatusEffects.statBuff, target);
					target.CharaStats[tmp] -= StatusEffects.amount;
					break;
				}
			case StatusEffectType.Debuff: {
					RPGStats.Stats tmp = FindStatModified(StatusEffects.statBuff, target);
					target.CharaStats[tmp] += StatusEffects.amount;
					break;
				}
			default:
				Debug.Log("error");
				break;
			}
		}

		RPGStats.Stats FindStatModified(RPGStats.Stats statType, Character target) {
			//return RPGStats.Stats
			switch(statType) {
			case RPGStats.Stats.Speed:
				return RPGStats.Stats.Speed;				
			case RPGStats.Stats.Str:
				return RPGStats.Stats.Str;
			case RPGStats.Stats.Def:
				return RPGStats.Stats.Def;
			case RPGStats.Stats.Int:
				return RPGStats.Stats.Int;
			case RPGStats.Stats.Mind:
				return RPGStats.Stats.Mind;
			case RPGStats.Stats.Hp:
				return RPGStats.Stats.Hp;
			case RPGStats.Stats.Mp:
				return RPGStats.Stats.Mp;
			case RPGStats.Stats.Dex:
				return RPGStats.Stats.Dex;
			case RPGStats.Stats.Agi:
				return RPGStats.Stats.Agi;
			default:
				break;
			}
			//defaults to strength
			return RPGStats.Stats.Str;
		}
	}
}