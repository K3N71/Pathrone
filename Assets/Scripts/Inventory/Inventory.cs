using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour {
	[Header("Components")]
	[SerializeField] private Transform itemSlotsTransform;
	[Header("Properties")]
	[SerializeField] private int width = 0;
	[SerializeField] private int height = 0;

	// private List<ItemScriptableObject> items = new List<ItemScriptableObject>( );
	private List<ItemSlot> itemSlots = new List<ItemSlot>( );

	#region Properties
	public ItemSlot this[ int x, int y] {
		get => itemSlots[x + (y * width)];
	}
	#endregion

	private void OnValidate ( ) {
		itemSlots.Clear( );
		itemSlots = itemSlotsTransform.GetComponentsInChildren<ItemSlot>( ).ToList( );
	}

	public void AddItem (ItemScriptableObject item) {

	}

	public void RemoveItem (ItemScriptableObject item) {

	}
}
