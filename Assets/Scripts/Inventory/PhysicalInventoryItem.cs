using System;
using System.Collections;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class PhysicalInventoryItem : MonoBehaviour
{
     PlayerInventory playerInventory;
    [SerializeField] public InventoryItem thisItem;

    private void Start()
    {
        playerInventory = InventoryManager.instance.playerInventory;
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
        if (playerInventory != null && thisItem != null)
        {
            if (thisItem.equipable)
            {
                if (playerInventory.inventoryItems.Count >= GameObject.FindObjectOfType<PlayerStats>().maxItemsCanHold)
                {
                    return false;
                }

                Guid guid = Guid.NewGuid();

                if (thisItem.slot == InventoryItem.Slot.weapon)
                {


                    thisItem.equipableWeaponryStats = RNGGod.GetRandonWeaponStats();
                    thisItem.guid = guid;
                }
                else
                {

                    thisItem.equipableArmoryStats = RNGGod.GetRandomArmoryStats();
                    thisItem.guid = guid;

                }


                playerInventory.inventoryItems.Add(thisItem);
                GlobalEvents.PickedItem(thisItem);//event happened
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
                    thisItem.usableStats = new UsableStats() { HPRestoreAmmount = 25 };
                    thisItem.guid = guid;
                    playerInventory.inventoryItems.Add(thisItem);
                    GlobalEvents.PickedItem(thisItem);//event happened

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
                thisItem.guid = guid;

                playerInventory.inventoryItems.Add(thisItem);
                GlobalEvents.PickedItem(thisItem);//event happened
                SoundEffectsManager.instance.PlayPickedMiscItemSound();

            }


            EmoteManager.Instance.DisplayPopUp(thisItem.itemImage);
        }
        return true;

    }
}
