using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class InventoryManager : MonoBehaviour {
	[Header("Components")]
	[SerializeField] private CursorItemSlotManager cursorItemSlotManager = null;
	[Space]
	[SerializeField] private ItemScriptableObject testItem = null;
	[Header("Properties")]
	[SerializeField] private int _width = 0;
	[SerializeField] private int _height = 0;
	[SerializeField] private ItemScriptableObject _heldItem;

	private List<ItemSlot> inventoryItemSlots = new List<ItemSlot>( );
	private List<List<ItemSlot>> inventoryItemSlotGroups = new List<List<ItemSlot>>( );

	#region Properties
	public int Width => _width;
	public int Height => _height;
	public ItemScriptableObject HeldItem {
		get => _heldItem;
		set => _heldItem = value;
	}
	public bool IsHoldingItem => (HeldItem != null);
	#endregion

	#region Unity
	private void OnValidate ( ) {
		// Regenerate the list of item slots
		// Also recalculate the width and height of the inventory
		// * The horizontal constraint should be set on the grid layout group
		inventoryItemSlots.Clear( );
		inventoryItemSlotGroups.Clear( );
		inventoryItemSlots = GetComponentsInChildren<ItemSlot>( ).ToList( );
		_width = GetComponent<GridLayoutGroup>( ).constraintCount;
		_height = inventoryItemSlots.Count / Width;

		// Also recalculate the grid position of the item slot
		for (int i = 0; i < inventoryItemSlots.Count; i++) {
			inventoryItemSlots[i].Position = new Vector2Int(i % Width, i / Width);
		}
	}

	private void Awake ( ) {
		OnValidate( );
	}

	private void Start ( ) {
		AddItem(GetItemSlot(Vector2Int.one), testItem);
	}
	#endregion

	public bool InteractWithItemSlot (ItemSlot inventoryItemSlot) {
		// Determine which action is best based on whether or not the player is currently holding an item
		if (IsHoldingItem) {
			if (!AddItem(GetItemSlot(inventoryItemSlot.Position - cursorItemSlotManager.FocussedPosition), HeldItem)) {
				return false;
			}

			ClearCursorItemSlots( );
			HeldItem = null;

			return true;
			// return PlaceHeldItem(inventoryItemSlot);
		} else {
			return HoldItem(inventoryItemSlot);
		}
	}

	public bool HoldItem (ItemSlot inventoryItemSlot) {
		// If the item slot does not have an item in it, then do not try and pick that item
		if (inventoryItemSlot.Item == null) {
			return false;
		}

		// Align the cursor item slots to be able to fit the item in the inventory
		ItemSlot topLeftItemSlot = GetItemSlot(inventoryItemSlot.Position - inventoryItemSlot.SpritePosition);
		cursorItemSlotManager.Offset = cursorItemSlotManager.transform.position - topLeftItemSlot.transform.position;
		cursorItemSlotManager.FocussedPosition = inventoryItemSlot.SpritePosition;

		// Get all of the item slots that are grouped together with the parent item slot
		List<ItemSlot> groupedItemSlots = GetGroupedItemSlots(inventoryItemSlot);

		// For all the item slots in the group, transfer their item to the corresponding cursor item slot
		while (groupedItemSlots.Count > 0) {
			// Set the corresponding cursor item slot sprite position and item
			cursorItemSlotManager.SetCursorItemSlotItem(groupedItemSlots[0].SpritePosition, inventoryItemSlot.Item, groupedItemSlots[0].SpritePosition);
			SetItemSlotItem(groupedItemSlots[0].Position, null, Vector2Int.zero);

			// Remove the item slot from the list of grouped item slots
			// Also reset the group id of the item slot as it is no longer part of a group
			groupedItemSlots[0].GroupIndex = -1;
			groupedItemSlots.RemoveAt(0);
		}

		HeldItem = inventoryItemSlot.Item;
		return true;
	}

	public bool PlaceHeldItem (ItemSlot inventoryItemSlot) {
		// Make sure that all item slots are free before trying to place the item
		// If they are not all free, then the item cannot be placed
		List<ItemSlot> affectedItemSlots = new List<ItemSlot>( );
		for (int i = 0; i < cursorItemSlotManager.CursorItemSlotsCount; i++) {
			// Get an item slot that will be interacted with by the cursor
			ItemSlot cursorItemSlot = cursorItemSlotManager.GetCursorItemSlot(new Vector2Int(i % cursorItemSlotManager.Width, i / cursorItemSlotManager.Width));
			ItemSlot currentInventoryItemSlot = GetItemSlot((inventoryItemSlot.Position - cursorItemSlotManager.FocussedPosition) + cursorItemSlot.Position);

			// If the cursor item slot has no item, then continue to the next item slot
			// Not matter what, the corresponding inventory item slot will not be affected
			if (cursorItemSlot.Item == null) {
				continue;
			}

			// If the inventory item slot currently has an item, then a new item cannot be placed here
			if (currentInventoryItemSlot == null || currentInventoryItemSlot.Item != null) {
				return false;
			}

			affectedItemSlots.Add(currentInventoryItemSlot);
		}

		// Add a new index to the item slot groups
		inventoryItemSlotGroups.Add(new List<ItemSlot>( ));

		for (int i = 0; i < affectedItemSlots.Count; i++) {
			// Set the item of the item slot
			Vector2Int cursorItemSlotPosition = new Vector2Int(i % HeldItem.Size.x, i / HeldItem.Size.x);
			SetItemSlotItem(affectedItemSlots[i].Position, HeldItem, cursorItemSlotPosition);
			cursorItemSlotManager.SetCursorItemSlotItem(cursorItemSlotPosition, null, Vector2Int.zero);

			// Add the grouped item slot to its new group
			// Also change the group index to match the array index
			affectedItemSlots[i].GroupIndex = inventoryItemSlotGroups.Count - 1;
			inventoryItemSlotGroups[^1].Add(affectedItemSlots[i]);
		}

		HeldItem = null;
		return true;
	}

	public bool AddItem (ItemSlot topLeftItemSlot, ItemScriptableObject item) {
		// Make sure that all item slots are free before trying to place the item
		// If they are not all free, then the item cannot be placed
		List<ItemSlot> affectedItemSlots = new List<ItemSlot>( );
		for (int i = 0; i < item.Size.x * item.Size.y; i++) {
			// Get an item slot that will be interacted with by the cursor
			Vector2Int spritePosition = new Vector2Int(i % item.Size.x, i / item.Size.x);
			ItemSlot inventoryItemSlot = GetItemSlot(topLeftItemSlot.Position + spritePosition);

			// If the inventory item slot currently has an item, then a new item cannot be placed here
			if (inventoryItemSlot == null || inventoryItemSlot.Item != null) {
				return false;
			}

			affectedItemSlots.Add(inventoryItemSlot);
		}

		// Add a new index to the item slot groups
		inventoryItemSlotGroups.Add(new List<ItemSlot>( ));

		for (int i = 0; i < affectedItemSlots.Count; i++) {
			// Set the item of the item slot
			Vector2Int spritePosition = new Vector2Int(i % item.Size.x, i / item.Size.x);
			SetItemSlotItem(affectedItemSlots[i].Position, item, spritePosition);

			// Add the grouped item slot to its new group
			// Also change the group index to match the array index
			affectedItemSlots[i].GroupIndex = inventoryItemSlotGroups.Count - 1;
			inventoryItemSlotGroups[^1].Add(affectedItemSlots[i]);
		}

		return true;
	}

	public bool RemoveItem (ItemSlot inventoryItemSlot, out ItemScriptableObject item) {
		item = null;



		return true;
	}

	private List<ItemSlot> GetGroupedItemSlots (ItemSlot itemSlot) {
		List<ItemSlot> groupedItemSlots = new List<ItemSlot>( ) { itemSlot };
		if (itemSlot.GroupIndex != -1) {
			groupedItemSlots = inventoryItemSlotGroups[itemSlot.GroupIndex];
			inventoryItemSlotGroups.RemoveAt(itemSlot.GroupIndex);
		}

		return groupedItemSlots;
	}

	public ItemSlot GetItemSlot (Vector2Int position) {
		if (position.x < 0 || position.x >= Width || position.y < 0 || position.y >= Height) {
			return null;
		}

		return inventoryItemSlots[position.x + (position.y * Width)];
	}

	public void SetItemSlotItem (Vector2Int position, ItemScriptableObject item, Vector2Int spritePosition) {
		GetItemSlot(position).SetItem(item, spritePosition);
	}

	private void ClearCursorItemSlots ( ) {
		for (int i = 0; i < cursorItemSlotManager.CursorItemSlotsCount; i++) {
			Vector2Int position = new Vector2Int(i % cursorItemSlotManager.Width, i / cursorItemSlotManager.Width);
			cursorItemSlotManager.SetCursorItemSlotItem(position, null, Vector2Int.zero);
		}
	}
}
