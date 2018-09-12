using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPGsys;

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
	public class Experience : MonoBehaviour
	{

		[SerializeField] List<Level> levels;

		// Use this for initialization
		void Awake()
		{
			foreach(Level l in levels)
			{
				l.Init();
			}
		}

		// Update is called once per frame
		void Update()
		{

		}
	}
}