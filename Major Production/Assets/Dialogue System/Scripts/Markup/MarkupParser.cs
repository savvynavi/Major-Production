using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dialogue.Parser;

namespace Dialogue
{
	public class MarkupParser
	{
		// TODO escaping characters?
		// TODO block can concatenate primitives with +?
		// TODO random choices

		//static readonly Parser<char, char> CharExceptBrackets = AnyCharExcept("{}");
		//static readonly Parser<char, char> CharExceptQuoteBrackets = AnyCharExcept("\"{}");
		//static readonly Parser<char, char> CharExceptOperators = AnyCharExcept("{}|.\"");

		//public static readonly Parser<char, MarkupToken> Literal =
		//	from text in CharExceptBrackets.ManyString()
		//	select (MarkupToken)(new LiteralText(text));

		//// TODO escaping quote in literal?
		//public static readonly Parser<char, MarkupToken> QuotedLiteral =
		//	from openQuote in Char('"')
		//	from text in CharExceptQuoteBrackets.ManyString()
		//	from closeQuote in Char('"')
		//	select (MarkupToken)(new LiteralText(text));

		//public static readonly Parser<char, MarkupToken> VariableToken =
		//	from actor in CharExceptOperators.ManyString()
		//	from dot in Char('.')
		//	from field in CharExceptOperators.ManyString()
		//	select (MarkupToken)(new Variable(actor, field));

		//public static readonly Parser<char, MarkupToken> MarkupBlock =
		//	from openbrace in Char('{')
		//	from token in VariableToken.Or(QuotedLiteral)
		//	from closebrace in Char('}')
		//	select token;

		//public static readonly Parser<char, IEnumerable<MarkupToken>> Dialogue = Literal.Or(MarkupBlock).Many();

	}
}