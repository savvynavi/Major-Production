using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPGsys;
using Newtonsoft.Json.Linq;

namespace RPG.XP
{
	[Serializable]
	public class StatFloatDict : Dialogue.SerializedDictionary<RPGStats.Stats, float> { };

	[Serializable]
	public class Level
	{
		[SerializeField] StatFloatDict StatChanges;
		public Dictionary<RPGStats.Stats, float> StatChangeDict;

		[SerializeField] List<Powers> unlockedPowers;

		public void Init()
		{
			StatChangeDict = StatChanges.ToDictionary();
		}

        public LevelUpEvent ApplyLevelUp(Character character)
        {
			Dictionary<RPGStats.Stats, float> OldStats = character.CloneBaseStats();
            character.ApplyStatChange(StatChangeDict);
			foreach(Powers power in unlockedPowers)
			{
				character.AddPower(power);
			}
			return new LevelUpEvent(this, OldStats);
        }
	}

	public class LevelUpEvent
	{
		public bool success { get; private set; }
		public Level level;
		public Dictionary<RPGStats.Stats, float> oldStats;

		public LevelUpEvent()
		{
			this.success = false;
			this.level = null;
			this.oldStats = null;
		}

		public LevelUpEvent(Level level, Dictionary<RPGStats.Stats, float> oldStats)
		{
			this.success = true;
			this.level = level;
			this.oldStats = oldStats;
		}

		public static implicit operator bool(LevelUpEvent e)
		{
			return e.success;
		}
	}

	public class XPEvent
	{
		public int initialLevel;
		public int finalLevel;
		public int initialXP;
		public int gainedXP;
		public int finalXP;
		public List<LevelUpEvent> levelUps = new List<LevelUpEvent>();
	}

	[RequireComponent(typeof(Character))]
	public class Experience : MonoBehaviour, Save.ISaveable
	{
		public Character character { get; private set; }

		[SerializeField] List<Level> levels;

        // serialized so I can see what's happening
		[SerializeField] int characterLevel;
		// NOTE: characterLevel and levels index are offset.
		// going from level 1 to level 2 uses levels[0]

		[SerializeField] int exp;

		// probably will replace setter with something that applies level changes
		public int CharacterLevel { get { return characterLevel; }}
        public int MaxLevel { get { return levels.Count + 1; } }

		// todo figure out setter method
		public int Exp { get { return exp; } }

		public int XPToLevel { get { return 100; } }

		// Use this for initialization
		void Awake()
		{
			character = GetComponent<Character>();
			characterLevel = 1;
			exp = 0;
			foreach(Level l in levels)
			{
				l.Init();
			}
		}

		// Update is called once per frame
		void Update()
		{

		}

        public LevelUpEvent LevelUp()
        {
			// start with fail event
			LevelUpEvent levelUpEvent = new LevelUpEvent();
            if (characterLevel < MaxLevel) {
                levelUpEvent = levels[characterLevel - 1].ApplyLevelUp(character);
                ++characterLevel;
            }
			return levelUpEvent;
        }

        public List<LevelUpEvent> LevelUp(int newLevel)
        {
			List<LevelUpEvent> levelEvents = new List<LevelUpEvent>();
            newLevel = Mathf.Min(newLevel, MaxLevel);
            while(characterLevel < newLevel)
            {
				LevelUpEvent currentEvent = LevelUp();
				if (currentEvent)
				{
					levelEvents.Add(currentEvent);
				}
				else
				{ 
					// Break from loop if LevelUp fails to increase level
                    break;
                }
            }
			return levelEvents;
        }

		public XPEvent AddXp(int xp)
		{
			XPEvent xpEvent = new XPEvent();
			xpEvent.initialLevel = characterLevel;
			xpEvent.initialXP = exp;
			xpEvent.gainedXP = xp;
			exp += xp;
			Debug.Log(character.name + " new XP = " + exp);
			// HACK figure out rule for XP to level, where to store that data, etc
			// for now, just do 100xp to level up
			while (exp >= XPToLevel)
			{
				exp -= XPToLevel;
				LevelUpEvent levelEvent = LevelUp();
				if (levelEvent)
				{
					xpEvent.levelUps.Add(levelEvent);
				}
				Debug.Log(character.name + " levelled up to L " + characterLevel + ", XP = " + exp);
			}
			if(characterLevel == MaxLevel)
			{
				exp = 0;
			}
			xpEvent.finalXP = exp;
			xpEvent.finalLevel = characterLevel;
			return xpEvent;
		}

		#region ISaveable Implementation
		public JObject Save()
		{
			return new JObject(
				new JProperty("characterLevel", characterLevel),
				new JProperty("exp", exp));
		}

		public void Load(JObject data)
		{
			// This assumes the character is level 1, from the prefab
			exp = data["exp"].Value<int>();
			int level = data["characterLevel"].Value<int>();
			LevelUp(level);
		}
		#endregion
	}
}