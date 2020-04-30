using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class InventorySlot : MonoBehaviour
{
    [Header("UI stuff to change")]
    [SerializeField] TextMeshProUGUI itemNameText;
    [SerializeField] TextMeshProUGUI itemNumberText;
    [SerializeField] Image itemImage;

    [Header("Variables from the Item")]
    public InventoryItem thisItem;
    public InventoryManager thisManager;


    public void Setup(InventoryItem newItem,InventoryManager newManager)
    {
        thisItem = newItem;
        thisManager = newManager;
        if (thisItem != null)
        {
            itemImage.sprite = newItem.itemImage;
            itemNumberText.text = newItem.numberHeld.ToString();
            itemNameText.text = newItem.itemName;
        }
    }

    public void ClickedOn()
    {
        if (thisItem)
        {
            thisManager.SetupDescriptionAndButton(thisItem.itemDescription,thisItem.usable,thisItem.equipable,thisItem);
        }
    }


}
