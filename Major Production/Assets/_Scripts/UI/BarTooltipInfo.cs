using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarTooltipInfo : MonoBehaviour, ITooltipTarget {

	public float maxValue;
	public float currentValue;
	[SerializeField] string valueType = "HP";

	public string TooltipText
	{
		get
		{
			return string.Format("{0:N0}/{1:N0} {2}", currentValue, maxValue, valueType);
		}
	}

	public void setValues(float current, float max)
	{
		currentValue = current;
		maxValue = max;
	}
}
