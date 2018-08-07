using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using Sprache;
using Dialogue;

public class DialogueMarkupTest {

	[Test]
	public void LiteralTextParses() {
		LiteralText literal = MarkupParser.Literal.Parse("Sphinx of black quartz judge my vow.");
		Assert.AreEqual("Sphinx of black quartz judge my vow.", literal.ToString());
	}

	[Test]
	public void LiteralTextExcludesBrace()
	{
		LiteralText literal = MarkupParser.Literal.Parse("Sphinx of black quartz judge my vow.{markup in here}");
		Assert.AreEqual("Sphinx of black quartz judge my vow.", literal.ToString());
	}

	// TODO further tests
}
