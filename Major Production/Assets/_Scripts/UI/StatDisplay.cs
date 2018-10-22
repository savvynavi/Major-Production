using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using RPGsys;

namespace RPG.UI
{
	// Displays a stat and the changes which could be applied to it
	// Maybe make this a base class which can inherit to text versions, bar versions, etc
	public class StatDisplay : MonoBehaviour
	{
		public class StatChangeData
		{
			public RPGItems.Item ItemToUse = null;
			public RPGItems.Item ItemToRemove = null;
			public Dictionary<RPGStats.Stats, float> changes = new Dictionary<RPGStats.Stats, float>();

			public void ApplyItemEffects(RPGItems.Item item, bool removing = false)
			{
				// Do nothing if passed null item
				if(item == null)
				{
					return;
				}

				float multiplier = removing ? -1 : 1;
				foreach (RPGsys.Status buff in item.Effect.currentEffects)
				{
					//HACK
					if (buff is RPGsys.Buff)
					{
						RPGsys.Buff b = (RPGsys.Buff)buff;
						RPGStats.Stats tgtStat = b.StatusEffects.statBuff;
						float amount = b.StatusEffects.amount * multiplier;
						switch (b.StatusEffects.effect)
						{
							case Status.StatusEffectType.Heal:
							//HACK change tgtStat to hp?
							case Status.StatusEffectType.Buff:
								break;
							case Status.StatusEffectType.Debuff:
								amount *= -1;
								break;
						}

						// Add to dictionary, or add to already existing value
						float currentChange = 0;
						changes.TryGetValue(tgtStat, out currentChange);
						changes[tgtStat] = currentChange + amount;
					}
				}
			}
		}

		public RPGsys.Character character;
		public RPGsys.RPGStats.Stats stat;

		[Header("Display")]
		[SerializeField]
		Text displayText;

		[Tooltip("0 = stat name, 1 = value, 2 = base value")]
		public string defaultFormat = "{0} {1:00.}";
		[Tooltip("0 = stat name, 1 = value, 2 = changed value, 3 = base value")]
		public string changedFormat = "{0} {1:00.} ({2:00.})";

		// TODO maybe have some Style object to make it easy to modify for multiple things?
		[SerializeField] Color defaultColour = new Color(0.2f,0.2f,0.2f,1);
		[SerializeField] Color increaseColour = new Color(0, 0.5f, 0, 1);
		[SerializeField] Color decreaseColour = Color.red;
		[SerializeField] Color unimportantColour = new Color(0.3f, 0.3f,0.3f,1);

		private void Awake()
		{
			displayText = GetComponent<Text>();
		}

		// Use this for initialization
		void Start()
		{

		}

		// Update is called once per frame
		void Update()
		{

		}

		public void Display(StatChangeData changeData = null)
		{


			if(changeData == null)
			{
				// Display normally
				displayText.color = defaultColour;
				displayText.text = string.Format(defaultFormat, RPGsys.RPGStats.GetStatName(stat), character.CharaStats[stat], character.BaseStat(stat));
			} else
			{
				// Display change 
				
				float change = 0;
				changeData.changes.TryGetValue(stat, out change);

				// If this is HP or MP don't show it being increased above maximum value
				if(stat == RPGStats.Stats.Hp || stat == RPGStats.Stats.Mp)
				{
					change = Mathf.Min(change, character.BaseStat(stat) - character.CharaStats[stat]);
				}

				if(change != 0)
				{
					if (change > 0)
					{
						displayText.color = increaseColour;
					}
					else
					{
						displayText.color = decreaseColour;
					}
					displayText.text = string.Format(changedFormat, RPGsys.RPGStats.GetStatName(stat), character.CharaStats[stat], character.CharaStats[stat] + change, character.BaseStat(stat));

				}
				else
				{
					displayText.color = unimportantColour;
					displayText.text = string.Format(defaultFormat, RPGsys.RPGStats.GetStatName(stat), character.CharaStats[stat], character.BaseStat(stat));
				}
			}
		}

	}
}