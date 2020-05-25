using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveGameService : MonoBehaviour
{
    // Start is called before the first frame update




    public SaveGameComponents GetSaveData()
    {
        float xPosition;
        float yPosition;
        int cene;
        List<InventoryItem> inventory;
        List<InventoryItem> privateChest;
        int gold;
        int experience;

        return new SaveGameComponents(0, 0, 1, InventoryManager.instance.playerInventory.inventoryItems, InventoryManager.instance.privateChestInventory.inventoryItems, 100, 100);
    }
}
