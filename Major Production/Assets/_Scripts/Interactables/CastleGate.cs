using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG
{
	public class CastleGate : Interactable
	{
		[SerializeField] Dialogue.Conversation conversation;
		public Dialogue.DialogueActor kingActor;
		Dialogue.DialogueManager dialogueManager;
		public int bossEntrypointIndex;
		[SerializeField] string winCutsceneName;
		// TODO interaction causes scene to set entrypoint to 0, begins King Conversation
		// TODO king has BossBattleWin event


		public override void Hilight()
		{
			//TODO
		}

		public override void Interact(InteractionUser user)
		{
			SceneLoader.Instance.SetEntryPoint(bossEntrypointIndex);
			dialogueManager.StartConversation(conversation);
		}

		public override void Unhilight()
		{
			//TODO 
		}

		// Use this for initialization
		void Start()
		{
			// TODO put king actor into dialogue manager
			dialogueManager = FindObjectOfType<Dialogue.DialogueManager>();

			// May change around based on conversation
			dialogueManager.actors["King"] = kingActor;
		}

		// Update is called once per frame
		void Update()
		{

		}

		public void BossBattleWin()
		{
			// TODO load win screen
			UnityEngine.SceneManagement.SceneManager.LoadScene(winCutsceneName);
			Debug.Log("You won the game!!!");
		}
	}
}