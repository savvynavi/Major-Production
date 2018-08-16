using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dialogue.Parser;

namespace Dialogue
{
	public class MarkupParser
	{
		// TODO escaping characters
		// TODO block can concatenate primitives with +?
		// TODO random choices

		public static readonly Parser<MarkupToken> Literal = new LiteralParser("{}".ToCharArray());

		public static readonly Parser<MarkupToken> QuotedLiteral = new Between<MarkupToken>('"', new LiteralParser("{}\"".ToCharArray()));

		public static readonly Parser<MarkupToken> VariableToken = null; //TODO

		public static readonly Parser<MarkupToken> MarkupBlock = new Between<MarkupToken>('{', new Or<MarkupToken>(VariableToken, QuotedLiteral), '}');
		//TODO having several things concatenated in the block
		//TODO having random option in the block

		public static readonly Parser<IEnumerable<MarkupToken>> Dialogue = new Many<MarkupToken>(new Or<MarkupToken>(Literal, MarkupBlock),true);

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

	public class LiteralParser : Parser<MarkupToken>{
		char[] excluded;
		public LiteralParser(char[] ExcludedCharacters)
		{
			excluded = ExcludedCharacters;
		}

		public override Result<MarkupToken> Parse(string input, int index = 0)
		{
			int endIndex = input.IndexOfAny(excluded, index);
			if(endIndex == -1)
			{
				endIndex = input.Length;
			}
			if(index == endIndex)
			{
				return new Result<MarkupToken>(new ParseError("Excluded character at start of index"));
			}
			else
			{
				int consumed = endIndex - index;
				return new Result<MarkupToken>(new LiteralText(input.Substring(index, consumed)), consumed);
			}
		}
	}

	public class VariableParser : Parser<MarkupToken>
	{
		public override Result<MarkupToken> Parse(string input, int index = 0)
		{
			// TODO split at dot, first part is actor second part is field
			// TODO return total characters consumed
			throw new System.NotImplementedException();
		}
	}
}