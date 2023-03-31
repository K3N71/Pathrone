using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CursorItemSlot : ItemSlot {
	private void Update ( ) {
		Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		transform.position = new Vector3(mouseWorldPosition.x, mouseWorldPosition.y, transform.position.z);
	}
}
