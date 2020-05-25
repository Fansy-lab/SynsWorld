using System;
using System.Collections;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class PhysicalInventoryItem : MonoBehaviour
{
    [SerializeField] PlayerInventory playerInventory;
    [SerializeField] public InventoryItem thisItem;

    private void Start()
    {
        Destroy(gameObject, 30);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.transform.tag == "Player")
        {

            bool addedToInventory = AddItemToInventory();

            if (addedToInventory)
                Destroy(gameObject);

        }
    }


    bool AddItemToInventory()
    {
        if (playerInventory && thisItem)
        {



            if (thisItem.equipable)
            {
                if (playerInventory.inventoryItems.Count >= GameObject.FindObjectOfType<PlayerStats>().maxItemsCanHold)
                {
                    return false;
                }

                Guid guid = Guid.NewGuid();

                InventoryItem inventoryItemInstance = ScriptableObject.CreateInstance("InventoryItem") as InventoryItem;
                if (thisItem.slot == InventoryItem.Slot.weapon)
                {
                    inventoryItemInstance.Init(null, RNGGod.GetRandonWeaponStats(),
                       null, thisItem.slot, thisItem.itemName,
                      thisItem.itemDescription, thisItem.itemImage,
                      1, false, true, false, false, guid);
                }
                else
                {
                    inventoryItemInstance.Init(RNGGod.GetRandomArmoryStats(), null,
                    null, thisItem.slot, thisItem.itemName,
                    thisItem.itemDescription, thisItem.itemImage,
                    1, false, true, false, false, guid);
                }


                playerInventory.inventoryItems.Add(inventoryItemInstance);
                GlobalEvents.PickedItem(inventoryItemInstance);//event happened
                SoundEffectsManager.instance.PlayPickedUpItemSound();


            }
            else if (thisItem.usable)
            {
                //check if player has that consumable
                bool hasConsumable = false;
                InventoryItem consumable = null;
                foreach (var item in playerInventory.inventoryItems)
                {
                    if (item.itemName == thisItem.itemName)
                    {
                        hasConsumable = true;
                        consumable = item;
                        item.numberHeld++;

                    }
                }
                if (hasConsumable == false)
                {
                    if (playerInventory.inventoryItems.Count >= GameObject.FindObjectOfType<PlayerStats>().maxItemsCanHold)
                    {
                        return false;
                    }

                    Guid guid = Guid.NewGuid();
                    InventoryItem inventoryItemInstance = ScriptableObject.CreateInstance("InventoryItem") as InventoryItem;
                    inventoryItemInstance.Init(null, null, new UsableStats() { HPRestoreAmmount = 25 }, null, thisItem.itemName,
                        thisItem.itemDescription, thisItem.itemImage, 1, true, false, false, false, guid);
                    playerInventory.inventoryItems.Add(inventoryItemInstance);
                    GlobalEvents.PickedItem(inventoryItemInstance);//event happened

                    //Debug.Log("Added to inventory: " + guid);
                }
                else
                {
                    GlobalEvents.PickedItem(consumable);//event happened

                }
                SoundEffectsManager.instance.PlayPickedMiscItemSound();
            }
            else if (thisItem.isCurrency)
            {
                PlayerStats playerStats = GameObject.FindObjectOfType<PlayerStats>();
                playerStats.gold += RNGGod.GetGoldAmmount();

                GlobalEvents.PickedItem(thisItem);//event happened
                SoundEffectsManager.instance.PlayGoldPickedUpSound();

            }
            else if (thisItem.isTrash)
            {
                if (playerInventory.inventoryItems.Count >= GameObject.FindObjectOfType<PlayerStats>().maxItemsCanHold)
                {
                    return false;
                }
                Guid guid = Guid.NewGuid();
                InventoryItem inventoryItemInstance = ScriptableObject.CreateInstance("InventoryItem") as InventoryItem;
                inventoryItemInstance.Init(null, null, null, null, thisItem.itemName,
                    thisItem.itemDescription, thisItem.itemImage, 1, false, false, false, true, guid);
                playerInventory.inventoryItems.Add(inventoryItemInstance);
                GlobalEvents.PickedItem(inventoryItemInstance);//event happened
                SoundEffectsManager.instance.PlayPickedMiscItemSound();

            }


            EmoteManager.Instance.DisplayPopUp(thisItem.itemImage);
        }
        return true;

    }
}
