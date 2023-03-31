using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour {
	[Header("Components")]
	[SerializeField] private Image itemImage = null;
	[SerializeField] private CursorItemSlot cursorItemSlot = null;
	[SerializeField] private Inventory inventory = null;
	[Header("Properties")]
	[SerializeField] private ItemScriptableObject item = null;

	#region Properties
	public ItemScriptableObject Item {
		get => item;
		set {
			if (value != null) {
				/*for (int x = 0; x < item.Size.x; x++) {
					for (int y = 0; y < item.Size.y; y++) {
						if (inventory[x, y].Item != null) {
							return;
						}
					}
				}*/

				itemImage.sprite = value.Sprite;
			}

			item = value;
			itemImage.enabled = (value != null);
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
		(itemSlot.Item, Item) = (Item, itemSlot.Item);
	}
}
