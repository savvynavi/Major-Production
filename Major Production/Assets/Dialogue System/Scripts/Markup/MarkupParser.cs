using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sprache;

namespace Dialogue
{
	public class MarkupParser
	{
		// TODO escaping characters?
		// TODO multiple things in same field?
		// TODO random choices


		public static Parser<char> OpenBracket = Parse.Char('{');
		public static Parser<char> CloseBracket = Parse.Char('}');

		public static Parser<LiteralText> Literal = (
			from text in Parse.Except(Parse.AnyChar, OpenBracket.Or(CloseBracket)).Many().Text()
			select new LiteralText(text)
			);

	}
}