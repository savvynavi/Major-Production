using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Dialogue
{
	public class CustomEvent : DialogueEvent
	{
		public override string Describe(string target, string parameters)
		{
			return "Calls event '" + target + "' on dialogue manager."; 
		}

		public override void Execute(DialogueManager manager, string target, string parameters)
		{
			UnityEvent customEvent;
			if(manager.customEvents.TryGetValue(target, out customEvent))
			{
				customEvent.Invoke();
			}
		}
	}
}