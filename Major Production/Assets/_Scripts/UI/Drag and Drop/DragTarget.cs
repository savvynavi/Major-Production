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
	public bool Hover(Draggable dragged)
	{
		if (!hovering)
		{
			OnHoverEnter(dragged);
		}
		hovering = true;
		return ConsumeHover(dragged);
	}

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
		throw new NotImplementedException();
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
