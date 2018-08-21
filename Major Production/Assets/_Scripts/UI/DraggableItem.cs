using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DraggableItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
	public ItemBox container;
	public Transform dragArea;
	bool dragging;
	Vector3 originalPos;

	public void OnBeginDrag(PointerEventData eventData)
	{
		dragging = true;
		originalPos = transform.position;
		GetComponent<Image>().raycastTarget = false;
		transform.SetParent(dragArea);
	}

	public void OnDrag(PointerEventData eventData)
	{
		if (dragging)
		{
			transform.position = eventData.position;
		}
	}

	public void OnEndDrag(PointerEventData eventData)
	{
		if (dragging)
		{
			// TODO check for using
			transform.position = originalPos;
			transform.SetParent(container.transform);
		}
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
