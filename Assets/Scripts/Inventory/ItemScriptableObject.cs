using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "ScriptableObjects/Item")]
public class ItemScriptableObject : ScriptableObject {
	[SerializeField] private int id = -1;
	[SerializeField] private Vector2Int size = Vector2Int.one;
	[SerializeField] private string name = "null";
	[SerializeField] private string description = "null";
	[SerializeField] private StringSpriteDictionary sprites = new StringSpriteDictionary( );

	#region Properties
	public int ID => id;
	public Vector2Int Size => size;
	public string Name => name;
	public string Description => description;

	public Sprite Sprite => this[0, 0];
	public Sprite this[int x, int y] {
		get => sprites[$"{x}{y}"];
	}
	#endregion

	private void OnValidate ( ) {
		if (Size.x * Size.y != sprites.Count) {
			sprites.Clear( );

			for (int x = 0; x < Size.x; x++) {
				for (int y = 0; y < Size.y; y++) {
					string key = $"{x}{y}";

					if (sprites.ContainsKey(key)) {
						continue;
					}

					sprites.Add(key, null);
				}
			}
		}
	}
}
