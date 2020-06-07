using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

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

    public void Equip(PlayerInventory playerInventory)
    {

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

            int totalMaxHP = 0;
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

            playerStats.RecalculateMaxHP();
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

        if (playerInventory != null)
        {



            playerInventory.inventoryItems.Add(itemToUnEquip);


        }
        RecalculateStats(playerInventory.equipedItems);
        SoundEffectsManager.instance.PlayUnEquippedItemSound();

    }

    internal void TransferToInventory(PlayerInventory playerInventory, PlayerInventory privateChestInventory)
    {




        if (this.usable)
        {
            SoundEffectsManager.instance.PlayPickedMiscItemSound();

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
            SoundEffectsManager.instance.PlayPickedUpItemSound();

            playerInventory.inventoryItems.Add(this);
            privateChestInventory.inventoryItems.Remove(this);
        }
    }
    public void TransferToPrivateChest(PlayerInventory inventory, PlayerInventory privateChest)
    {

        if (this.usable)
        {
            SoundEffectsManager.instance.PlayPickedMiscItemSound();
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
            SoundEffectsManager.instance.PlayPickedUpItemSound();
            privateChest.inventoryItems.Add(this);
            inventory.inventoryItems.Remove(this);
        }
    }
}
