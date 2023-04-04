using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using static UnityEditor.Progress;

public enum ItemType {
    NULL,
    TEST_RED,
    TEST_GREEN,
    TEST_BLUE,
    TEST_PURPLEPINK,
    TEST_YELLOW
}

public class InventoryManager : MonoBehaviour {
    [Header("Components")]
    [SerializeField] private Transform inventoryItemSlotTransform = null;
    [SerializeField] private Transform cursorItemSlotTransform = null;
    [SerializeField] private FollowMouse inventoryCursor = null;
    [Space]
    [SerializeField] private ItemTypeItemDictionary _items = new ItemTypeItemDictionary( );
    [Header("Properties")]
    [SerializeField] private ItemType _heldItemType;
    [SerializeField] private int _inventoryWidth = 0;
    [SerializeField] private int _inventoryHeight = 0;
    [SerializeField] private int _cursorWidth = 0;
    [SerializeField] private int _cursorHeight = 0;
    [SerializeField] private Vector2Int focussedPosition = Vector2Int.zero;

    private List<ItemSlot> inventoryItemSlots = new List<ItemSlot>( );
    private List<ItemSlot> cursorItemSlots = new List<ItemSlot>( );
    private List<List<ItemSlot>> inventoryItemSlotGroups = new List<List<ItemSlot>>( );

    #region Properties
    public int InventoryWidth => _inventoryWidth;
    public int InventoryHeight => _inventoryHeight;
    public int CursorWidth => _cursorWidth;
    public int CursorHeight => _cursorHeight;
    public bool IsHoldingItem => (HeldItemType != ItemType.NULL);
    public ItemType HeldItemType {
        get => _heldItemType;
        set => _heldItemType = value;
    }
    public ItemTypeItemDictionary Items => _items;
    #endregion

    #region Unity
    private void OnValidate ( ) {
        // Regenerate the list of item slots
        // Also recalculate the width and height of the inventory
        // * The horizontal constraint should be set on the grid layout group
        inventoryItemSlots.Clear( );
        inventoryItemSlotGroups.Clear( );
        inventoryItemSlots = inventoryItemSlotTransform.GetComponentsInChildren<ItemSlot>( ).ToList( );
        _inventoryWidth = inventoryItemSlotTransform.GetComponent<GridLayoutGroup>( ).constraintCount;
        _inventoryHeight = inventoryItemSlots.Count / InventoryWidth;

        // Also recalculate the grid position of the item slot
        for (int i = 0; i < inventoryItemSlots.Count; i++) {
            inventoryItemSlots[i].Position = new Vector2Int(i % InventoryWidth, i / InventoryWidth);
        }

        // Regenerate the list of cursor item slots
        // Also recalculate the width and height of the cursor grid
        // * The horizontal constraint should be set on the grid layout group
        cursorItemSlots.Clear( );
        cursorItemSlots = cursorItemSlotTransform.GetComponentsInChildren<ItemSlot>( ).ToList( );
        _cursorWidth = cursorItemSlotTransform.GetComponent<GridLayoutGroup>( ).constraintCount;
        _cursorHeight = cursorItemSlots.Count / CursorWidth;

        // Recalculate the position of the cursor item slot on the grid
        for (int i = 0; i < cursorItemSlots.Count; i++) {
            cursorItemSlots[i].Position = new Vector2Int(i % CursorWidth, i / CursorWidth);
        }
    }

    private void Awake ( ) {
        OnValidate( );
    }

    private void Start ( ) {
        // Add a bunch of items to the inventory to test it out
        InsertItem(ItemType.TEST_RED);
        InsertItem(ItemType.TEST_PURPLEPINK);
        InsertItem(ItemType.TEST_BLUE);
        InsertItem(ItemType.TEST_GREEN);
        InsertItem(ItemType.TEST_YELLOW);
    }
    #endregion

    /// <summary>
    /// Handle an interaction with an inventory item slot. This might pick up an item or place an item.
    /// </summary>
    /// <param name="inventoryItemSlot">The inventory item slot that was interacted with</param>
    /// <returns>Returns whether or not the actions performed inside this method were successful or not</returns>
    public bool InteractWithItemSlot (ItemSlot inventoryItemSlot) {
        // Determine which action is best based on whether or not the player is currently holding an item
        if (IsHoldingItem) {
            // If an item can be added to the area that will be taken up by the current held item, then place the item there
            if (AddItem(GetInventoryItemSlot(inventoryItemSlot.Position - focussedPosition), HeldItemType)) {
                // Clear the currently held item
                for (int i = 0; i < cursorItemSlots.Count; i++) {
                    Vector2Int position = new Vector2Int(i % CursorWidth, i / CursorWidth);
                    SetCursorItemSlotItem(position, ItemType.NULL, Vector2Int.zero);
                }
                HeldItemType = ItemType.NULL;

                // Adding the item was successful, so return true
                return true;
            }
        } else {
            // If an item can be removed from the inventory slot and can be held by the cursor item slots, then remove the item
            Vector2Int spritePosition = inventoryItemSlot.SpritePosition;
            if (RemoveItem(inventoryItemSlot, out ItemType itemType)) {
                // Get the scriptable object of the item type
                Item item = _items[itemType];

                // Align the cursor item slots
                ItemSlot topLeftItemSlot = GetInventoryItemSlot(inventoryItemSlot.Position - spritePosition);
                inventoryCursor.Offset = topLeftItemSlot.transform.position - (inventoryCursor.transform.position - inventoryCursor.Offset);
                focussedPosition = spritePosition;

                // Add the item to the cursor item slots
                for (int i = 0; i < cursorItemSlots.Count; i++) {
                    // Make sure that the cursor position will be effected by a new item being added
                    // If the position is out of the sprite bounds of the item, then do not set the cursor item to the new item
                    Vector2Int position = new Vector2Int(i % CursorWidth, i / CursorWidth);
                    if (position.x >= item.Size.x || position.y >= item.Size.y) {
                        continue;
                    }

                    SetCursorItemSlotItem(position, itemType, position);
                }
                HeldItemType = itemType;

                // Removing the item from the inventory was successful, so return true
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Add an item at a certain inventory item slot to the inventory
    /// </summary>
    /// <param name="itemSlot">The top-left corner of where the item should be placed</param>
    /// <param name="itemType">The item type to add</param>
    /// <returns>Returns whether or not the item was successfully added to the inventory</returns>
    public bool AddItem (ItemSlot itemSlot, ItemType itemType) {
        // Get the scriptable object of the item type
        Item item = _items[itemType];

        // Make sure that all item slots are free before trying to place the item
        // If they are not all free, then the item cannot be placed
        List<ItemSlot> effectedInventoryItemSlots = new List<ItemSlot>( );
        for (int i = 0; i < item.Size.x * item.Size.y; i++) {
            // Get an item slot that will be interacted with by the cursor
            Vector2Int spritePosition = new Vector2Int(i % item.Size.x, i / item.Size.x);
            ItemSlot inventoryItemSlot = GetInventoryItemSlot(itemSlot.Position + spritePosition);

            // If the inventory item slot currently has an item, then a new item cannot be placed here
            if (inventoryItemSlot == null || inventoryItemSlot.ItemType != ItemType.NULL) {
                return false;
            }

            effectedInventoryItemSlots.Add(inventoryItemSlot);
        }

        // Get an open group index
        int inventoryItemSlotGroupIndex = GetFirstOpenGroupIndex( );

        // Set the item on each of the effected inventory item slots
        for (int i = 0; i < effectedInventoryItemSlots.Count; i++) {
            // Set the item of the item slot
            Vector2Int spritePosition = new Vector2Int(i % item.Size.x, i / item.Size.x);
            SetInventoryItemSlotItem(effectedInventoryItemSlots[i].Position, itemType, spritePosition);

            // Add the grouped item slot to its new group
            // Also change the group index to match the array index
            effectedInventoryItemSlots[i].GroupIndex = inventoryItemSlotGroupIndex;
            inventoryItemSlotGroups[inventoryItemSlotGroupIndex].Add(effectedInventoryItemSlots[i]);
        }

        return true;
    }

    /// <summary>
    /// Insert an item into the inventory at the first avaiable item slot
    /// </summary>
    /// <param name="itemType">The item type to add</param>
    /// <returns>Returns whether or not the item was successfully added to the inventory</returns>
    public bool InsertItem (ItemType itemType) {
        // Loop through all of the inventory item slots and check to see if the item can be added to that location
        for (int i = 0; i < inventoryItemSlots.Count; i++) {
            // If the item can be added, then add it to inventory and return true
            if (AddItem(inventoryItemSlots[i], itemType)) {
                return true;
            }
        }

        // If all of the inventory slots have been checked and the item cannot fit anywhere, then return false
        return false;
    }

    /// <summary>
    /// Remove an item from a certain inventory item slot in the inventory
    /// </summary>
    /// <param name="itemSlot">The item slot to remove the item of</param>
    /// <param name="itemType">A reference to the item that was removed from the inventory</param>
    /// <returns>Returns whether or not the item was successfully removed</returns>
    public bool RemoveItem (ItemSlot itemSlot, out ItemType itemType) {
        // If the item slot does not have an item in it, then do not try and remove an item
        itemType = itemSlot.ItemType;
        if (itemType == ItemType.NULL) {
            return false;
        }

        // Get all of the item slots that are grouped together with the input item slot
        List<ItemSlot> itemSlotGroup = new List<ItemSlot>( ) { itemSlot };
        if (itemSlot.GroupIndex != -1) {
            itemSlotGroup = inventoryItemSlotGroups[itemSlot.GroupIndex];
        }

        // For all the item slots in the group, remove them from the board
        while (itemSlotGroup.Count > 0) {
            // Remove the item from the item slot
            SetInventoryItemSlotItem(itemSlotGroup[0].Position, ItemType.NULL, Vector2Int.zero);

            // Remove the item slot from the list of grouped item slots
            // Also reset the group id of the item slot as it is no longer part of a group
            itemSlotGroup[0].GroupIndex = -1;
            itemSlotGroup.RemoveAt(0);
        }

        return true;
    }

    /// <summary>
    /// Get an inventory item slot based on a grid position
    /// </summary>
    /// <param name="position">The grid position of the item slot trying to be found</param>
    /// <returns>Returns the item slot if one exists at the input position, null otherwise</returns>
    private ItemSlot GetInventoryItemSlot (Vector2Int position) {
        if (position.x < 0 || position.x >= InventoryWidth || position.y < 0 || position.y >= InventoryHeight) {
            return null;
        }

        return inventoryItemSlots[position.x + (position.y * InventoryWidth)];
    }

    /// <summary>
    /// Get a cursor item slot based on a grid position
    /// </summary>
    /// <param name="position">The grid position of the item slot trying to be found</param>
    /// <returns>Returns the item slot if one exists at the input position, null otherwise</returns>
    private ItemSlot GetCursorItemSlot (Vector2Int position) {
        if (position.x < 0 || position.x >= CursorWidth || position.y < 0 || position.y >= CursorHeight) {
            return null;
        }

        return cursorItemSlots[position.x + (position.y * CursorWidth)];
    }

    /// <summary>
    /// Find the first open item slot group index in the array
    /// </summary>
    /// <returns>Returns an integer that represents the index of the first empty group index</returns>
    private int GetFirstOpenGroupIndex ( ) {
        // Loop through all of the groups and find the first empty one
        for (int i = 0; i < inventoryItemSlotGroups.Count; i++) {
            if (inventoryItemSlotGroups[i].Count == 0) {
                return i;
            }
        }

        // If no groups are empty, create a new one
        inventoryItemSlotGroups.Add(new List<ItemSlot>( ));
        return inventoryItemSlotGroups.Count - 1;
    }

    /// <summary>
    /// Set the item type of an inventory item slot
    /// </summary>
    /// <param name="position">The position of the inventory item slot to set</param>
    /// <param name="itemType">The type of item to set to the inventory item slot</param>
    /// <param name="spritePosition">The section of the sprite that will show up in the inventory item slot</param>
    private void SetInventoryItemSlotItem (Vector2Int position, ItemType itemType, Vector2Int spritePosition) {
        GetInventoryItemSlot(position).SetItem(itemType, spritePosition);
    }

    /// <summary>
    /// Set the item type of an cursor item slot
    /// </summary>
    /// <param name="position">The position of the cursor item slot to set</param>
    /// <param name="itemType">The type of item to set to the cursor item slot</param>
    /// <param name="spritePosition">The section of the sprite that will show up in the cursor item slot</param>
    private void SetCursorItemSlotItem (Vector2Int position, ItemType itemType, Vector2Int spritePosition) {
        GetCursorItemSlot(position).SetItem(itemType, spritePosition);
    }
}
