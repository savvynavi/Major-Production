using System.Collections;
using System.Collections.Generic;
using System;
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

		public static readonly Parser<MarkupToken> VariableToken = new VariableParser(); 

		public static readonly Parser<MarkupToken> MarkupBlock = new Between<MarkupToken>('{', new Or<MarkupToken>(VariableToken, QuotedLiteral), '}');
		//TODO having several things concatenated in the block
		//TODO having random option in the block

		public static readonly Parser<MarkupToken> RandomChoice = null; //TODO

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
			CheckInputValid(input,index);
			if(index == input.Length)
			{
				return new Result<MarkupToken>(new ParseError("Unexpected end of input"));
			}
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
		// TODO write tests for each possible failed result
		public override Result<MarkupToken> Parse(string input, int index = 0)
		{
			// split at dot, first part is actor second part is field
			CheckInputValid(input, index);
			if (index == input.Length)
			{
				return new Result<MarkupToken>(new ParseError("Unexpected end of input"));
			}
			int consumed = 0;
			// Trim initial whitespace
			while(index + consumed < input.Length && char.IsWhiteSpace(input[index + consumed]))
			{
				++consumed;
			}
			if(index + consumed >= input.Length)
			{
				return new Result<MarkupToken>(new ParseError("Unexpected end of input"));
			}
			// Look for dot and get characters before it
			int dotIndex = input.IndexOf('.', index + consumed);
			if (dotIndex == -1)
			{
				return new Result<MarkupToken>(new ParseError("No '.' character found"));
			}

			// Get actor name
			int partLength = dotIndex - (index + consumed);
			string firstPart = input.Substring(index + consumed, partLength);
			consumed += partLength + 1;
			string actor = firstPart.TrimStart();   // Trim whitespace from start of actor
			if (string.IsNullOrEmpty(actor))
			{
				return new Result<MarkupToken>(new ParseError("Actor part of variable not found"));
			}
			int illegalCharacter = actor.IndexOfAny(".+{}|\" ".ToCharArray());
			if(illegalCharacter > -1)
			{
				return new Result<MarkupToken>(new ParseError("Illegal character '" + actor[illegalCharacter] + "' found in actor name"));
			}
			if (index + consumed >= input.Length)
			{
				return new Result<MarkupToken>(new ParseError("Unexpected end of input"));
			}
			// Find first example of illegal character in remaining text and treat as end of field
			illegalCharacter = input.IndexOfAny(".+{}|\" ".ToCharArray(), index + consumed);
			if(illegalCharacter == -1)
			{
				illegalCharacter = input.Length;
			}
			partLength = illegalCharacter - (index + consumed);
			string field = input.Substring(index + consumed, partLength);
			consumed += partLength;
			if (string.IsNullOrEmpty(field))
			{
				return new Result<MarkupToken>(new ParseError("Field part of variable not found"));
			}
			// Trim ending whitespace
			while (index + consumed < input.Length && char.IsWhiteSpace(input[index + consumed]))
			{
				++consumed;
			}
			return new Result<MarkupToken>(new Variable(actor, field), consumed);
		}
	}
}