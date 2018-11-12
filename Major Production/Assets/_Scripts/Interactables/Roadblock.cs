using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RPG {
	[RequireComponent(typeof(PersistentTrigger))]
	public class Roadblock : Interactable
	{
		[SerializeField] PersistentTrigger trigger;
		// HACK this feels dirty
		[SerializeField] DialogueBox dialogue;
		[SerializeField] Sprite roadblockImage;

		private void Start()
		{
			trigger = GetComponent<PersistentTrigger>();
			if (trigger.Triggered)
			{
				gameObject.SetActive(false);
			}
		}

		public override void Hilight()
		{
			//TODO
		}

		public override void Interact(InteractionUser user)
		{
			StartCoroutine(TryBreakRoadblockRoutine(user));
		}

		public override void Unhilight()
		{
			//TODO
		}

		IEnumerator TryBreakRoadblockRoutine(InteractionUser user)
		{
			SceneLoader.Instance.currentSceneController.SetBusy();
			user.CanInteract = false;
			// HACK this feels dirty
			bool wait = true;
			UnityEngine.Events.UnityAction continueAction = () => wait = false;
			System.Func<bool> waitP = () => { return wait; };

			dialogue.SetTitle("Roadblock");
			dialogue.ClearButtons();
			dialogue.AddButton("Next", continueAction);
			dialogue.SetPortrait(roadblockImage);
			dialogue.ShowBox();
			dialogue.SetDialogue("A pile of objects blocks your path out of the town");
			yield return new WaitWhile(waitP);
			wait = true;
			if(!(SceneLoader.Instance.CheckBool("06 Barracks_Interior", "knight_battle")))
			{
				dialogue.SetDialogue("You need a strong knight to move these objects!");
				yield return new WaitWhile(waitP);
			}else if (!(SceneLoader.Instance.CheckBool("04 Town","rogue_battle")))
			{
				dialogue.SetDialogue("You need an expert in breaking and entering for this job.");
				yield return new WaitWhile(waitP);
			}
			else if (!(SceneLoader.Instance.CheckBool("05 Tavern_Interior", "bard_battle")))
			{
				dialogue.SetDialogue("You can't work without musical accompaniment!");
				yield return new WaitWhile(waitP);
			}
			else
			{
				dialogue.SetDialogue("Your party breaks down the roadblock");
				yield return new WaitWhile(waitP);
				// TODO break down barricade particle effect/animation
				yield return RoadblockFallRoutine();
				gameObject.SetActive(false);
			}

			dialogue.ClearButtons();
			dialogue.HideBox();
			user.CanInteract = true;
			SceneLoader.Instance.currentSceneController.ClearBusy();
		}

		IEnumerator RoadblockFallRoutine()
		{
			// TODO use routine for falling down animation, trigger particle effect
			yield return new WaitForEndOfFrame();
		}
	}
}