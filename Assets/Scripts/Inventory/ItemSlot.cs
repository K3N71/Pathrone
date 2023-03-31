using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour {
	[Header("Components")]
	[SerializeField] private Image itemImage = null;
	[SerializeField] private CursorItemSlot cursorItemSlot = null;
	[Header("Properties")]
	[SerializeField] private ItemScriptableObject item = null;

	#region Properties
	public ItemScriptableObject Item {
		get => item;
		set {
			item = value;

			itemImage.enabled = (item != null);

			if (item != null) {
				itemImage.sprite = item.Sprite;
			}
		}
	}
	#endregion

	protected void OnValidate ( ) {
		Item = Item;
	}

	protected void OnMouseDown ( ) {
		SwapItem(cursorItemSlot);
	}

	private void SwapItem (ItemSlot itemSlot) {
		ItemScriptableObject currentItem = Item;
		Item = itemSlot.Item;
		itemSlot.Item = currentItem;
	}
}
