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
    [SerializeField] private int x = 0;
    [SerializeField] private int y = 0;
    [SerializeField] private List<ItemSlot> linkedItemSlots = new List<ItemSlot>( );

    #region Properties
    public int X => x;
    public int Y => y;
    public ItemScriptableObject Item {
        get => item;
        set {
            item = value;
            itemImage.enabled = (value != null);

            if (value != null) {
                itemImage.sprite = value.Sprite;
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
        (itemSlot.Item, Item) = (Item, itemSlot.Item);
    }

    private bool SetItem (ItemScriptableObject item, int x, int y) {
        // A list of the item slots that will be effected by the item being added
        List<ItemSlot> itemSlots = new List<ItemSlot>( );

        // Loop through and check to make sure that all item slots the current held item will cover are free
        for (int i = 0; i < item.Size.x; i++) {
            for (int j = 0; j < item.Size.y; j++) {
                // If the item slot is not null, then the slot already has an item in it and cannot be set
                if (inventory[x + i, y + j].Item != null) {
                    return false;
                }

                // If the item slot is free, then add it to the list of item slots
                itemSlots.Add(inventory[x + i, y + j]);
            }
        }

        for (int i = 0; i < item.Size.x * item.Size.y; i++) {
            itemSlots[i].Item = Item;
        }

        return true;
    }
}
