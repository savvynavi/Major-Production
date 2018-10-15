using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.UI
{
	public class InventoryScreen : MenuScreen
	{

		public RectTransform itemPanel;
		public RectTransform characterPanel;
		[SerializeField] GameObject ItemBoxPrefab;
		[SerializeField] GameObject CharacterBoxPrefab;
		// TODO figure out where this belongs, how it gets opened and closed

		// Use this for initialization
		void Start()
		{
		}

		// Update is called once per frame
		void Update()
		{

		}

		public override void Open()
		{
			gameObject.SetActive(true);
			GameController.Instance.inventory.OnInventoryChanged.AddListener(UpdateItems);
			UpdateContents();
		}

		public override void Close()
		{
			GameController.Instance.inventory.OnInventoryChanged.RemoveListener(UpdateItems);
			gameObject.SetActive(false);
		}

		public void UpdateContents()
		{
			UpdateItems();
			UpdateCharacters();
		}

		public void UpdateItems()
		{
			foreach (Transform child in itemPanel)
			{
				GameObject.Destroy(child.gameObject);
			}
			foreach (RPGItems.Item item in GameController.Instance.inventory.playerInventory)
			{
				GameObject obj = Instantiate(ItemBoxPrefab, itemPanel);
				InventorySlot box = obj.GetComponent<InventorySlot>();
				box.ContainedItem = item;
				box.draggable.dragArea = this.transform;
			}
		}

		public void UpdateCharacters()
		{
			foreach (Transform child in characterPanel)
			{
				GameObject.Destroy(child.gameObject);
			}
			foreach (RPGsys.Character c in GameController.Instance.Characters)
			{
				GameObject obj = Instantiate(CharacterBoxPrefab, characterPanel);
				obj.GetComponent<CharacterBox>().ContainedCharacter = c;
			}
		}

		public void UnequipAllItemsFromAll()
		{
			foreach (RPGsys.Character character in GameController.Instance.Characters)
			{
				character.UnequipAll();
			}
		}
	}
}