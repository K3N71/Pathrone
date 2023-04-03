using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour {
    [Header("Components")]
    [SerializeField] private InventoryManager inventoryManager = null;
    [Space]
    [SerializeField] private Image itemImage = null;
    [Header("Properties")]
    [SerializeField] private ItemType _itemType = ItemType.NULL;
    [SerializeField] private Vector2Int _position = Vector2Int.zero;
    [SerializeField] private Vector2Int _spritePosition = Vector2Int.zero;
    [SerializeField] private int _groupIndex = -1;

    #region Properties
    public Vector2Int Position {
        get => _position;
        set => _position = value;
    }
    public Vector2Int SpritePosition {
        get => _spritePosition;
        private set => _spritePosition = value;
    }
    public int GroupIndex {
        get => _groupIndex;
        set => _groupIndex = value;
    }
    public ItemType ItemType {
        get => _itemType;
        private set => _itemType = value;
    }
    #endregion

    private void OnValidate ( ) {
        inventoryManager = FindObjectOfType<InventoryManager>( );

        // Make sure the inventory starts with no items in it
        // For now, this makes it easier to deal with adding items to the inventory
        // In the future having functionality to set inventory items in the editor may be a good idea
        SetItem(ItemType.NULL, Vector2Int.zero);
    }

    public void SetItem (ItemType itemType, Vector2Int spritePosition) {
        ItemType = itemType;
        SpritePosition = spritePosition;

        itemImage.enabled = (itemType != ItemType.NULL);
        if (itemType != ItemType.NULL) {
            itemImage.sprite = inventoryManager.Items[itemType][SpritePosition.x, SpritePosition.y];
        }
    }

    private void OnMouseDown ( ) {
        inventoryManager.InteractWithItemSlot(this);
    }
}
