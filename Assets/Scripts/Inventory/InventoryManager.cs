using System.Collections;
using System.Collections.Generic;
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

    // Start is called before the first frame update
    void Start()
    {
        MakeInventorySlots();
        SetTextAndButton("", false);
    }
    public void MakeInventorySlots()
    {
        if (plyerInventory)
        {
            for (int i = 0; i < plyerInventory.inventoryItems.Count; i++)
            {
                GameObject temp = Instantiate(blankInventorySlot, inventoryPanel.transform.position, Quaternion.identity);
                temp.transform.SetParent(inventoryPanel.transform);

                InventorySlot newSlot = temp.GetComponent<InventorySlot>();
                newSlot.transform.SetParent(inventoryPanel.transform);
                newSlot.Setup(plyerInventory.inventoryItems[i], this);
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
        }
        else if (isEquipable)
        {
            useButton.GetComponentInChildren<TextMeshProUGUI>().text = "Equip";

        }
    }

    public void UseButtonPressed()
    {
        if (currentItemSelected)
        {
            currentItemSelected.Use();
        }
    }

}
