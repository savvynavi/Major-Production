using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace RPG.UI
{
	// Component allowing an element to be dragged and dropped onto drag targets
	[RequireComponent(typeof(CanvasGroup))]
	public class Draggable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
	{
		public Transform container;	// Original parent of draggable, will be moved back to it on release
		public Transform dragArea;	// Area in which it can be dragged
		public bool dragging { get; protected set; }
		protected Vector3 originalPos;

		public static Draggable CurrentDragged = null;  // The Draggable currently being dragged, or null if not dragging

		CanvasGroup cg;

		private void Awake()
		{
			cg = GetComponent<CanvasGroup>();
		}

		public virtual void OnBeginDrag(PointerEventData eventData)
		{
			dragging = true;
			CurrentDragged = this;
			originalPos = transform.position;
			cg.blocksRaycasts = false;
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
				CurrentDragged = null;
				bool dropped = false;
				foreach (DragTarget target in GetDragTargetsUnderMouse())
				{
					if (!dropped)
					{
						dropped = target.Drop(this);
					}
					target.DragReleased();
				}

				transform.position = originalPos;
				cg.blocksRaycasts = true;
				transform.SetParent(container.transform);
			}
		}

		public static List<DragTarget> GetDragTargetsUnderMouse()
		{
			List<DragTarget> targets = new List<DragTarget>();
			foreach (GameObject obj in GetObjectsUnderMouse())
			{
				DragTarget target = obj.GetComponent<DragTarget>();
				if (target != null)
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
}