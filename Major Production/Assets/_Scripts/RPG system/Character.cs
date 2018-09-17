using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using RPGItems;
using RPG.Save;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace RPGsys{
	public class Character : MonoBehaviour, ISaveable{
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
		public RPG.XP.Experience experience { get; private set; }

		public int ChoiceOrder;
		public Image Portrait;

		//dictionary stuff
		public Dictionary<RPGStats.Stats, float> CharaStats = new Dictionary<RPGStats.Stats, float>();

		// Returns the base stat value for a given stat
		public float BaseStat(RPGStats.Stats stat)
		{
			switch (stat)
			{
				case RPGStats.Stats.Speed:
					return speedStat;
				case RPGStats.Stats.Str:
					return strStat;
				case RPGStats.Stats.Def:
					return defStat;
				case RPGStats.Stats.Int:
					return intStat;
				case RPGStats.Stats.Mind:
					return mindStat;
				case RPGStats.Stats.Hp:
					return hpStat;
				case RPGStats.Stats.Mp:
					return mpStat;
				case RPGStats.Stats.Dex:
					return dexStat;
				case RPGStats.Stats.Agi:
					return agiStat;
				default:
					return -1;
			}
		}

		public void SetBaseStat(RPGStats.Stats stat, float value)
		{
			switch (stat)
			{
				case RPGStats.Stats.Speed:
					speedStat = value;
					break;
				case RPGStats.Stats.Str:
					strStat = value;
					break;
				case RPGStats.Stats.Def:
					defStat = value;
					break;
				case RPGStats.Stats.Int:
					intStat = value;
					break;
				case RPGStats.Stats.Mind:
					mindStat = value;
					break;
				case RPGStats.Stats.Hp:
					hpStat = value;
					break;
				case RPGStats.Stats.Mp:
					mpStat = value;
					break;
				case RPGStats.Stats.Dex:
					dexStat = value;
					break;
				case RPGStats.Stats.Agi:
					agiStat = value;
					break;
				default:
					throw new System.ArgumentOutOfRangeException();
					break;
			}
		}

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

		//stores the 1 weapon a character can wield
		public RPGItems.Item Weapon;
		//stores a list of equipables, mainly the rings
		public List<RPGItems.Item> Equipment;

		void OnEnable()
		{
		}

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
			experience = GetComponent<RPG.XP.Experience>();
		}

		//if timer less than zero, remove from effect list
		public void Timer(){
			List<Status> deadEffects = new List<Status>();
			foreach(Status effect in currentEffects){
				if(effect.equipable == Status.Equipable.True) {
					return;
				}
				
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

		#region Items
		// Uses or equips item and applies its effects. Returns false if not usable.
		public bool UseItem(Item item)
		{
			//TODO check if item usable?
			item.Effect.Apply(this, item);
            // Is this calling the wrong thing or forgetting to set equipable?
			if(item.Type == Item.ItemType.Equipable)
			{
				Equipment.Add(item);
			}
			return true;
		}

		// Removes item from equipment and unapplies its effects. Returns false if item not equipped
		public bool Unequip(Item item) {
			if (Equipment.Remove(item))
			{
				foreach(Buff buff in item.Effect.currentEffects)
				{
					buff.EquipRemove(this, item);
				}
				return true;
			}
			else
			{
				return false;
			}
		}
		#endregion

		#region ISaveable Implementation

		public JObject Save()
		{
			// Not saving base stats on assumption they won't change
			// Will have to do so if that changes

			// Not saving effects either because only equipment effects persist out of battle
			// Again this may change

			JObject saveData = new JObject(
				new JProperty("name", Utility.TrimCloned(name)),
				new JProperty("hp", Hp),
				new JProperty("mp", Mp),
				new JProperty("weapon", Weapon != null ? Utility.TrimCloned(Weapon.name) : ""),
				new JProperty("equipment",
					new JArray(from i in Equipment
							   select Utility.TrimCloned(i.name))));
			if(experience != null)
			{
				saveData.Add("experience", experience.Save());
			}
			return saveData;
		}

		public void Load(JObject data)
		{
            name = (string)data["name"];
            // HACK character names should be their own field, not the object's name

			// Load experience first because maybe that will affect equipment legality?
			if(experience != null)
			{
				experience.Load(data.Value<JObject>("experience"));
			}

			string weaponName = (string)data["weapon"];
			if (string.IsNullOrEmpty(weaponName))
			{
				Weapon = null;
			}
			else
			{
				Weapon = Factory<Item>.CreateInstance(weaponName);
			}

			foreach(JToken i in data["equipment"])
			{
				UseItem(Factory<Item>.CreateInstance((string)i));
			}

			Hp = (float)data["hp"];
			Mp = (float)data["mp"];
		}

		#endregion

		#region Level Up

		public void ApplyStatChange(Dictionary<RPGStats.Stats, float> StatChanges)
		{
			foreach(KeyValuePair<RPGStats.Stats, float> statValue in StatChanges)
			{
				float newBaseValue = BaseStat(statValue.Key) + statValue.Value;
				SetBaseStat(statValue.Key, newBaseValue);

				CharaStats[statValue.Key] += statValue.Value;
			}
		}

		#endregion
	}
}