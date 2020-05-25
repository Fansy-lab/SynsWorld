using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class InventoryItem
{

    public enum Slot
    {
        head, chest, leggings, boots, weapon, gloves
    }

    public Guid guid;
    public string itemName;
    public string itemDescription;
    public Sprite itemImage;
    public int numberHeld;
    public Slot slot;
    public bool usable;
    public bool equipable;
    public bool unique;

    public bool isTrash;

    public bool isCurrency;
    public int currencyAmmount;

    public EquipableArmoryStats equipableArmoryStats;
    public EquipableWeaponryStats equipableWeaponryStats;
    public UsableStats usableStats;

    public UnityEvent thisEvent;





    public void Equip(PlayerInventory playerInventory)
    {
        if (thisEvent != null)
            thisEvent.Invoke();
        InventoryItem itemSustituido = playerInventory.EquipItem(this);
        int index = playerInventory.inventoryItems.FindIndex(a => a.guid == this.guid);

        playerInventory.inventoryItems.Remove(this);
        if (itemSustituido !=null)
        {






            playerInventory.inventoryItems.Insert(index, itemSustituido);
        }

        RecalculateStats(playerInventory.equipedItems);
        PlayAnimationInEquipmentInventory();
        SoundEffectsManager.instance.PlayEquippedItemSound();

    }

    public static void RecalculateStats(Dictionary<Slot, InventoryItem> equipedItems)
    {
        PlayerStats playerStats = GameObject.FindObjectOfType<PlayerStats>();

        if (playerStats)
        {
            int totalAttack = 0;
            int totalArmor = 0;
            int totalMaxHP = LevelSystem.currentLevel * 2;
            int totalEvasion = 0;
            int totalAttackSpeed = 0;
            foreach (KeyValuePair<Slot, InventoryItem> entry in equipedItems)
            {
                if(entry.Value != null)
                {
                    if(entry.Value.equipableArmoryStats != null)
                    {
                        totalEvasion += entry.Value.equipableArmoryStats.EvasionAmmount;
                        totalArmor += entry.Value.equipableArmoryStats.ArmorAmmount;
                        totalMaxHP += entry.Value.equipableArmoryStats.HealthAmmount;
                    }
                    if (entry.Value.equipableWeaponryStats != null)
                    {
                        totalAttack += (entry.Value.equipableWeaponryStats.AttackMinDamage + entry.Value.equipableWeaponryStats.AttackMaxDamage)/2;
                        totalAttackSpeed += entry.Value.equipableWeaponryStats.AttackSpeed;

                    }

                }
            }
            playerStats.armor = totalArmor;
            playerStats.maxHealth = totalMaxHP +100;
            playerStats.DPS = totalAttack *totalAttackSpeed;

            playerStats.evasion = totalEvasion;
            playerStats.SetMaxHealth();

        }
    }

    private void PlayAnimationInEquipmentInventory()
    {
        GameObject thisGameObjectInTheScene = GameObject.Find(slot.ToString());
        Animator anim = thisGameObjectInTheScene.GetComponentInChildren<Animator>();
        if(anim)
        anim.SetTrigger("Notice");
    }

    public void Use(PlayerInventory playerInventory )
    {

        if (thisEvent !=null)
            thisEvent.Invoke();




            Debug.Log("Using Item" +itemName);
            if (itemName.ToLower().Contains("hp"))
            {
                PlayerStats playerStats = GameObject.FindObjectOfType<PlayerStats>();
                playerStats.DrinkPotion();
            }
            DecreaseAmount(1);




    }

    public void DecreaseAmount(int amountToDecrease)
    {
        numberHeld-=amountToDecrease;
        if (numberHeld < 0)
        {
            numberHeld = 0;
        }
    }

    internal void UnEquip(PlayerInventory playerInventory,InventoryItem itemToUnEquip)
    {
        playerInventory.equipedItems[itemToUnEquip.slot] = null;

        if (playerInventory)
        {



            playerInventory.inventoryItems.Add(itemToUnEquip);


        }
        RecalculateStats(playerInventory.equipedItems);
        SoundEffectsManager.instance.PlayUnEquippedItemSound();

    }

    internal void TransferToInventory(PlayerInventory playerInventory, PlayerInventory privateChestInventory)
    {
        if (thisEvent != null)
            thisEvent.Invoke();



        if (this.usable)
        {
            bool hasUsableItemInventory = false;
            foreach (var item in playerInventory.inventoryItems.Where(x => x != null))
            {
                if (item.itemName == itemName)
                {
                    hasUsableItemInventory = true;
                    item.numberHeld += numberHeld;
                    privateChestInventory.inventoryItems.Remove(this);

                }
            }
            if (hasUsableItemInventory == false)
            {
                playerInventory.inventoryItems.Add(this);
                privateChestInventory.inventoryItems.Remove(this);
            }

        }
        else
        {
            playerInventory.inventoryItems.Add(this);
            privateChestInventory.inventoryItems.Remove(this);
        }
    }
    public void TransferToPrivateChest(PlayerInventory inventory, PlayerInventory privateChest)
    {
        if (thisEvent != null)
            thisEvent.Invoke();

        if (this.usable)
        {
            bool hasUsableItemInPrivateChest=false;
            foreach (var item in privateChest.inventoryItems.Where(x=>x!=null))
            {
                if (item.itemName == itemName)
                {
                    hasUsableItemInPrivateChest = true;
                    item.numberHeld+=numberHeld;
                    inventory.inventoryItems.Remove(this);

                }
            }
            if (hasUsableItemInPrivateChest==false)
            {
                privateChest.inventoryItems.Add(this);
                inventory.inventoryItems.Remove(this);
            }

        }
        else
        {
            privateChest.inventoryItems.Add(this);
            inventory.inventoryItems.Remove(this);
        }
    }
}
