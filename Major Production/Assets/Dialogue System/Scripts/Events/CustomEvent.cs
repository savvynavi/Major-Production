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
			System.Text.StringBuilder description = new System.Text.StringBuilder();
			description.AppendFormat("Calls event {0}", parameters);
			if(!string.IsNullOrEmpty(target))
			{
				description.AppendFormat(" on actor '{0}'", target);
			}
			return "Calls event '" + parameters + "' on dialogue manager."; 
		}

		public override void Execute(DialogueManager manager, string target, string parameters)
		{
			UnityEvent customEvent;
			if (string.IsNullOrEmpty(target))
			{
				if (manager.customEvents.TryGetValue(parameters, out customEvent))
				{
					customEvent.Invoke();
				}
				else
				{
					Debug.LogWarning("Event '" + parameters + "; not found in dialogue manager");
				}
			} else
			{
				DialogueActor actor;
				if(manager.actors.TryGetValue(target, out actor))
				{
					
				} else
				{
					Debug.LogWarning("Actor '" + target + "' not found in dialogue manager");
				}
			}


		}
	}
}