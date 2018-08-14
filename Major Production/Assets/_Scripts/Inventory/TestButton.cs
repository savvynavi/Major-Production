using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace RPGItems {
	public class TestButton : MonoBehaviour {
		InventoryManager manager;
		Button btn;
		public Item item;
		public RPGsys.Character chara;
		public bool AddItemToPlayer;

		private void Start() {
			manager = GameObject.Find("Battle Manager").GetComponent<InventoryManager>();
			btn = GetComponent<Button>();

			if(AddItemToPlayer == true) {
				btn.onClick.AddListener(() => OnClickAdd());
			} else {
				btn.onClick.AddListener(() => OnClickRemove());
			}

		}

		public void OnClickAdd() {
			manager.Use(item, chara);
			Debug.Log("button clicked!");
		}

		public void OnClickRemove() {
			manager.Unequip(item, chara);
			Debug.Log("button clicked!");
		}
	}
}

