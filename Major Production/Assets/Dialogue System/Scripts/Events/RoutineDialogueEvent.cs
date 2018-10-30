using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dialogue
{
	// An asset used to allow conversations to perform actions
	public abstract class DialogueRoutineEvent : DialogueEvent
	{
		public abstract IEnumerator DoRoutine(DialogueManager manager, string target, string parameters);
	}
}