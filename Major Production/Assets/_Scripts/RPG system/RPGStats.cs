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
			Fire,
			Ice,
			Healing
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
	}
}