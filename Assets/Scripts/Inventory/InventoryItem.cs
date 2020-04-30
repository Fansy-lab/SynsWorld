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

    public void Use()
    {
        if (thisEvent !=null)
        {
            thisEvent.Invoke();

        }
        if (usable)
        {
            GM.Instance.UseItem(this);

        }
        else if (equipable)
        {
            GM.Instance.EquipItem(this);
        }

    }
}
