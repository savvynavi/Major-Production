using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG
{
	public class CastleGate : Interactable
	{
		[SerializeField] Dialogue.Conversation conversation;
		// TODO interaction causes scene to set entrypoint to 0, begins King Conversation
		// TODO king has BossBattleWin event
		public override void Hilight()
		{
			throw new System.NotImplementedException();
		}

		public override void Interact(InteractionUser user)
		{
			throw new System.NotImplementedException();
		}

		public override void Unhilight()
		{
			throw new System.NotImplementedException();
		}

		// Use this for initialization
		void Start()
		{
			// TODO put king actor into dialogue manager
		}

		// Update is called once per frame
		void Update()
		{

		}

		public void BossBattleWin()
		{
			// TODO load win screen
		}
	}
}