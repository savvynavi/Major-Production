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

		public void Init()
		{
			StatChangeDict = StatChanges.ToDictionary();
		}

        public void ApplyLevelUp(Character character)
        {
            character.ApplyStatChange(StatChangeDict);
        }
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

        public bool LevelUp()
        {
            if (characterLevel < MaxLevel) {
                levels[characterLevel - 1].ApplyLevelUp(character);
                ++characterLevel;
                return true;
            }
            else
            {
                return false;
            }
        }

        public void LevelUp(int newLevel)
        {
            newLevel = Mathf.Min(newLevel, MaxLevel);
            while(characterLevel < newLevel)
            {
                if (!LevelUp())
                {
                    break;
                }
            }
        }

		public void AddXp(int xp)
		{
			exp += xp;
			Debug.Log(character.name + " new XP = " + exp);
			// HACK figure out rule for XP to level, where to store that data, etc
			// for now, just do 100xp to level up
			while (exp >= 100)
			{
				exp -= 100;
				LevelUp();
				Debug.Log(character.name + " levelled up to L " + characterLevel + ", XP = " + exp);
			}
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