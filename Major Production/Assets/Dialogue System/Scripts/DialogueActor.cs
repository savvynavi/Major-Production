using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Dialogue
{
	public enum EPronouns
	{
		He,
		She,
		It,
		They
	}

    public class DialogueActor : MonoBehaviour
    {
        public string Name;

        public Sprite Portrait;

        public FieldManager fields;

		[SerializeField] private StringEventDict customEventDictionary;
		public Dictionary<string, UnityEvent> customEvents;

		public EPronouns pronoun;

		public string HeShe
		{
			get
			{
				switch (pronoun)
				{
					case EPronouns.He:
						return "he";
					case EPronouns.She:
						return "she";
					case EPronouns.It:
						return "it";
					case EPronouns.They:
						return "they";
					default:
						return "(error: pronoun not found)";
				}
			}
		}

		public string HimHer
		{
			get
			{
				switch (pronoun)
				{
					case EPronouns.He:
						return "him";
					case EPronouns.She:
						return "her";
					case EPronouns.It:
						return "it";
					case EPronouns.They:
						return "them";
					default:
						return "(error: pronoun not found)";
				}
			}
		}

		public string HisHer
		{
			get
			{
				switch (pronoun)
				{
					case EPronouns.He:
						return "his";
					case EPronouns.She:
						return "her";
					case EPronouns.It:
						return "its";
					case EPronouns.They:
						return "their";
					default:
						return "(error: pronoun not found)";
				}
			}
		}

		public string HisHers
		{
			get
			{
				switch (pronoun)
				{
					case EPronouns.He:
						return "his";
					case EPronouns.She:
						return "hers";
					case EPronouns.It:
						return "its";
					case EPronouns.They:
						return "theirs";
					default:
						return "(error: pronoun not found)";
				}
			}
		}

		private void Awake()
		{
			customEvents = customEventDictionary.ToDictionary();
		}
	}
}
