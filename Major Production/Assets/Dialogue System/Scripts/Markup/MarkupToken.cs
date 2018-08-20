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
				// TODO make case insensitive
			if(Field.ToLowerInvariant() == "name")
			{
				return actorObject.Name;
			} else
			{
				// TODO search through fields for appropriate thing?
				return actorObject.fields.GetNumber(Field).ToString();
			}
		}
	}

	// TODO might change up heirarchy (got to figure out grammar better)

	// TODO random choice token (contains list of literals or variables)
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
}