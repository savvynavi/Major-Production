using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public abstract class Draggable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
	public Transform container;
	public Transform dragArea;
	protected bool dragging;
	protected Vector3 originalPos;

	public virtual void OnBeginDrag(PointerEventData eventData)
	{
		dragging = true;
		originalPos = transform.position;
		GetComponent<Image>().raycastTarget = false;
		transform.SetParent(dragArea);
		transform.SetAsLastSibling();   // Draw on top
	}

	public virtual void OnDrag(PointerEventData eventData)
	{
		if (dragging)
		{
			transform.position = eventData.position;
		}
		// TODO check for dragtarget hovered over
	}

	public abstract void OnEndDrag(PointerEventData eventData);	// TODO implement, make it check for dragtargets

	public List<GameObject> GetObjectsUnderMouse()
	{
		List<RaycastResult> hitObjects = new List<RaycastResult>();
		PointerEventData pointer = new PointerEventData(EventSystem.current);
		pointer.position = Input.mousePosition;
		EventSystem.current.RaycastAll(pointer, hitObjects);
		List<GameObject> objects = new List<GameObject>();
		foreach (RaycastResult result in hitObjects)
		{
			objects.Add(result.gameObject);
		}
		return objects;
	}
}
