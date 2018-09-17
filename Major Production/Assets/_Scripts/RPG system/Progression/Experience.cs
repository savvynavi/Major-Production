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

		// probably will replace setter with something that applies level changes
		public int CharacterLevel { get { return characterLevel; }}
        public int MaxLevel { get { return levels.Count + 1; } }

		// Use this for initialization
		void Awake()
		{
			character = GetComponent<Character>();
           characterLevel = 1;
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
            newLevel = Mathf.Max(newLevel, MaxLevel);
            while(characterLevel < newLevel)
            {
                if (!LevelUp())
                {
                    break;
                }
            }
        }

		public JObject Save()
		{
			throw new NotImplementedException();
		}

		public void Load(JObject data)
		{
			throw new NotImplementedException();
		}
	}
}