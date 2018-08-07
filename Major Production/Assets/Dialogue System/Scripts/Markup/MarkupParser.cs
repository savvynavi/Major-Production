using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sprache;

namespace Dialogue
{
	public class MarkupParser
	{
		// TODO escaping characters?
		// TODO block can concatenate primitives with +?
		// TODO random choices


		public static Parser<char> OpenBracket = Parse.Char('{');
		public static Parser<char> CloseBracket = Parse.Char('}');
		public static Parser<char> Brackets = OpenBracket.Or(CloseBracket);
		public static Parser<char> Pipe = Parse.Char('|');
		public static Parser<char> Point = Parse.Char('.');
		public static Parser<char> Quote = Parse.Char('"');
		public static Parser<char> Operators = Pipe.Or(Point).Or(Quote);

		// TODO might need to have parser for other characters ie also checks for pipes, dot

		public static Parser<LiteralText> Literal = (
			from text in Parse.Except(Parse.AnyChar, Brackets).Many().Text()
			select new LiteralText(text)
			);

		// TODO escaping quote in quoted literal?
		public static Parser<LiteralText> QuotedLiteral = (
			from text in Parse.Contained(Parse.Except(Parse.AnyChar, Brackets.Or(Quote)).Many().Text(), Quote, Quote)
			select new LiteralText(text)
			);

		public static Parser<Variable> VariableToken = (
			from actor in Parse.Except(Parse.AnyChar, Brackets.Or(Operators)).Many().Text()
			from dot in Point
			from field in Parse.Except(Parse.AnyChar, Brackets.Or(Operators)).Many().Text()
			select new Variable(actor, field)
			);


		//public static Parser<RandomChoice> RandomOptions = (
		//		// TODO implement
		//	);

		public static Parser<MarkupToken> MarkupBlock = (
			//from openbrace in OpenBracket
			//from token in VariableToken//.Or<MarkupToken>(QuotedLiteral).Or<MarkupToken>(RandomOptions)    //TODO add random choice
			//from closebrace in CloseBracket
			//select token
			Parse.Contained(VariableToken, OpenBracket, CloseBracket)

			);

		// TODO fix this: running the test breaks Unity. Apparently Or causes the TestRunner to crash?
		public static Parser<IEnumerable<MarkupToken>> Dialogue = (MarkupBlock.Or(Literal)).Many();

	}
}