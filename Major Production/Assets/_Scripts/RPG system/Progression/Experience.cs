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
	}

	[RequireComponent(typeof(Character))]
	public class Experience : MonoBehaviour, Save.ISaveable
	{
		public Character character { get; private set; }

		[SerializeField] List<Level> levels;

		[SerializeField] int characterLevel;
		// NOTE: characterLevel and levels index are offset.
		// going from level 1 to level 2 uses levels[0]

		// probably will replace setter with something that applies level changes
		public int CharacterLevel { get { return characterLevel; } set { characterLevel = value; } }

		// Use this for initialization
		void Awake()
		{
			character = GetComponent<Character>();
			foreach(Level l in levels)
			{
				l.Init();
			}
		}

		// Update is called once per frame
		void Update()
		{

		}

		private void ApplyLevelUp(Level level)
		{
			character.ApplyStatChange(level.StatChangeDict);
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