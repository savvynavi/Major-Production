using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class DragTarget : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
	bool hovering = false;

	//TODO add arguments

	public virtual bool ConsumeHover(Draggable dragged)
	{
		return true;
	}

	protected abstract void OnHoverEnter(Draggable dragged);

	protected abstract void OnHoverLeave();

	public abstract bool Drop(Draggable dragged);

	public void OnPointerEnter(PointerEventData eventData)
	{
		Debug.Log("pointer enters");
		// TODO check if draggable being dragged is under pointer, if so it's starting hover
		Draggable dragged = Draggable.CurrentDragged;
		if(dragged != null && dragged.gameObject != this.gameObject)
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
	}

	public void DragReleased()
	{
		if (hovering)
		{
			hovering = false;
			OnHoverLeave();
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
}
