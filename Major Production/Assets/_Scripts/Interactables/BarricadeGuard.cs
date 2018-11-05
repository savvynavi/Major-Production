using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Dialogue.DialogueActor))]
public class BarricadeGuard : MonoBehaviour {
	// TODO figure out what components this uses (animator? just the transform?)
	public Dialogue.DialogueActor actor;

	private void Awake()
	{
		actor = GetComponent<Dialogue.DialogueActor>();
	}

	public void GuardAppear()
	{
		//TODO
	}

	public void GuardDeath()
	{
		//TODO
	}
}
