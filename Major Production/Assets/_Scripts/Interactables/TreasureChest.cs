using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG {
	public class TreasureChest : Interactable {

		[SerializeField]List<RPGItems.Item> contents;
		[SerializeField] DialogueBox dialogue;
		[SerializeField] Sprite treasureImage;
		[SerializeField] ParticleSystem sparkleEffect;
		[SerializeField] Light spotlight;
		// TODO saving treasure chest
		public override void Interact(InteractionUser user)
		{
			StartCoroutine(OpenChestRoutine(user));
		}

		public override void Hilight()
		{
			// TODO make chest glow, show particle effects
		}

		public override void Unhilight()
		{
			// TODO turn off stuff from above
		}

		// Use this for initialization
		void Start() {

		}

		// Update is called once per frame
		void Update() {

		}

		IEnumerator OpenChestRoutine(InteractionUser user)
		{
			// TODO figure out right way to pause stuff
			// maybe something on SceneController
			GameController.Instance.Pause();
			user.CanInteract = false;

			// TODO factor out dialogue stuff to some other class

			// HACK this feels dirty
			bool wait = true;
			UnityEngine.Events.UnityAction continueAction = () => wait = false;
			System.Func<bool> waitP = () => { return wait; };

			dialogue.SetTitle("Treasure Chest");
			dialogue.ClearButtons();
			dialogue.AddButton("Next", continueAction);
			dialogue.SetPortrait(treasureImage);
			dialogue.ShowBox();
			if (contents.Count > 0)
			{
				dialogue.SetDialogue("It was full of treasure!");
				yield return new WaitWhile(waitP);
				wait = true;
				dialogue.SetDialogue("You found:\n" + ContentsText()); // TODO say what items were found
				dialogue.ClearButtons();
				dialogue.AddButton("Exit", continueAction);
				dialogue.RebuildLayout();
				yield return new WaitWhile(waitP);
				// Add items to inventory
				foreach (RPGItems.Item item in contents) {
					GameController.Instance.inventory.Add(Instantiate(item));
				}
				EmptyChest();
			} else
			{
				dialogue.SetDialogue("The chest was empty");
				dialogue.ClearButtons();
				dialogue.AddButton("Exit", continueAction);
				dialogue.RebuildLayout();
				yield return new WaitWhile(waitP);
			}
			dialogue.ClearButtons();
			dialogue.HideBox();

			user.CanInteract = true;
			GameController.Instance.Unpause();
		}

		string ContentsText()
		{
			System.Text.StringBuilder textBuilder = new System.Text.StringBuilder();
			foreach(RPGItems.Item item in contents)
			{
				textBuilder.Append(item.Name);
				textBuilder.Append('\n');
			}
			return textBuilder.ToString();
		}

		void EmptyChest()
		{
			contents.Clear();
			sparkleEffect.Stop();
			spotlight.enabled = false;
		}
	}
}