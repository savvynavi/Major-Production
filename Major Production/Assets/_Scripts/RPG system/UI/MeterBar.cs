using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI
{
	[RequireComponent(typeof(Image))]
	public class MeterBar : MonoBehaviour, ITooltipTarget
	{

		public float maxValue;
		public float currentValue;
		[SerializeField] string valueType = "HP";
		public Image Bar;

		public string TooltipText
		{
			get
			{
				return string.Format("{0:N0}/{1:N0} {2}", currentValue, maxValue, valueType);
			}
		}

		private void Awake()
		{
			Bar = GetComponent<Image>();
		}

		public void setValues(float current, float max)
		{
			currentValue = current;
			maxValue = max;
		}

		public void Init(float current, float max)
		{
			currentValue = current;
			maxValue = max;
			Bar.fillAmount = current / max;
		}

		private void Update()
		{
			float targetFill = currentValue / maxValue;
			Bar.fillAmount = Mathf.MoveTowards(Bar.fillAmount, targetFill, Time.unscaledDeltaTime);	// Using unscaled while pausing stops time
			// maybe some weightier effect
		}
	}
}