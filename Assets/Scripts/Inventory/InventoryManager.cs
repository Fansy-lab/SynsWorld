using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
public class InventoryManager : MonoBehaviour
{

    [Header("Inventory Information")]
    public PlayerInventory plyerInventory;
    [SerializeField] private GameObject blankInventorySlot;
    [SerializeField] private GameObject inventoryPanel;
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private GameObject useButton;
    public InventoryItem currentItemSelected;


    public void SetTextAndButton(string description,bool active)
    {
        descriptionText.text = description;
        if (active)
            useButton.SetActive(true);
        else
            useButton.SetActive(false);
    }

    void OnEnable() //everytime it gets active in the scene
    {
        UpdateInventoryUI();
        SetTextAndButton("", false);
    }
    public void MakeInventorySlots()
    {
        if (plyerInventory)
        {

            foreach (var item in plyerInventory.inventoryItems.ToList())
            {
                if( item !=null && item.numberHeld > 0)
                {
                    GameObject temp = Instantiate(blankInventorySlot, inventoryPanel.transform.position, Quaternion.identity);
                    temp.transform.SetParent(inventoryPanel.transform);

                    InventorySlot newSlot = temp.GetComponent<InventorySlot>();
                    newSlot.transform.SetParent(inventoryPanel.transform);
                    newSlot.Setup(item, this);
                }
                else
                {
                    plyerInventory.inventoryItems.Remove(item);

                }
            }  
            
           
        }
    }

    public void SetupDescriptionAndButton(string description,bool isUsable,bool isEquipable,InventoryItem newItem)
    {
        descriptionText.text = description;
        
        useButton.SetActive(true);
        currentItemSelected = newItem;
        if (isUsable)
        {
            useButton.GetComponentInChildren<TextMeshProUGUI>().text = "Use";
            descriptionText.text += "\r\n Restores: " + newItem.usableStats.HPRestoreAmmount + " health";
        }
        else if (isEquipable)
        {
            if(newItem.equipableArmoryStats != null)
            {
                descriptionText.text +=  "\r\n Armor: +" + newItem.equipableArmoryStats.ArmorAmmount + "\r\n HP: +" + newItem.equipableArmoryStats.HealthAmmount;

            }
            if(newItem.equipableWeaponryStats != null)
            {
                descriptionText.text += "\r\n + Damage: +" + newItem.equipableWeaponryStats.PlusDamage;

            }
            useButton.GetComponentInChildren<TextMeshProUGUI>().text = "Equip";

        }
    }

    void ClearInventorySlots()
    {
        for (int i = 0; i < inventoryPanel.transform.childCount; i++)
        {
            Destroy(inventoryPanel.transform.GetChild(i).gameObject);
        }
    }

    public void UseButtonPressed()
    {
        if (currentItemSelected)
        {
            currentItemSelected.Use();

            UpdateInventoryUI();

            if (currentItemSelected.numberHeld == 0)
                SetTextAndButton("", false);
        }

    }

    public void UpdateInventoryUI()
    {
        //clear all inventory slots
        ClearInventorySlots();
        //and refill invenotry slots with new numbers
        MakeInventorySlots();
    }
}
