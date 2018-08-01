using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

namespace RPGsys{
	public class Character : MonoBehaviour{
		//base stats
		public float speedStat;
		public float strStat;
		public float defStat;
		public float intStat;
		public float mindStat;
		public float hpStat;
		public float mpStat;
		public float dexStat;
		public float agiStat;

		public ClassInfo classInfo;
		public List<RPGStats.DmgType> Strengths;
		public List<RPGStats.DmgType> Weaknesses;
		public Animator anim;

		public int ChoiceOrder;

		//dictionary stuff
		public Dictionary<RPGStats.Stats, float> CharaStats = new Dictionary<RPGStats.Stats, float>();

		public float Speed{
			get { return CharaStats[RPGStats.Stats.Speed]; }
			set { CharaStats[RPGStats.Stats.Speed] = value; }
		}

		public float Str{
			get { return CharaStats[RPGStats.Stats.Str]; }
			set { CharaStats[RPGStats.Stats.Str] = value; }
		}
		public float Def{
			get { return CharaStats[RPGStats.Stats.Def]; }
			set { CharaStats[RPGStats.Stats.Def] = value; }
		}
		public float Int{
			get { return CharaStats[RPGStats.Stats.Int]; }
			set { CharaStats[RPGStats.Stats.Int] = value; }
		}
		public float Mind{
			get { return CharaStats[RPGStats.Stats.Mind]; }
			set { CharaStats[RPGStats.Stats.Mind] = value; }
		}
		public float Hp{
			get { return CharaStats[RPGStats.Stats.Hp]; }
			set { CharaStats[RPGStats.Stats.Hp] = value; }
		}
		public float Mp{
			get { return CharaStats[RPGStats.Stats.Mp]; }
			set { CharaStats[RPGStats.Stats.Mp] = value; }
		}
		public float Dex{
			get { return CharaStats[RPGStats.Stats.Dex]; }
			set { CharaStats[RPGStats.Stats.Dex] = value; }
		}
		public float Agi{
			get { return CharaStats[RPGStats.Stats.Agi]; }
			set { CharaStats[RPGStats.Stats.Agi] = value; }
		}
		public GameObject target;

		//Material material;
		public List<Status> currentEffects;

		void Awake(){
			Speed = speedStat;
			Str = strStat;
			Def = defStat;
			Int = intStat;
			Mind = mindStat;
			Hp = hpStat;
			Mp = mpStat;
			Dex = dexStat;
			Agi = agiStat;


			classInfo = Instantiate(classInfo);
			for(int i = 0; i < classInfo.classPowers.Count(); i++) {
				classInfo.classPowers[i] = Instantiate(classInfo.classPowers[i]);
			}

			anim = GetComponent<Animator>();
		}

		//if timer less than zero, remove from effect list
		public void Timer(){
			List<Status> deadEffects = new List<Status>();
			foreach(Status effect in currentEffects){
				//if the timer is less than zero, add to dead list, else count down
				effect.UpdateEffect(this);
				if(effect.timer < 0) {
					deadEffects.Add(effect);
				}

			}

			//if there are things in dead list, loop over death list, remove from current effects
			if(deadEffects.Count() > 0) {
				foreach(Status deadEffect in deadEffects) {
					deadEffect.Remove(this);
					currentEffects.Remove(deadEffect);
				}
			}
		}
	}
}