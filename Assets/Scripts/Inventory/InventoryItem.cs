using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


[CreateAssetMenu(fileName ="New Item",menuName ="Inventory/Items")]
public class InventoryItem : ScriptableObject
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


    public EquipableArmoryStats equipableArmoryStats;
    public EquipableWeaponryStats equipableWeaponryStats;
    public UsableStats usableStats;

    public UnityEvent thisEvent;

    public void Init(EquipableArmoryStats armoryStats,EquipableWeaponryStats
        weaponryStats, UsableStats usableStats,Slot? slot,string itemName,string itemDescription,Sprite itemImage,int numberHeld,
        bool usable,bool equipable,bool unique,Guid guid)
    {
        this.guid = guid;
        this.itemDescription = itemDescription;
        this.itemName = itemName;
        this.itemImage = itemImage;
        this.numberHeld = numberHeld;
        this.usable = usable;
        this.equipable = equipable;
        this.unique = unique;
        if(slot !=null)
            this.slot = slot.Value;
        if (armoryStats != null)
        {
            equipableArmoryStats = armoryStats;
        }
        if (weaponryStats != null)
        {
            equipableWeaponryStats = weaponryStats;
        }
        if (usable)
        {
            this.usableStats = usableStats;
        }
    }

    public void Equip(PlayerInventory playerInventory)
    {
        if (thisEvent != null)
            thisEvent.Invoke();
        InventoryItem itemSustituido = playerInventory.EquipItem(this);
        playerInventory.inventoryItems.Remove(this);
        if (itemSustituido)
        {


            InventoryItem inventoryItemInstance = ScriptableObject.CreateInstance("InventoryItem") as InventoryItem;
            inventoryItemInstance.Init(new EquipableArmoryStats() { ArmorAmmount = itemSustituido.equipableArmoryStats.ArmorAmmount, HealthAmmount = itemSustituido.equipableArmoryStats.HealthAmmount },
                null, null, itemSustituido.slot, itemSustituido.itemName, itemSustituido.itemDescription,
                itemSustituido.itemImage, 1, false, true, false, itemSustituido.guid);


            playerInventory.inventoryItems.Add(inventoryItemInstance);
            Debug.Log("Item EQUIPED: " + guid + "    Item in inventory: " + itemSustituido.guid);
        }

        RecalculateStats(playerInventory.equipedItems);
        PlayAnimationInEquipmentInventory();

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
                        totalAttack += entry.Value.equipableWeaponryStats.Attack;
                        totalAttackSpeed += entry.Value.equipableWeaponryStats.AttackSpeed;
                        
                    }

                }
            }
            playerStats.playerData.armor = totalArmor;
            playerStats.playerData.maxHealth = totalMaxHP +100;
            playerStats.playerData.attack = totalAttack;
            playerStats.playerData.attackSpeed = totalAttack;
            playerStats.playerData.evasion = totalEvasion;
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
            InventoryItem inventoryItemInstance = ScriptableObject.CreateInstance("InventoryItem") as InventoryItem;
            inventoryItemInstance.Init(new EquipableArmoryStats() { ArmorAmmount = itemToUnEquip.equipableArmoryStats.ArmorAmmount,
                HealthAmmount = itemToUnEquip.equipableArmoryStats.HealthAmmount },
                null, null, itemToUnEquip.slot, itemToUnEquip.itemName, itemToUnEquip.itemDescription,
                itemToUnEquip.itemImage, 1, false, true, false, itemToUnEquip.guid);


            playerInventory.inventoryItems.Add(inventoryItemInstance);

          
        }
        RecalculateStats(playerInventory.equipedItems);
        

    }
}
