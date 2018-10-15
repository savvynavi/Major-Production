using System.Collections;
using System.Collections.Generic;
using RPGsys;
using UnityEngine;

namespace RPGItems
{
	[CreateAssetMenu(fileName = "Consumable", menuName = "RPG/Consumable", order = 4)]

	public class Consumable : Item
	{
		public override bool IsUsable(Character character)
		{
			return true;
		}

		public override bool Use(Character character)
		{
			bool usable = IsUsable(character);
			if (usable)
			{
				ApplyEffect(character);
			}
			return usable;
		}
	}
}
