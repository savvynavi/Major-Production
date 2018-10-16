using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG {
	[RequireComponent(typeof(PersistentTrigger))]
	public class TreasureChest : Interactable {

		[SerializeField]List<RPGItems.Item> contents;
		[SerializeField] DialogueBox dialogue;
		[SerializeField] Sprite treasureImage;
		[SerializeField] ParticleSystem sparkleEffect;
		[SerializeField] Light spotlight;
		[SerializeField] PersistentTrigger trigger;

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
			if (trigger.Triggered)
			{
				EmptyChest();
			}
		}

		// Update is called once per frame
		void Update() {

		}

		IEnumerator OpenChestRoutine(InteractionUser user)
		{
			// Suspend interactions
			SceneLoader.Instance.currentSceneController.SetBusy();
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
				dialogue.SetDialogue("You found:\n" + ContentsText());
				dialogue.ClearButtons();
				dialogue.AddButton("Exit", continueAction);
				dialogue.RebuildLayout();

				// Add items to inventory
				foreach (RPGItems.Item item in contents)
				{
					GameController.Instance.inventory.Add(Instantiate(item));
				}
				EmptyChest();
				trigger.Triggered = true;
				trigger.Save();

				yield return new WaitWhile(waitP);
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
			SceneLoader.Instance.currentSceneController.ClearBusy();
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