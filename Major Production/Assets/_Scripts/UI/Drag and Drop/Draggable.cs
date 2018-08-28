using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public abstract class Draggable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
	public Transform container;
	public Transform dragArea;
	public bool dragging { get; protected set; }
	protected Vector3 originalPos;

	public virtual void OnBeginDrag(PointerEventData eventData)
	{
		dragging = true;
		originalPos = transform.position;
		GetComponent<Image>().raycastTarget = false;
		transform.SetParent(dragArea);
		transform.SetAsLastSibling();   // Draw on top
		eventData.Use();
	}

	public virtual void OnDrag(PointerEventData eventData)
	{
		if (dragging)
		{
			transform.position = eventData.position;
		}
	}

	public virtual void OnEndDrag(PointerEventData eventData)
	{
		if (dragging)
		{
			dragging = false;
			foreach (DragTarget target in GetDragTargetsUnderMouse())
			{
				if (target.Drop(this))
				{
					break;
				}
			}

			transform.position = originalPos;
			GetComponent<Image>().raycastTarget = true;
			transform.SetParent(container.transform);
		}
	}

	public static List<DragTarget> GetDragTargetsUnderMouse()
	{
		List<DragTarget> targets = new List<DragTarget>();
		foreach(GameObject obj in GetObjectsUnderMouse())
		{
			DragTarget target = obj.GetComponent<DragTarget>();
			if(target != null)
			{
				targets.Add(target);
			}
		}
		return targets;
	}

	public static List<GameObject> GetObjectsUnderMouse()
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
