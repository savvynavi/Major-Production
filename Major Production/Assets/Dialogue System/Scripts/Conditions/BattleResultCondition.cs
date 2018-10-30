using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG;

namespace Dialogue
{
	[CreateAssetMenu(menuName ="Dialogue/Condition/Battle Result")]
	public class BattleResultCondition : Condition
	{
		public RPGsys.EBattleResult result;

		public override string Describe()
		{
			System.Text.StringBuilder description = new System.Text.StringBuilder();
			description.Append("Last battle was ");
			if (Not)
			{
				description.Append("not ");
			}
			description.Append(result.ToString("G"));
			return description.ToString();
		}

		public override bool Evaluate(DialogueManager dialogue)
		{
			return (result == BattleManager.Instance.lastResult) != Not;
		}
	}
}