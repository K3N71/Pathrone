using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "ScriptableObjects/Item")]
public class ItemScriptableObject : ScriptableObject {
	[SerializeField] private int id = -1;
	[SerializeField] private string name = "null";
	[SerializeField] private string description = "null";
	[SerializeField] private Sprite sprite = null;

	#region Properties
	public int ID => id;
	public string Name => name;
	public string Description => description;
	public Sprite Sprite => sprite;
	#endregion
}
