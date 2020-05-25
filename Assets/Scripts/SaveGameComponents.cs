using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveGameComponents
{
    public float _xPosition;
    public float _yPosition;
    public int _scene;
    public List<InventoryItem> _inventory;
    public List<InventoryItem> _privateChest;
    public int _gold;
    public int _experience;

    public SaveGameComponents(float xposition,float ypostion,int scene, List<InventoryItem> inventory, List<InventoryItem> privateChest,int gold,int experience)
    {
        _xPosition = xposition;
        _yPosition = ypostion;
        _scene = scene;
        _inventory = inventory;
        _privateChest = privateChest;
        _gold = gold;
        _experience = experience;
    }
}
