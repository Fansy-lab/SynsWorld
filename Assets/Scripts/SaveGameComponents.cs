using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Sprites;
using UnityEngine;
using static InventoryItem;

[System.Serializable]
public class SaveGameComponents
{
    public float _xPosition;
    public float _yPosition;
    public int _scene;
    public List<InventoryToSave> _inventory = new List<InventoryToSave>();
    public Dictionary<InventoryItem.Slot, InventoryToSave> _equipedItems = new Dictionary<Slot, InventoryToSave>();
    public List<InventoryToSave> _privateChest = new List<InventoryToSave>();
    public int _gold;
    public int _experience;




    public SaveGameComponents(float xposition,float ypostion,int scene, List<InventoryItem> inventory, Dictionary<InventoryItem.Slot, InventoryItem> equipedItems, List<InventoryItem> privateChest,int gold,int experience)
    {
        _xPosition = xposition;
        _yPosition = ypostion;
        _scene = scene;
        _gold = gold;
        _experience = experience;
        foreach (var item in inventory)
        {



            int[] texArr = new int[2];
            texArr[0] = 32;
            texArr[1] = 32;
            _inventory.Add(new InventoryToSave()
            {


                currencyAmmount = item.currencyAmmount,
                equipable=item.equipable,
                equipableArmoryStats = item.equipableArmoryStats,
                equipableWeaponryStats = item.equipableWeaponryStats,
                guid = item.guid,
                isCurrency = item.isCurrency,
                isTrash = item.isTrash,
                itemDescription = item.itemDescription,
                itemName = item.itemName,
                numberHeld = item.numberHeld,
                slot = item.slot,
                spriteName =item.itemImage.name,
                unique = item.unique,
                usable = item.usable,
                usableStats = item.usableStats
            }) ;
        }
        foreach (var item in privateChest)
        {
            _privateChest.Add(new InventoryToSave()
            {
                currencyAmmount = item.currencyAmmount,
                equipable = item.equipable,
                equipableArmoryStats = item.equipableArmoryStats,
                equipableWeaponryStats = item.equipableWeaponryStats,
                guid = item.guid,
                isCurrency = item.isCurrency,
                isTrash = item.isTrash,
                itemDescription = item.itemDescription,
                itemName = item.itemName,
                numberHeld = item.numberHeld,
                slot = item.slot,
                spriteName = item.itemImage.name,
                unique = item.unique,
                usable = item.usable,
                usableStats = item.usableStats
            });
        }
    }
}

[System.Serializable]
public class InventoryToSave
{





    public Guid guid;
    public string itemName;
    public string itemDescription;
    public int numberHeld;
    public string spriteName;
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

    public List<InventoryItem> DeSerializeInventory(List<InventoryToSave> list)
    {
        List<InventoryItem> listToReturn = new List<InventoryItem>();
        foreach (var item in list)
        {
            listToReturn.Add(new InventoryItem()
            {

                currencyAmmount = item.currencyAmmount,
                equipable = item.equipable,
                equipableArmoryStats = item.equipableArmoryStats,
                equipableWeaponryStats = item.equipableWeaponryStats,
                guid = item.guid,
                isCurrency = item.isCurrency,
                isTrash = item.isTrash,
                itemDescription = item.itemDescription,
                itemImage = GetSprite(item.spriteName),
                itemName = item.itemName,
                numberHeld = item.numberHeld,
                slot = item.slot,
                unique = item.unique,
                usable = item.usable,
                usableStats = item.usableStats
            }); ;
        }
        return listToReturn;
    }

    private Sprite GetSprite(string spriteName)
    {
        AtlasLoader atlasLoader = new AtlasLoader("#1 - Transparent Icons");
        Sprite itemIcon = atlasLoader.getAtlas(spriteName);
        return itemIcon;
    }
}
public class AtlasLoader
{
    public Dictionary<string, Sprite> spriteDic = new Dictionary<string, Sprite>();

    //Creates new Instance only, Manually call the loadSprite function later on
    public AtlasLoader()
    {

    }

    //Creates new Instance and Loads the provided sprites
    public AtlasLoader(string spriteBaseName)
    {
        loadSprite(spriteBaseName);
    }

    //Loads the provided sprites
    public void loadSprite(string spriteBaseName)
    {
        Sprite[] allSprites = Resources.LoadAll<Sprite>(spriteBaseName);
        if (allSprites == null || allSprites.Length <= 0)
        {
            Debug.LogError("The Provided Base-Atlas Sprite `" + spriteBaseName + "` does not exist!");
            return;
        }

        for (int i = 0; i < allSprites.Length; i++)
        {
            spriteDic.Add(allSprites[i].name, allSprites[i]);
        }
    }

    //Get the provided atlas from the loaded sprites
    public Sprite getAtlas(string atlasName)
    {
        Sprite tempSprite;

        if (!spriteDic.TryGetValue(atlasName, out tempSprite))
        {
            Debug.LogError("The Provided atlas `" + atlasName + "` does not exist!");
            return null;
        }
        return tempSprite;
    }

    //Returns number of sprites in the Atlas
    public int atlasCount()
    {
        return spriteDic.Count;
    }
}