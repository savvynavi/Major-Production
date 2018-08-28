using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class DragTarget : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IEndDragHandler
{
	bool hovering = false;

	//TODO add arguments

	protected virtual bool ConsumeHover(Draggable dragged)
	{
		return true;
	}

	protected abstract void OnHoverEnter(Draggable dragged);

	protected abstract void OnHoverLeave();

	public abstract bool Drop(Draggable dragged);

	public void OnPointerEnter(PointerEventData eventData)
	{
		// TODO check if draggable being dragged is under pointer, if so it's starting hover
		foreach(GameObject obj in Draggable.GetObjectsUnderMouse())
		{
			Draggable dragged = obj.GetComponent<Draggable>();
			if(obj != this.gameObject && dragged != null && dragged.dragging)
			{
				if (!hovering)
				{
					// maybe should also remember dragged object? Not sure how that case would happen though
					hovering = true;
					OnHoverEnter(dragged);
					// TODO figure out if this the drag? Or is the draggable being above enough?
					if (ConsumeHover(dragged))
					{
						eventData.Use();
					}
				}
				break;
			}
		}
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		if (hovering)
		{
			hovering = false;
			OnHoverLeave();
		}
	}

	public void OnEndDrag(PointerEventData eventData)
	{
		if (hovering)
		{
			hovering = false;
			OnHoverLeave();
		}
	}
}
