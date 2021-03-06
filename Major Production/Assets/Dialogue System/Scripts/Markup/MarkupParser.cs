﻿using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Dialogue.Parser;

namespace Dialogue
{
	public class MarkupParser
	{
		public static readonly Parser<MarkupToken> Literal = new LiteralParser("{}".ToCharArray());

		public static readonly Parser<MarkupToken> QuotedLiteral = new TrimWhitespace<MarkupToken>(new Between<MarkupToken>('"', new LiteralParser("{}\"".ToCharArray())));

		public static readonly Parser<MarkupToken> VariableToken = new TrimWhitespace<MarkupToken>(new VariableParser());

		public static readonly Parser<MarkupToken> Concatenated = new ConcatenateParser(new Or<MarkupToken>(VariableToken, QuotedLiteral), '+');

		public static readonly Parser<MarkupToken> RandomToken = new RandomParser(Concatenated, '|'); 

		public static readonly Parser<MarkupToken> MarkupBlock = new Between<MarkupToken>('{', RandomToken, '}');
		//TODO having several things concatenated in the block

		public static readonly Parser<IEnumerable<MarkupToken>> Dialogue = new Many<MarkupToken>(new Or<MarkupToken>(Literal, MarkupBlock),true);
		
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
			if (input.Length == 0)
			{
				return new Result<MarkupToken>(new LiteralText(""), 0);
			}
			if (index == input.Length)
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
		public override Result<MarkupToken> Parse(string input, int index = 0)
		{
			// split at dot, first part is actor second part is field
			CheckInputValid(input, index);
			if (index == input.Length)
			{
				return new Result<MarkupToken>(new ParseError("Unexpected end of input"));
			}
			int consumed = 0;
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
			return new Result<MarkupToken>(new Variable(actor, field), consumed);
		}
	}

	public class RandomParser : Parser<MarkupToken>
	{
		ManyDelimited<MarkupToken> delimiterParser;

		public RandomParser(Parser<MarkupToken> parser, char delimiter)
		{
			delimiterParser = new ManyDelimited<MarkupToken>(parser, delimiter);
		}
		public override Result<MarkupToken> Parse(string input, int index = 0)
		{
			Result < IEnumerable < MarkupToken >> tryResult = delimiterParser.Parse(input, index);
			if (tryResult.Success)
			{
				List<MarkupToken> options = new List<MarkupToken>(tryResult.Value);
				if(options.Count == 1)
				{
					return new Result<MarkupToken>(options[0], tryResult.Consumed);
				} else
				{
					return new Result<MarkupToken>(new RandomChoice(options), tryResult.Consumed);
				}
			} else
			{
				return new Result<MarkupToken>(tryResult.Error);
			}
			
		}
	}

	public class ConcatenateParser : Parser<MarkupToken>
	{
		ManyDelimited<MarkupToken> delimiterParser;

		public ConcatenateParser(Parser<MarkupToken> parser, char delimiter)
		{
			delimiterParser = new ManyDelimited<MarkupToken>(parser, delimiter);
		}

		public override Result<MarkupToken> Parse(string input, int index = 0)
		{
			Result<IEnumerable<MarkupToken>> tryResult = delimiterParser.Parse(input, index);
			if (tryResult.Success)
			{
				List<MarkupToken> options = new List<MarkupToken>(tryResult.Value);
				if (options.Count == 1)
				{
					return new Result<MarkupToken>(options[0], tryResult.Consumed);
				}
				else
				{
					return new Result<MarkupToken>(new ConcatenatedTokens(options), tryResult.Consumed);
				}
			}
			else
			{
				return new Result<MarkupToken>(tryResult.Error);
			}
		}
	}
}