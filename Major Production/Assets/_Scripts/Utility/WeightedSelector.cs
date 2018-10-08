using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace RPG
{
	[Serializable]
	public class WeightedOption<T>
	{
		public T Option;
		public float Weight;

		public static T SelectRandom(IEnumerable<WeightedOption<T>> optionList)
		{
			float totalWeight = optionList.Sum(o => o.Weight);
			float value = UnityEngine.Random.Range(0, totalWeight);
			foreach (WeightedOption<T> o in optionList)
			{
				value -= o.Weight;
				if (value <= 0)
				{
					return o.Option;
				}
			}
			// in case of rounding error, return last value
			return optionList.Last().Option;
		}
	}
}
