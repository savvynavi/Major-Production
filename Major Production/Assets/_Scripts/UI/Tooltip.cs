using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Tooltip : MonoBehaviour {

	[SerializeField] Image tooltipBackground;
	[SerializeField] Text tooltipText;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		ITooltipTarget target = GetTooltipUnderMouse();
		if (target != null)
		{
			string text = target.TooltipText;
			if (string.IsNullOrEmpty(text))
			{
				tooltipBackground.gameObject.SetActive(false);
			}
			else
			{
				tooltipBackground.gameObject.SetActive(true);
				tooltipText.text = target.TooltipText;
				SetTooltipPosition();
			}
		}
		else
		{
			tooltipBackground.gameObject.SetActive(false);
		}
	}

	private void OnDisable()
	{
		tooltipBackground.gameObject.SetActive(false);
	}

	private void SetTooltipPosition()
	{
		// TODO set pivot based on collision with sides, topleft as default?
		// TODO move tooltip away from mouse so cursor doesn't cover it
		Vector3 mousePos = Input.mousePosition;
		tooltipBackground.rectTransform.position = mousePos;
		tooltipBackground.rectTransform.pivot = new Vector2(mousePos.x > Screen.width / 2 ? 1 : 0,
					mousePos.y > Screen.height / 2 ? 1 : 0);
	}

	ITooltipTarget GetTooltipUnderMouse()
	{
		ITooltipTarget result = null;
		foreach(GameObject go in Utility.GetObjectsUnderMouse())
		{
			result = go.GetComponent<ITooltipTarget>();
			if(result != null)
			{
				break;
			}
		}
		return result;
	}
}
