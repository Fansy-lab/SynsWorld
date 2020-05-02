using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName ="New Item",menuName ="Inventory/Items")]
public class InventoryItem : ScriptableObject
{
    public string itemName;
    public string itemDescription;
    public Sprite itemImage;
    public int numberHeld;

    public bool usable;
    public bool equipable;
    public bool unique;


    public EquipableArmoryStats equipableArmoryStats;
    public EquipableWeaponryStats equipableWeaponryStats;
    public UsableStats usableStats;

    public UnityEvent thisEvent;

    public void Init(EquipableArmoryStats armoryStats,EquipableWeaponryStats
        weaponryStats, UsableStats usableStats,string itemName,string itemDescription,Sprite itemImage,int numberHeld,
        bool usable,bool equipable,bool unique)
    {
        this.itemDescription = itemDescription;
        this.itemName = itemName;
        this.itemImage = itemImage;
        this.numberHeld = numberHeld;
        this.usable = usable;
        this.equipable = equipable;
        this.unique = unique;
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

    public void Use()
    {
       
        if (thisEvent !=null)
            thisEvent.Invoke(); 

        if (usable)
        {
            GM.Instance.UseItem(this);

        }
        else if (equipable)
        {
            GM.Instance.EquipItem(this);
            

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
}
