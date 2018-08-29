using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;

namespace RPG.UI
{
	// Base class which can have draggables hovered over or dropped onto it
	public abstract class DragTarget : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
	{
		bool hovering = false;

		//TODO add arguments

		// maybe doesn't work now pointer enter used?
		public virtual bool ConsumeHover(Draggable dragged)
		{
			return true;
		}

		protected abstract void OnHoverEnter(Draggable dragged);

		protected abstract void OnHoverLeave();

		public abstract bool Drop(Draggable dragged);

		public void OnPointerEnter(PointerEventData eventData)
		{
			Draggable dragged = Draggable.CurrentDragged;
			if (dragged != null && dragged.gameObject != this.gameObject)
			{
				// maybe should also remember dragged object? Not sure how that case would happen though
				hovering = true;
				OnHoverEnter(dragged);
				if (ConsumeHover(dragged))
				{
					eventData.Use();
					// TODO figure out if this works or is needed
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
}