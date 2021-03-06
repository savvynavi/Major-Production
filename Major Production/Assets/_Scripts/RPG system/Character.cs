﻿using System.Collections;
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
	public class EquipmentSlot
	{
		public EquipmentSlot(Character character, Equipment.EquipmentType type)
		{
			this.character = character;
			this.type = type;
		}
		public Equipment.EquipmentType type { get; private set; }
		public Character character { get; private set; }
		public Equipment equippedItem { get; private set; }
		public bool IsEmpty { get { return equippedItem == null; } }

		public bool CanEquip(Equipment item)
		{
			bool matchesSlot = item != null && item.equipmentType == type;
			// also check correct class if weapon
			if (matchesSlot && item.equipmentType == Equipment.EquipmentType.Weapon)
			{
				matchesSlot &= item.classRequirement == character.classInfo.classType;
			}
			return matchesSlot;
		}

		// Equips item if allowed and slot is empty
		public bool Equip(Equipment item)
		{
			if (CanEquip(item) && equippedItem == null && item != null)
			{
				equippedItem = item;
				item.ApplyEffect(character);
				return true;
			}
			else
			{
				return false;
			}
		}

		// Unequips current item
		public Equipment Unequip()
		{
			Equipment oldItem = equippedItem;
			if (equippedItem != null)
			{
				equippedItem.RemoveEffect(character);
				equippedItem = null;
			}
			return oldItem;
		}

		public bool SwapItem(EquipmentSlot other)
		{
			if (other.type == this.type && other.character == this.character)
			{
				Equipment temp = this.equippedItem;
				this.equippedItem = other.equippedItem;
				other.equippedItem = temp;
				return true;
			}
			else {
				return false;
			}
		}

		public void Load(string equipmentName)
		{
			RPGItems.Equipment instance = null;
			if (!string.IsNullOrEmpty(equipmentName))
			{
				instance = (RPGItems.Equipment)Factory<Item>.CreateInstance(equipmentName);
				Equip(instance);
			} else {
				equippedItem = null;
			}
		}
	}

	public class Character : MonoBehaviour, ISaveable{
		public const int maxActivePowers = 4;
		public const int maxRingSlots = 2;

		[Header("General Info")]
		public string characterName;
		public Color uiColour;
		public Sprite Portrait;
		public Sprite ButtonPortrait;
		public Animator anim;

		//base stats
		[Header("Character base stats")]
		public float speedStat;
		public float strStat;
		public float defStat;
		public float intStat;
		public float mindStat;
		public float hpStat;
		public float mpStat;
		public float dexStat;
		public float agiStat;

		//bodyparts
		[Header("bodypart transforms")]
		public Transform rootStat;
		public Transform headStat;
		public Transform torsoStat;
		public Transform leftHandStat;
		public Transform rightHandStat;
		public Transform feetStat;

		[Header("class and strengths/weaknesses")]
		public ClassInfo classInfo;
		public List<RPGStats.DmgType> Strengths;
		public List<RPGStats.DmgType> Weaknesses;

		public RPG.XP.Experience experience { get; private set; }

		[Header("position in turn order")]
		public int ChoiceOrder;

		//is true when that player is setting moves
		public bool ActivePlayer { get; set; }

		//dictionary stuff
		public Dictionary<RPGStats.Stats, float> CharaStats = new Dictionary<RPGStats.Stats, float>();

		//DICTIONARY OF BODYPARTS AND TRANSFORMS FOR PLAYER POWERS
		public Dictionary<Powers.EffectPosition, Transform> CharaBodyparts = new Dictionary<Powers.EffectPosition, Transform>(); 

		List<Powers> activePowers;	// maybe extract out to own class?
		public ReadOnlyCollection<Powers> ActivePowers { get { return activePowers.AsReadOnly(); } }

		//setting up transform dictionary
		public Transform Root {
			get {return CharaBodyparts[RPGsys.Powers.EffectPosition.ROOT]; }
			set { CharaBodyparts[RPGsys.Powers.EffectPosition.ROOT] = value; }
		}
		public Transform Head {
			get { return CharaBodyparts[RPGsys.Powers.EffectPosition.HEAD]; }
			set { CharaBodyparts[RPGsys.Powers.EffectPosition.HEAD] = value; }
		}
		public Transform Torso {
			get { return CharaBodyparts[RPGsys.Powers.EffectPosition.TORSO]; }
			set { CharaBodyparts[RPGsys.Powers.EffectPosition.TORSO] = value; }
		}
		public Transform LeftHand {
			get { return CharaBodyparts[RPGsys.Powers.EffectPosition.LEFT_HAND]; }
			set { CharaBodyparts[RPGsys.Powers.EffectPosition.LEFT_HAND] = value; }
		}
		public Transform RightHand {
			get { return CharaBodyparts[RPGsys.Powers.EffectPosition.RIGHT_HAND]; }
			set { CharaBodyparts[RPGsys.Powers.EffectPosition.RIGHT_HAND] = value; }
		}
		public Transform Feet {
			get { return CharaBodyparts[RPGsys.Powers.EffectPosition.FEET]; }
			set { CharaBodyparts[RPGsys.Powers.EffectPosition.FEET] = value; }
		}


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
			}
		}

		public Dictionary<RPGStats.Stats, float> CloneBaseStats()
		{
			Dictionary<RPGStats.Stats, float> statClone = new Dictionary<RPGStats.Stats, float>();
			foreach(RPGStats.Stats stat in System.Enum.GetValues(typeof(RPGStats.Stats)))
			{
				statClone.Add(stat, BaseStat(stat));
			}
			return statClone;
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
		[Header("Current target and effects")]
		public GameObject target;
		public List<Character> targetList;

		//Material material;
		public List<Status> currentEffects;

		public bool IsDead { get { return Hp <= 0; } }

		//stores the 1 weapon a character can wield
		public EquipmentSlot weapon { get; private set; }
		//stores the character's rings
		public EquipmentSlot ringL { get; private set; }
		public EquipmentSlot ringR { get; private set; }

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

			Root = rootStat;
			Head = headStat;
			LeftHand = leftHandStat;
			RightHand = rightHandStat;
			Torso = torsoStat;
			Feet = feetStat;

			targetList = new List<Character>();
			targetList = null;

			classInfo = Instantiate(classInfo);
			for(int i = 0; i < classInfo.classPowers.Count(); i++) {
				classInfo.classPowers[i] = Instantiate(classInfo.classPowers[i]);
			}

			// set first four class powers as active
			activePowers = new List<Powers>(classInfo.classPowers.Take(4));

			anim = GetComponent<Animator>();
			experience = GetComponent<RPG.XP.Experience>();
			weapon = new EquipmentSlot(this, Equipment.EquipmentType.Weapon);
			ringL = new EquipmentSlot(this, Equipment.EquipmentType.Ring);
			ringR = new EquipmentSlot(this, Equipment.EquipmentType.Ring);
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
		//public bool Unequip(Item item) {
		//	if (Equipment.Remove(item))
		//	{
		//		foreach(Buff buff in item.Effect.currentEffects)
		//		{
		//			buff.EquipRemove(this, item);
		//		}
		//		return true;
		//	}
		//	else
		//	{
		//		return false;
		//	}
		//}

		// Unequips all items and moves them into inventory
		public void UnequipAll()
		{
			InventoryManager inventory = GameController.Instance.inventory;
			List<Item> removed = new List<Item>();
			if (weapon.equippedItem != null)
			{
				removed.Add(weapon.Unequip());
			}
			if(ringL.equippedItem != null)
			{
				removed.Add(ringL.Unequip());
			}
			if(ringR.equippedItem != null)
			{
				removed.Add(ringR.Unequip());
			}
			inventory.AddRange(removed);
		}

		public bool HasEmptyRingSlot()
		{
			return ringL.IsEmpty || ringR.IsEmpty;
		}

		public bool PlaceInEmptyRingSlot(Equipment ring)
		{
			if(ring.equipmentType != Equipment.EquipmentType.Ring)
			{
				return false;
			}
			else
			{
				if(ringL.IsEmpty)
				{
					return ringL.Equip(ring);
				} else if(ringR.IsEmpty)
				{
					return ringR.Equip(ring);
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
				new JProperty("weapon", !weapon.IsEmpty ? Utility.TrimCloned(weapon.equippedItem.name) : ""),
				new JProperty("ringL", !ringL.IsEmpty ? Utility.TrimCloned(ringL.equippedItem.name) : ""),
				new JProperty("ringR", !ringR.IsEmpty ? Utility.TrimCloned(ringR.equippedItem.name) : ""),
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

			weapon.Load((string)data["weapon"]);
			ringL.Load((string)data["ringL"]);
			ringR.Load((string)data["ringR"]);
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