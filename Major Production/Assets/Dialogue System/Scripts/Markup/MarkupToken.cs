using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Dialogue.Parser;

namespace Dialogue
{
	public abstract class MarkupToken
	{
		//TODO figure out best interface
		public abstract string Evaluate(DialogueManager manager);

		public static string EvaluateTokens(IEnumerable<MarkupToken> tokens, DialogueManager manager)
		{
			StringBuilder result = new StringBuilder();
			foreach(MarkupToken token in tokens)
			{
				result.Append(token.Evaluate(manager));
			}
			return result.ToString();
		}

		// HACK maybe move out to some utility class
		public static string FirstToUpper(string input)
		{
			if (string.IsNullOrEmpty(input))
			{
				throw new System.ArgumentException("Input string null or empty", "input");
			}
			if (input.Length > 1)
			{
				return char.ToUpper(input[0]) + input.Substring(1);
			} else
			{
				return input.ToUpper();
			}
		}
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

		public override string Evaluate(DialogueManager manager)
		{
			return Contents;
		}
	}

	// Represents a field or variable on an actor
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

		public override string Evaluate(DialogueManager manager)
		{
			// TODO get correct field from actor (or from field?)

			// TODO check for keyword, otherwise look for actor
			//TODO return something if actor not found
			DialogueActor actorObject;
			if (Actor == "SPEAKER")
			{
				actorObject = manager.GetCurrentActor();
			}
			else
			{
				actorObject = manager.actors[Actor];
			}

			switch (Field)
			{
				case "name":
				case "Name":
				case "NAME":
					return actorObject.Name;
				case "heshe":
					return actorObject.HeShe;
				case "HeShe":
					return FirstToUpper(actorObject.HeShe);
				case "himher":
					return actorObject.HimHer;
				case "HimHer":
					return FirstToUpper(actorObject.HimHer);
				case "hisher":
					return actorObject.HisHer;
				case "HisHer":
					return FirstToUpper(actorObject.HisHer);
				case "hishers":
					return actorObject.HisHers;
				case "HisHers":
					return FirstToUpper(actorObject.HisHers);
				default:
					// TODO search through fields for appropriate thing?
					return actorObject.fields.GetNumber(Field).ToString();
			}
		}
	}

	// Represents a group of possible options, one of which is randomly selected
	public class RandomChoice : MarkupToken
	{
		List<MarkupToken> Options;

		public RandomChoice(IEnumerable<MarkupToken> options)
		{
			Options = new List<MarkupToken>(options);
		}

		public override string Evaluate(DialogueManager manager)
		{
			return Options[Random.Range(0, Options.Count)].Evaluate(manager);
		}

		public override string ToString()
		{
			StringBuilder sb = new StringBuilder("[Random Choice: ");
			foreach (MarkupToken token in Options)
			{
				sb.Append(token.ToString());
				sb.Append("|");
			}
			sb.Length--;    // Remove last character
			sb.Append("]");
			return sb.ToString();
		}
	}

	public class ConcatenatedTokens : MarkupToken
	{
		List<MarkupToken> Tokens;

		public ConcatenatedTokens(IEnumerable<MarkupToken> tokens)
		{
			Tokens = new List<MarkupToken>(tokens);
		}

		public override string Evaluate(DialogueManager manager)
		{
			StringBuilder sb = new StringBuilder();
			foreach (MarkupToken token in Tokens)
			{
				sb.Append(token.Evaluate(manager));
			}
			return sb.ToString();
		}

		public override string ToString()
		{
			StringBuilder sb = new StringBuilder("[");
			foreach (MarkupToken token in Tokens)
			{
				sb.Append(token.ToString());
				sb.Append(" + ");
			}
			sb.Length -= 3;    // Remove last characters
			sb.Append("]");
			return sb.ToString();
		}
	}
}