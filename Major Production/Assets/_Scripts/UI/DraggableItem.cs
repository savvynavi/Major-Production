using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

// TODO extract out to Draggable parent class
// TODO make a DropTarget (DragTarget?) class that reacts to Draggables being hovered over or dropped onto
public class DraggableItem : Draggable
{
	// TODO destroy this if container destroyed?
	public ItemBox itemBox;

	//public override void OnEndDrag(PointerEventData eventData)
	//{
	//	if (dragging)
	//	{
	//		dragging = false;
	//		bool used = false;
	//		List<GameObject> targets = GetObjectsUnderMouse();
	//		foreach (GameObject target in targets)
	//		{
	//			CharacterBox cb = target.GetComponent<CharacterBox>();
	//			if (cb)
	//			{
	//				// TODO check that character is able to use item
	//				GameController.Instance.inventory.Use(itemBox.ContainedItem, cb.ContainedCharacter);
	//				used = true;
	//				break;
	//			}
	//		}

	//		if (!used)
	//		{
	//			// If nothing else, return to original place
	//			transform.position = originalPos;
	//			GetComponent<Image>().raycastTarget = true;
	//			transform.SetParent(container.transform);
	//		}
	//	}
	//}
}
