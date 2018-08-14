using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using Dialogue;

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

	// TODO testing quoted literals in a sentence




	[Test]
	public void VariableParses()
	{
		Variable var = (Variable)MarkupParser.MarkupBlock.Parse("{Guy.foo}").Value;
		Assert.AreEqual("Guy", var.Actor);
		Assert.AreEqual("foo", var.Field);
	}
	// TODO more variable unit tests

	//TODO figure out why these crash unity. Possibly Or causing trouble when different types?

	//[Test]
	//public void SentenceWithVariables()
	//{
	//	List<MarkupToken> tokens = new List<MarkupToken>(MarkupParser.Dialogue.Parse("This {Animal.species} is no more. It has {Death.euphemism}. It is an ex-{Animal.species}."));
	//	Assert.AreEqual("This ", tokens[0].ToString());
	//	Assert.AreEqual("[Variable: Animal.species]", tokens[1].ToString());
	//	Assert.AreEqual(" is no more. It has ", tokens[2].ToString());
	//	Assert.AreEqual("[Variable: Death.euphemism]", tokens[3].ToString());
	//	Assert.AreEqual(". It is an ex-", tokens[4].ToString());
	//	Assert.AreEqual("[Variable: Animal.species]", tokens[5].ToString());
	//}

	//TODO test random choice
	//[Test]
	//public void RandomChoice()
	//{

	//}

	//[Test]
	//public void SentenceWithRandomChoices()
	//{
	//	List<MarkupToken> tokens = new List<MarkupToken>(MarkupParser.Dialogue.Parse("This {Animal.species} is no more. It has {\"ceased to be\"|\"passed on\"}. It is {Death.behaviour|Animal.species|\"joined the choir invisible\"}"));
	//	Assert.AreEqual(" is no more. It has ", tokens[2].ToString());
	//	Assert.AreEqual("[Random Choice: ceased to be|passed on]", tokens[3].ToString());
	//	Assert.AreEqual("[Random Choice: [Variable: Death.behaviour]|[Variable: Animal.species]|joined the choir invisible", tokens[5].ToString());
	//} 

	// TODO further tests

}
