using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using RPGItems;
using RPG.Save;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace RPGsys{
	public class Character : MonoBehaviour, ISaveable{
		public const int maxActivePowers = 4;
		public const int maxRingSlots = 2;

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
		public Sprite Portrait;
		//is true when that player is setting moves
		public bool ActivePlayer { get; set; }

		//dictionary stuff
		public Dictionary<RPGStats.Stats, float> CharaStats = new Dictionary<RPGStats.Stats, float>();

		List<Powers> activePowers;	// maybe extract out to own class?
		public ReadOnlyCollection<Powers> ActivePowers { get { return activePowers.AsReadOnly(); } }

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

		#region Stat Accessors
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
		#endregion
		public GameObject target;

		//Material material;
		public List<Status> currentEffects;

		//stores the 1 weapon a character can wield
		public RPGItems.Equipment Weapon;
		//stores the character's rings
		public RPGItems.Equipment ringL;
		public RPGItems.Equipment ringR;

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

			// set first four class powers as active
			activePowers = new List<Powers>(classInfo.classPowers.Take(4));


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
			return item.Use(this);
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

		public bool HasEmptyRingSlot()
		{
			return ringL == null || ringR == null;
		}

		public bool PlaceInEmptyRingSlot(Equipment ring)
		{
			if(ring.equipmentType != Equipment.EquipmentType.Ring)
			{
				return false;
			}
			else
			{
				if(ringL == null)
				{
					ringL = ring;
					return true;
				} else if(ringR == null)
				{
					ringR = ring;
					return true;
				} else
				{
					return false;
				}
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
				new JProperty("ringL", ringL != null ? Utility.TrimCloned(ringL.name) : ""),
				new JProperty("ringR", ringR != null ? Utility.TrimCloned(ringR.name) : ""),
				new JProperty("activePowers",
					new JArray(from p in activePowers
							   select Utility.TrimCloned(p.name))));
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

			activePowers.Clear();

			// set active powers
			foreach(string powerName in data["activePowers"])
			{
				Powers activatedPower = classInfo.classPowers.Find(p => Utility.TrimCloned(p.name) == powerName);
				if(activatedPower != null)
				{
					activePowers.Add(activatedPower);
				} else
				{
					//maybe throw?
					throw new System.ArgumentException("Power not found");
				}
			}
			if(activePowers.Count < 1)
			{
				throw new System.ArgumentException("No active powers");
				// maybe instead pick the first one?
			}

			Weapon = InstantiateAndApplyEquipment((string)data["weapon"]);
			ringL = InstantiateAndApplyEquipment((string)data["ringL"]);
			ringR = InstantiateAndApplyEquipment((string)data["ringR"]);
			
			Hp = (float)data["hp"];
			Mp = (float)data["mp"];
		}

		private RPGItems.Equipment InstantiateAndApplyEquipment(string equipmentName)
		{

			RPGItems.Equipment instance = null;
			if (!string.IsNullOrEmpty(equipmentName))
			{
				instance = (RPGItems.Equipment)Factory<Item>.CreateInstance(equipmentName);
				instance.ApplyEffect(this);
			}
			return instance;
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

		// add power to list of available powers
		public void AddPower(Powers power)
		{
			// Check if power already in list
			if (!classInfo.classPowers.Exists(p => p.Equals(power))){
				// Add new instance of power to list
				classInfo.classPowers.Add(Instantiate(power));
			}
			else
			{
				Debug.LogWarning("Power " + power.powName + " could not be added as it already exists in list.");
			}
			Debug.Log("Current powers: " + (from p in classInfo.classPowers select p.powName).Aggregate((a, b) => a + ", " + b));
		}

		#endregion

		#region Active Powers
		public bool ActivatePower(Powers power)
		{
			// Check there aren't too many powers on
			// Then, if power is in classInfo, and not in ActivePowers, add to ActivePowers
			if(activePowers.Count < Character.maxActivePowers &&
				classInfo.classPowers.Contains(power) &&
				!activePowers.Contains(power))
			{
				activePowers.Add(power);
				return true;
			} else
			{
				return false;
			}
		}

		public bool DeactivatePower(Powers power)
		{
			return activePowers.Remove(power);
		}
		#endregion
	}
}