using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.UI
{
	public class InventoryScreen : MenuScreen
	{

		public InventoryPanel inventoryPanel;
		public RectTransform characterPanel;
		[SerializeField] GameObject CharacterBoxPrefab;
		// TODO figure out where this belongs, how it gets opened and closed

		// Use this for initialization
		void Start()
		{
			inventoryPanel.dragArea = this.transform;
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
			inventoryPanel.UpdateItems();
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