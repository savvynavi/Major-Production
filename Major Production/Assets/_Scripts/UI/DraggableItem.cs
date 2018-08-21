using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

// TODO extract out to Draggable parent class
// TODO make a DropTarget (DragTarget?) class that reacts to Draggables being hovered over or dropped onto
public class DraggableItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
	// TODO destroy this if container destroyed?
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
		transform.SetAsLastSibling();	// Draw on top
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
			dragging = false;
			bool used = false;
			List<GameObject> targets = GetObjectsUnderMouse();
			foreach (GameObject target in targets)
			{
				CharacterBox cb = target.GetComponent<CharacterBox>();
				if (cb)
				{
					// TODO check that character is able to use item
					GameController.Instance.inventory.Use(container.ContainedItem, cb.ContainedCharacter);
					used = true;
					break;
				}
			}

			if (!used)
			{
				// If nothing else, return to original place
				transform.position = originalPos;
				GetComponent<Image>().raycastTarget = true;
				transform.SetParent(container.transform);
			}
		}
	}

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

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
