using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dialogue
{
	[CreateAssetMenu(menuName = "Dialogue/Event/Load Scene")]
	public class LoadSceneEvent : DialogueEvent
	{
		public override string Describe(string target, string parameters)
		{
			return "Load Scene " + parameters;
		}

		public override void Execute(DialogueManager manager, string target, string parameters)
		{
			// Force conversation to end
			manager.EndConversation();
			SceneLoader.Instance.LoadScene(parameters);
		}
	}
}