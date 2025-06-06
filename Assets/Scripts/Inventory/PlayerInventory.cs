﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerInventory: MonoBehaviour
{
     public List<InventoryItem> inventoryItems = new List<InventoryItem>();
     public Dictionary<InventoryItem.Slot, InventoryItem> equipedItems = new Dictionary<InventoryItem.Slot, InventoryItem>();


    public InventoryItem EquipItem(InventoryItem item)
    {

        switch (item.slot)
        {
            case InventoryItem.Slot.head:
                InventoryItem equipedHead;
                equipedItems.TryGetValue(item.slot, out equipedHead);
                equipedItems[InventoryItem.Slot.head] = item;
                if (equipedHead != null)
                {
                    return equipedHead;
                }

                break;
            case InventoryItem.Slot.chest:
                InventoryItem equipedChest;
                equipedItems.TryGetValue(item.slot, out equipedChest);
                equipedItems[InventoryItem.Slot.chest] = item;
                if (equipedChest != null)
                {
                    return equipedChest;

                }
                break;
            case InventoryItem.Slot.weapon:

                InventoryItem equipedWeapon;
                equipedItems.TryGetValue(item.slot, out equipedWeapon);
                equipedItems[InventoryItem.Slot.weapon] = item;
                if (equipedWeapon != null)
                {
                    return equipedWeapon;

                }
                break;
            case InventoryItem.Slot.gloves:
                InventoryItem equipedGloves;
                equipedItems.TryGetValue(item.slot, out equipedGloves);
                equipedItems[InventoryItem.Slot.gloves] = item;
                if (equipedGloves != null)
                {
                    return equipedGloves;

                }
                break;
            case InventoryItem.Slot.leggings:
                InventoryItem equippedLeggings;
                equipedItems.TryGetValue(item.slot, out equippedLeggings);
                equipedItems[InventoryItem.Slot.leggings] = item;
                if (equippedLeggings != null)
                {
                    return equippedLeggings;
                }
                break;
            case InventoryItem.Slot.boots:
                InventoryItem equippedBoots;
                equipedItems.TryGetValue(item.slot, out equippedBoots);
                equipedItems[InventoryItem.Slot.boots] = item;
                if (equippedBoots != null)
                {
                    return equippedBoots;
                }
                break;
            default:
                break;
        }
        return null;

    }
}
