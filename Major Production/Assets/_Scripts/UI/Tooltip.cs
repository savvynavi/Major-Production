using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Tooltip : MonoBehaviour {

	static int mousePadding = 15;
	[SerializeField] Image tooltipBackground;
	[SerializeField] Text tooltipText;
	LayoutGroup layoutGroup;

	// Use this for initialization
	void Start () {
		layoutGroup = tooltipBackground.GetComponent<LayoutGroup>();
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
				LayoutRebuilder.MarkLayoutForRebuild(tooltipBackground.rectTransform);
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
		
		Vector2 mousePos = Input.mousePosition;
		Vector2 tipSize = tooltipBackground.rectTransform.rect.size * tooltipBackground.canvas.scaleFactor;
		Vector2 tipExpectedBound = mousePos + tipSize;	
		Vector2 pivot = new Vector2(0,1);
		if (mousePos.x > Screen.width / 2 && tipExpectedBound.x > Screen.width)
		{
			pivot.x = 1;
		}
		if(mousePos.y > Screen.height / 2 && tipExpectedBound.y > Screen.height)
		{
			pivot.y = 0;
		}

		layoutGroup.padding.left = (int)(mousePadding / tooltipBackground.canvas.scaleFactor);

		tooltipBackground.rectTransform.pivot = pivot;
		tooltipBackground.rectTransform.position = mousePos;
	}

	ITooltipTarget GetTooltipUnderMouse()
	{
		ITooltipTarget result = null;
		List<GameObject> hits = Utility.GetObjectsUnderMouse();
		//if(hits.Count > 0)
		//{
		//	result = hits[0].GetComponent<ITooltipTarget>();
		//}
		foreach (GameObject go in hits)
		{
			result = go.GetComponent<ITooltipTarget>();
			if (result != null)
			{
				break;
			}
		}
		return result;
	}
}
