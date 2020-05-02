using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory/Player Inventory/PlayerInventory")]
public class PlayerInventory : ScriptableObject
{
     public List<InventoryItem> inventoryItems = new List<InventoryItem>();
}
