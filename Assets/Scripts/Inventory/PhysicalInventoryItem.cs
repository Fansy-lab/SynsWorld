using System.Collections;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class PhysicalInventoryItem : MonoBehaviour
{
    [SerializeField] PlayerInventory playerInventory;
    [SerializeField] InventoryItem thisItem;
    

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name == "Player")
        {
            AddItemToInventory();
            Destroy(gameObject);

        }
    }

    
    void AddItemToInventory()
    {
        if (playerInventory && thisItem)
        {

            if (thisItem.equipable)
            {
                InventoryItem inventoryItemInstance = ScriptableObject.CreateInstance("InventoryItem") as InventoryItem;
                inventoryItemInstance.Init(new EquipableArmoryStats() { ArmorAmmount = Random.Range(1, 6), HealthAmmount = Random.Range(5, 10) },
                    null, null, "SimpleArmor", "Simple armor", thisItem.itemImage, 1, false, true, false);


                playerInventory.inventoryItems.Add(inventoryItemInstance);
            }
            else if (thisItem.usable)
            {
                //check if player has that consumable
                bool hasConsumable = false;
                foreach (var item in playerInventory.inventoryItems)
                {
                    if (item.itemName == thisItem.itemName)
                    {
                        hasConsumable = true;
                        item.numberHeld++;
                    }
                }
                if(hasConsumable ==false)
                {
                    InventoryItem inventoryItemInstance = ScriptableObject.CreateInstance("InventoryItem") as InventoryItem;
                    inventoryItemInstance.Init(null, null, new UsableStats() { HPRestoreAmmount = 25 }, "HP Potion", "Health potion", thisItem.itemImage, 1, true, false, false);
                    playerInventory.inventoryItems.Add(inventoryItemInstance);
                }
            }

            






            EmoteManager.Instance.DisplayPopUp(thisItem.itemImage);
        }

    }
}
