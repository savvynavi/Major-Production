using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace RPG.UI
{
	public interface ITooltipTarget
	{
		string TooltipText { get; }
	}
}