using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class CursorItemSlotManager : MonoBehaviour {
	[Header("Properties")]
	[SerializeField] private int _width = 0;
	[SerializeField] private int _height = 0;
	[SerializeField] private Vector3 _offset = Vector3.zero;
	[SerializeField] private Vector2Int _focussedPosition = Vector2Int.zero;

	private List<ItemSlot> cursorItemSlots = new List<ItemSlot>( );

	#region Properties
	public int Width => _width;
	public int Height => _height;
	public Vector3 Offset {
		get => _offset;
		set => _offset = value;
	}
	public Vector2Int FocussedPosition {
		get => _focussedPosition;
		set => _focussedPosition = value;
	}
	public int CursorItemSlotsCount => cursorItemSlots.Count;
	#endregion

	#region Unity
	private void OnValidate ( ) {
		// Regenerate the list of cursor item slots
		// Also recalculate the width and height of the cursor grid
		// * The horizontal constraint should be set on the grid layout group
		cursorItemSlots.Clear( );
		cursorItemSlots = GetComponentsInChildren<ItemSlot>( ).ToList( );
		_width = GetComponent<GridLayoutGroup>( ).constraintCount;
		_height = cursorItemSlots.Count / Width;

		// Recalculate the position of the cursor item slot on the grid
		for (int i = 0; i < cursorItemSlots.Count; i++) {
			cursorItemSlots[i].Position = new Vector2Int(i % Width, i / Width);
		}
	}

	private void Update ( ) {
		Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition) + Offset;
		transform.position = new Vector3(mouseWorldPosition.x, mouseWorldPosition.y, transform.position.z);
	}
	#endregion

	public ItemSlot GetCursorItemSlot (Vector2Int position) {
		if (position.x < 0 || position.x >= Width || position.y < 0 || position.y >= Height) {
			return null;
		}

		return cursorItemSlots[position.x + (position.y * Width)];
	}

	public void SetCursorItemSlotItem (Vector2Int position, ItemScriptableObject item, Vector2Int spritePosition) {
		GetCursorItemSlot(position).SetItem(item, spritePosition);
	}
}
