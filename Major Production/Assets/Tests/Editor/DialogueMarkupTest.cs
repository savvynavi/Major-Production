using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using Dialogue;
using Dialogue.Parser;

public class DialogueMarkupTest {

	// TODO decide behaviour for malformed input and test those handle correctly

	[Test]
	public void LiteralTextParses() {
		LiteralText literal = (LiteralText)MarkupParser.Literal.Parse("Sphinx of black quartz judge my vow.").Value;
		Assert.AreEqual("Sphinx of black quartz judge my vow.", literal.ToString());
	}

	[Test]
	public void LiteralTextExcludesBrace()
	{
		LiteralText literal = (LiteralText)MarkupParser.Literal.Parse("Sphinx of black quartz judge my vow.{markup in here}").Value;
		Assert.AreEqual("Sphinx of black quartz judge my vow.", literal.ToString());
	}


	[Test]
	public void QuotedLiteralParses()
	{
		LiteralText literal = (LiteralText)MarkupParser.QuotedLiteral.Parse("\"Sphinx of black quartz judge my vow.\"").Value;
		Assert.AreEqual("Sphinx of black quartz judge my vow.", literal.ToString());
	}

	[Test]
	public void EscapedQuote()
	{
		LiteralText literal = (LiteralText)MarkupParser.QuotedLiteral.Parse("\"Sphinx of \\\"black quartz\\\" judge my vow.\"").Value;
		Assert.AreEqual("Sphinx of \"black quartz\" judge my vow.", literal.ToString());
	}

	[Test]
	public void EmptyInput()
	{
		LiteralText literal = (LiteralText)MarkupParser.Literal.Parse("").Value;
		Assert.AreEqual("", literal.ToString());
		List<MarkupToken> tokens = new List<MarkupToken>(MarkupParser.Dialogue.Parse("").Value);
		Assert.AreEqual("", tokens[0].ToString());
	}

	// TODO testing quoted literals in a sentence




	[Test]
	public void VariableParses()
	{
		Variable var = (Variable)MarkupParser.MarkupBlock.Parse("{Guy.foo}").Value;
		Assert.AreEqual("Guy", var.Actor);
		Assert.AreEqual("foo", var.Field);
	}
	
	[Test]
	public void ConcatenatedTokenTest()
	{
		ConcatenatedTokens cat = (ConcatenatedTokens)MarkupParser.MarkupBlock.Parse("{\"First thing\" + Guy.foo}").Value;
		Assert.AreEqual("[First thing + [Variable: Guy.foo]]", cat.ToString());
	}

	[Test]
	public void SentenceWithVariables()
	{
		List<MarkupToken> tokens = new List<MarkupToken>(MarkupParser.Dialogue.Parse("This {Animal.species} is no more. It has {Death.euphemism}. It is an ex-{Animal.species}.").Value);
		Assert.AreEqual("This ", tokens[0].ToString());
		Assert.AreEqual("[Variable: Animal.species]", tokens[1].ToString());
		Assert.AreEqual(" is no more. It has ", tokens[2].ToString());
		Assert.AreEqual("[Variable: Death.euphemism]", tokens[3].ToString());
		Assert.AreEqual(". It is an ex-", tokens[4].ToString());
		Assert.AreEqual("[Variable: Animal.species]", tokens[5].ToString());
	}

	[Test]
	public void SentenceWithRandomChoices()
	{
		List<MarkupToken> tokens = new List<MarkupToken>(MarkupParser.Dialogue.Parse("This {Animal.species} is no more. It has {\"ceased to be\"|\"passed on\"}. It is {Death.behaviour|Animal.species|\"joined the choir invisible\"}").Value);
		Assert.AreEqual(" is no more. It has ", tokens[2].ToString());
		Assert.AreEqual("[Random Choice: ceased to be|passed on]", tokens[3].ToString());
		Assert.AreEqual("[Random Choice: [Variable: Death.behaviour]|[Variable: Animal.species]|joined the choir invisible]", tokens[5].ToString());
	}

	[Test]
	public void SentenceWithConcatenation()
	{
		List<MarkupToken> tokens = new List<MarkupToken>(MarkupParser.Dialogue.Parse("This {Animal.species} is no more. It has {\"ceased to be\"|\"passed on\"}. It is {Death.behaviour| \"an ex-\" + Animal.species|\"joined the choir invisible\"}").Value);
		Assert.AreEqual(" is no more. It has ", tokens[2].ToString());
		Assert.AreEqual("[Random Choice: ceased to be|passed on]", tokens[3].ToString());
		Assert.AreEqual("[Random Choice: [Variable: Death.behaviour]|[an ex- + [Variable: Animal.species]]|joined the choir invisible]", tokens[5].ToString());
	}

	// TODO further tests

}

public class DialogueMarkupVariableTests
{
	[Test]
	public void WhitespaceTrimmed()
	{
		Variable var = (Variable) MarkupParser.MarkupBlock.Parse("{ Guy.foo }").Value;
		Assert.AreEqual("Guy", var.Actor);
		Assert.AreEqual("foo", var.Field);
	}

	[Test]
	public void EmptyFails()
	{
		Result<MarkupToken> result = MarkupParser.VariableToken.Parse("");
		Assert.False(result.Success);
		result = MarkupParser.VariableToken.Parse(" ");
		Assert.False(result.Success);
		result = MarkupParser.VariableToken.Parse("Guy.");
		Assert.False(result.Success);
		result = MarkupParser.VariableToken.Parse(".foo");
		Assert.False(result.Success);
	}

	[Test]
	public void VariableHasDot()
	{
		Result<MarkupToken> result = MarkupParser.VariableToken.Parse("Guyfoo");
		Assert.False(result.Success);
	}

	[Test]
	public void ActorIsValid()
	{
		Result<MarkupToken> result = MarkupParser.VariableToken.Parse("G{uy.foo");
		Assert.False(result.Success);
		result = MarkupParser.VariableToken.Parse("Gu y.foo");
		Assert.False(result.Success);
	}

	[Test]
	public void FieldIsValid()
	{
		Result<MarkupToken> result = MarkupParser.VariableToken.Parse("Guy. foo");
		Assert.False(result.Success);
		result = MarkupParser.VariableToken.Parse("Guy.}foo");
		Assert.False(result.Success);
	} 
}
