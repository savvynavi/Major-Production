using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RPG {
	[RequireComponent(typeof(PersistentTrigger))]
	public class Roadblock : Interactable
	{
		[SerializeField] PersistentTrigger wasBroken;
		// HACK this feels dirty
		[SerializeField] DialogueBox dialogue;
		[SerializeField] Sprite roadblockImage;
		[SerializeField] BoxCollider trigger;
		public GameObject roadblockBefore;
		public GameObject roadblockAfter;
		public ParticleSystem breakEffect;
		public float swapTime;

		// HACK for testing this
		public bool forceAllow;

		private void Start()
		{
			wasBroken = GetComponent<PersistentTrigger>();
			if (wasBroken.Triggered)
			{
				BreakRoadblock();
			}
			else
			{
				roadblockBefore.SetActive(true);
				roadblockAfter.SetActive(false);
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
			// HACK for testing
			if (forceAllow)
			{
				dialogue.SetDialogue("Your party breaks down the roadblock");
				yield return new WaitWhile(waitP);
				trigger.enabled = false;
				EndDialogue(user);
				yield return RoadblockFallRoutine();
			} else 
			if (!(SceneLoader.Instance.CheckBool("06 Barracks_Interior", "knight_battle")))
			{
				dialogue.SetDialogue("You need a strong knight to move these objects!");
				yield return new WaitWhile(waitP);
				EndDialogue(user);
			}
			else if (!(SceneLoader.Instance.CheckBool("04 Town", "rogue_battle")))
			{
				dialogue.SetDialogue("You need an expert in breaking and entering for this job.");
				yield return new WaitWhile(waitP);
				EndDialogue(user);
			}
			else if (!(SceneLoader.Instance.CheckBool("05 Tavern_Interior", "bard_battle")))
			{
				dialogue.SetDialogue("You can't work without musical accompaniment!");
				yield return new WaitWhile(waitP);
				EndDialogue(user);
			}
			else
			{
				dialogue.SetDialogue("Your party breaks down the roadblock");
				yield return new WaitWhile(waitP);
				trigger.enabled = false;
				EndDialogue(user);
				yield return RoadblockFallRoutine();
			}

		}

		IEnumerator RoadblockFallRoutine()
		{
			breakEffect.Play();
			yield return new WaitForSeconds(swapTime);
			BreakRoadblock();
		}

		void BreakRoadblock()
		{
			roadblockBefore.SetActive(false);
			roadblockAfter.SetActive(true);
			trigger.enabled = false;
		}

		void EndDialogue(InteractionUser user)
		{
			dialogue.ClearButtons();
			dialogue.HideBox();
			SceneLoader.Instance.currentSceneController.ClearBusy();
		}
	}
}