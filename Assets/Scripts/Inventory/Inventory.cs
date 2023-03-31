using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour {
	[Header("Components")]
	[SerializeField] private Transform itemSlotsTransform;
	[Header("Properties")]
	[SerializeField] private int width = 0;
	[SerializeField] private int height = 0;

	private List<ItemScriptableObject> items = new List<ItemScriptableObject>( );

	public void AddItem (ItemScriptableObject item) {
		
	}

	public void RemoveItem (ItemScriptableObject item) {
		
	}
}
