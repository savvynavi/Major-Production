using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RPGsys
{
	public class MoveDescriptionUI : MonoBehaviour
	{
		[SerializeField] Text descriptionText;

		public void Display(TurnBehaviour.TurnInfo info)
		{
			descriptionText.text = info.player.characterName + " used " + info.ability.powName;
		}
	}
}