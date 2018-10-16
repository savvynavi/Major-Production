using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPGItems;

namespace RPG.UI{
	public class InventoryPanel : DragTarget {
		#region Drag and Drop
		public override bool Drop(Draggable dragged)
		{
			// Move item to end of inventory
			// TODO when this knows about contents, instead figure out index based on position?
			InventoryManager inventory = GameController.Instance.inventory;
			DraggableItem draggedItem = (DraggableItem)dragged;
			int index = inventory.playerInventory.Count;
			if (draggedItem != null && draggedItem.itemBox != this)
			{
				Item item = draggedItem.itemBox.ContainedItem;
				if (draggedItem.itemBox is InventorySlot)
				{
					return inventory.MoveItem(item, index);
				} else if (draggedItem.itemBox is EquipmentSlotUI)
				{
					EquipmentSlotUI slot = (EquipmentSlotUI)draggedItem.itemBox;
					item = slot.equipmentSlot.Unequip();
					if (item != null)
					{
						inventory.Add(item, index);
						return true;
					}
				}
			}
				return false;
		}

		protected override void OnHoverEnter(Draggable dragged)
		{
			//HACK checking if in character screen
			CharacterScreen screen = GetComponentInParent<CharacterScreen>();
			if (screen != null)
			{
				if (dragged is DraggableItem)
				{
					DraggableItem di = (DraggableItem)dragged;
					if (di.itemBox is EquipmentSlotUI)
					{
						StatDisplay.StatChangeData statChangeData = new StatDisplay.StatChangeData();
						statChangeData.ItemToUse = di.itemBox.ContainedItem;
						statChangeData.ApplyItemEffects(di.itemBox.ContainedItem, true);
						screen.DisplayCharacter(statChangeData);
					}
				}
			}
		}

		protected override void OnHoverLeave()
		{
			// reset character display if in character screen
			CharacterScreen screen = GetComponentInParent<CharacterScreen>();
			if (screen != null)
			{
				screen.DisplayCharacter();
			}
		}
		#endregion
		// TODO move logic for populating panel into here
		[SerializeField] GameObject ItemBoxPrefab;
		[SerializeField] RectTransform itemContainer;
		public Transform dragArea;

		// Use this for initialization
		void Start () {
		
		}
	
		// Update is called once per frame
		void Update () {
		
		}

		public void UpdateItems()
		{
			foreach(Transform child in itemContainer.transform)
			{
				GameObject.Destroy(child.gameObject);
			}
			foreach(Item item in GameController.Instance.inventory.playerInventory)
			{
				GameObject obj = Instantiate(ItemBoxPrefab, itemContainer);
				InventorySlot box = obj.GetComponent<InventorySlot>();
				box.ContainedItem = item;
				box.draggable.dragArea = dragArea;
			}
		}
	}
}