using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class InventorySlot : MonoBehaviour, IPointerEnterHandler,IPointerExitHandler
{
    [Header("UI stuff to change")]
    [SerializeField] TextMeshProUGUI itemNameText;
    [SerializeField] TextMeshProUGUI itemNumberText;
    [SerializeField] Image itemImage;

    [Header("Variables from the Item")]
    public InventoryItem thisItem;
    public InventoryManager thisManager;

    [SerializeField]private ToolTip toolTip;



    public void Setup(InventoryItem newItem,InventoryManager newManager)
    {
        toolTip = newManager.toolTip;
        thisItem = newItem;
        thisManager = newManager;
        if (thisItem != null)
        {
            itemImage.sprite = newItem.itemImage;
            itemNumberText.text = newItem.numberHeld.ToString();
            itemNameText.text = newItem.itemName;
        }
    }

    public void ClickedOnInInventory()
    {
        if (thisItem.itemName != "")//if is not null
        {
            thisManager.SetupDifferences(thisItem);
        }
    }
    public void ClickedOnInPrivateChest()
    {
        if (thisItem.itemName != "")//if is not null
        {
            thisManager.SetupToTransferToIventory(thisItem);
        }
    }
    public void ClickedOnInEquipment()
    {
        if (thisItem.itemName !="")//if is not null
        {
            thisManager.SetupUnEquipButton(thisItem,this);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (toolTip)
        {
            if (thisItem != null)
            {


                if (thisItem.equipable)
                {
                    string descriptionToDisplay = "";


                    if (thisItem.equipable)
                    {
                        if (thisItem.slot !=InventoryItem.Slot.weapon)
                        {
                            descriptionToDisplay += " Armor: +" +
                                thisItem.equipableArmoryStats.ArmorAmmount + "\r\n HP: +" +
                                thisItem.equipableArmoryStats.HealthAmmount + "\r\n Evasion: +" +
                                thisItem.equipableArmoryStats.EvasionAmmount;

                        }
                        else
                        {
                            descriptionToDisplay += " Min Damage: +" + thisItem.equipableWeaponryStats.AttackMinDamage;
                            descriptionToDisplay += "\r\n Max Damage: +" + thisItem.equipableWeaponryStats.AttackMaxDamage;

                            descriptionToDisplay += "\r\n Att.Speed: +" + thisItem.equipableWeaponryStats.AttackSpeed;


                        }


                    }

                    if (transform.position.x > Screen.width / 2)
                    {
                        toolTip.GetComponent<RectTransform>().pivot = new Vector2(1, 0);

                    }
                    else
                    {
                        toolTip.GetComponent<RectTransform>().pivot = new Vector2(-0.25f, 0);
                    }


                    toolTip.ShowTooltip(descriptionToDisplay, transform.position);

                }

            }
        }

    }



    public void OnPointerExit(PointerEventData eventData)
    {
        if(toolTip)
        toolTip.HideTooltip();

       // thisManager.CleanDescription();
    }

}