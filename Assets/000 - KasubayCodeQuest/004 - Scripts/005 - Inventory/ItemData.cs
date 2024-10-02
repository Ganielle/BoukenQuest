using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "InventoryItem", menuName = "Codemancer/Inventory/InventoryItem", order = 1)]
public class ItemData : ScriptableObject
{
    [field: SerializeField] public string ItemID { get; private set; }
    [field: SerializeField] public Sprite ItemSprite { get; private set; }
    [field: SerializeField] public bool IsUsable { get; private set; }
    [field: SerializeField] public string ItemName { get; private set; }
    [field: SerializeField] public float ItemPrice { get; private set; }
}
