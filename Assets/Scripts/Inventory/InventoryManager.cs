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
    [SerializeField] private Transform inventoryItemSlotTransform = null;
    [SerializeField] private Transform cursorItemSlotTransform = null;
    [SerializeField] private FollowMouse inventoryCursor = null;
    [Space]
    [SerializeField] private ItemScriptableObject testItem = null;
    [SerializeField] private ItemScriptableObject testItem2 = null;
    [Header("Properties")]
    [SerializeField] private ItemScriptableObject _heldItem;
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
    public bool IsHoldingItem => (HeldItem != null);
    public ItemScriptableObject HeldItem {
        get => _heldItem;
        set => _heldItem = value;
    }
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
        AddItem(GetItemSlot(Vector2Int.one), testItem);
        AddItem(GetItemSlot(Vector2Int.zero), testItem2);
    }
    #endregion

    public bool InteractWithItemSlot (ItemSlot inventoryItemSlot) {
        // Determine which action is best based on whether or not the player is currently holding an item
        if (IsHoldingItem) {
            // If an item can be added to the area that will be taken up by the current held item, then place the item there
            if (AddItem(GetItemSlot(inventoryItemSlot.Position - focussedPosition), HeldItem)) {
                // Clear the currently held item
                for (int i = 0; i < cursorItemSlots.Count; i++) {
                    Vector2Int position = new Vector2Int(i % CursorWidth, i / CursorWidth);
                    SetCursorItemSlotItem(position, null, Vector2Int.zero);
                }
                HeldItem = null;

                // Adding the item was successful, so return true
                return true;
            }
        } else {
            // If an item can be removed from the inventory slot and can be held by the cursor item slots, then remove the item
            if (RemoveItem(inventoryItemSlot, out ItemScriptableObject item)) {
                // Align the cursor item slots
                ItemSlot topLeftItemSlot = GetItemSlot(inventoryItemSlot.Position - inventoryItemSlot.SpritePosition);
                inventoryCursor.Offset = inventoryCursor.transform.position - topLeftItemSlot.transform.position;
                focussedPosition = inventoryItemSlot.SpritePosition;

                // Add the item to the cursor item slots
                for (int i = 0; i < cursorItemSlots.Count; i++) {
                    // Make sure that the cursor position will be effected by a new item being added
                    // If the position is out of the sprite bounds of the item, then do not set the cursor item to the new item
                    Vector2Int position = new Vector2Int(i % CursorWidth, i / CursorWidth);
                    if (position.x >= item.Size.x || position.y >= item.Size.y) {
                        continue;
                    }

                    SetCursorItemSlotItem(position, item, position);
                }
                HeldItem = item;

                // Removing the item from the inventory was successful, so return true
                return true;
            }
        }

        return false;
    }

    /* public bool HoldItem (ItemSlot inventoryItemSlot) {
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
     }*/

    /*public bool PlaceHeldItem (ItemSlot inventoryItemSlot) {
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
    }*/

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
        // If the item slot does not have an item in it, then do not try and remove an item
        item = inventoryItemSlot.Item;
        if (item == null) {
            return false;
        }

        // Get all of the item slots that are grouped together with the input item slot
        List<ItemSlot> itemSlotGroup = new List<ItemSlot>( ) { inventoryItemSlot };
        if (inventoryItemSlot.GroupIndex != -1) {
            itemSlotGroup = inventoryItemSlotGroups[inventoryItemSlot.GroupIndex];
            inventoryItemSlotGroups.RemoveAt(inventoryItemSlot.GroupIndex);
        }

        // For all the item slots in the group, remove them from the board
        while (itemSlotGroup.Count > 0) {
            // Remove the item from the item slot
            SetItemSlotItem(itemSlotGroup[0].Position, null, Vector2Int.zero);

            // Remove the item slot from the list of grouped item slots
            // Also reset the group id of the item slot as it is no longer part of a group
            itemSlotGroup[0].GroupIndex = -1;
            itemSlotGroup.RemoveAt(0);
        }

        return true;
    }

    /* private List<ItemSlot> GetGroupedItemSlots (ItemSlot itemSlot) {
         List<ItemSlot> groupedItemSlots = new List<ItemSlot>( ) { itemSlot };
         if (itemSlot.GroupIndex != -1) {
             groupedItemSlots = inventoryItemSlotGroups[itemSlot.GroupIndex];
             inventoryItemSlotGroups.RemoveAt(itemSlot.GroupIndex);
         }

         return groupedItemSlots;
     }*/

    public ItemSlot GetItemSlot (Vector2Int position) {
        if (position.x < 0 || position.x >= InventoryWidth || position.y < 0 || position.y >= InventoryHeight) {
            return null;
        }

        return inventoryItemSlots[position.x + (position.y * InventoryWidth)];
    }

    public ItemSlot GetCursorItemSlot (Vector2Int position) {
        if (position.x < 0 || position.x >= CursorWidth || position.y < 0 || position.y >= CursorHeight) {
            return null;
        }

        return cursorItemSlots[position.x + (position.y * CursorWidth)];
    }

    public void SetItemSlotItem (Vector2Int position, ItemScriptableObject item, Vector2Int spritePosition) {
        GetItemSlot(position).SetItem(item, spritePosition);
    }

    public void SetCursorItemSlotItem (Vector2Int position, ItemScriptableObject item, Vector2Int spritePosition) {
        GetCursorItemSlot(position).SetItem(item, spritePosition);
    }

    /*private void ClearCursorItemSlots ( ) {
        for (int i = 0; i < cursorItemSlots.Count; i++) {
            Vector2Int position = new Vector2Int(i % CursorWidth, i / CursorWidth);
            SetCursorItemSlotItem(position, null, Vector2Int.zero);
        }
    }*/
}
