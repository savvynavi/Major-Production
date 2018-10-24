using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPGsys
{
	public class RPGStats : MonoBehaviour
	{

		public enum DmgType
		{
			None,
			Physical,
			Magic,
			Healing
		}

		public enum Element 
		{
			None,
			Fire,
			Poison,
			Lightning,
			Ice
		}

		public enum Stats
		{
			Speed,
			Str,
			Def,
			Int,
			Mind,
			Hp,
			Mp,
			Dex,
			Agi
		}

		public enum ItemTypes
		{
			Head,
			Torso,
			Legs,
			Hand,
			Weapon
		}

		static Dictionary<Stats, string> StatNames = new Dictionary<Stats, string> {
			{ Stats.Speed, "Speed"},
			{Stats.Str, "Strength" },
			{Stats.Def, "Defence" },
			{Stats.Int, "Intelligence" },
			{Stats.Mind, "Mind" },
			{Stats.Hp, "HP" },
			{Stats.Mp, "MP" },
			{Stats.Dex, "Dexterity" },
			{Stats.Agi, "Agility" }
		};

		public static string GetStatName(Stats stat)
		{
			return StatNames[stat];
		}

	}
}