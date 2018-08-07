using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Dialogue
{
	public abstract class MarkupToken
	{
		//TODO figure out best interface
	}

	// Represents a string of text
	public class LiteralText : MarkupToken
	{
		public string Contents { get; private set; }

		public LiteralText(string contents)
		{
			Contents = contents;
		}

		public override string ToString()
		{
			return Contents;
		}
	}

	public class Variable : MarkupToken
	{
		// TODO might need more parameters (ie to get from fields?)
		public string Actor { get; private set; }
		public string Field { get; private set; }

		public Variable(string actor, string field)
		{
			Actor = actor;
			Field = field;
		}

		public override string ToString()
		{
			return "[Variable: " + Actor + "." + Field + "]";
		}
	}

	// TODO might change up heirarchy (got to figure out grammar better)

	// TODO random choice token (contains list of literals or variables)
	//public class RandomChoice : MarkupToken
	//{
	//	List<MarkupToken> Options;

	//	public RandomChoice(IEnumerable<MarkupToken> options)
	//	{
	//		Options = new List<MarkupToken>(options);
	//	}

	//	public override string ToString()
	//	{
	//		StringBuilder sb = new StringBuilder("[Random Choice: ");
	//		foreach(MarkupToken token in Options)
	//		{
	//			sb.Append(token.ToString());
	//			sb.Append("|");
	//		}
	//		sb.Length--;    // Remove last character
	//		sb.Append("]");
	//		return sb.ToString();
	//	}
	//}
}