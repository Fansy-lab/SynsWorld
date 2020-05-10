using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager instance;
    private static int m_referenceCount = 0;


    [Header("Player Stats")]
    public TextMeshProUGUI attack;
    public TextMeshProUGUI attackSpeed;
    public TextMeshProUGUI maxHP;
    public TextMeshProUGUI armor;
    public TextMeshProUGUI evasion;
    [Header("Player Experience")]

    public Slider expSlider;
    public TextMeshProUGUI levelText;


[Header("Inventory Information")]
    public PlayerInventory playerInventory;
    public ToolTip toolTip;

    [SerializeField] private GameObject headSlot;
    [SerializeField] private GameObject chestSlot;
    [SerializeField] private GameObject leggingsSlot;
    [SerializeField] private GameObject weaponSlot;
    [SerializeField] private GameObject glovesSlot;
    [SerializeField] private GameObject bootsSlot;

    [SerializeField] private TextMeshProUGUI GoldText;



    [SerializeField] private GameObject blankInventorySlot;
    [SerializeField] private GameObject inventoryPanel;
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private GameObject useButton;
    [SerializeField] private GameObject unEquipButton;

    [SerializeField] private GameObject destroyButton;

    public InventoryItem currentItemSelectedInInventory;
    public InventoryItem currentItemSelectedInEquipment;

    private void Awake()
    {

        m_referenceCount++;
        if (m_referenceCount > 1)
        {
            DestroyImmediate(this.gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    private void Start()
    {
        GlobalEvents.OnPickedItem += PlayPopUpAndRefreshUI;
        GlobalEvents.OnGainedExperience += UpdateExperienceBar;
        GlobalEvents.OnLevelUp += ResetExpBar;


    }

    internal void SetupDescription(string description,InventoryItem newItem)
    {
        descriptionText.text = description;
       
       
        if (newItem.equipable)
        {
            if (newItem.equipableArmoryStats != null)
            {
                descriptionText.text += "\r\n Armor: +" +
                    newItem.equipableArmoryStats.ArmorAmmount + "\r\n HP: +" +
                    newItem.equipableArmoryStats.HealthAmmount + "\r\n Evasion: +" +
                    newItem.equipableArmoryStats.EvasionAmmount;

            }
            if (newItem.equipableWeaponryStats != null)
            {
                descriptionText.text += "\r\n  Damage: +" + newItem.equipableWeaponryStats.Attack;
                descriptionText.text += "\r\n  Att.Speed: +" + newItem.equipableWeaponryStats.AttackSpeed;


            }
           

        }
    }

    internal void CleanDescription()
    {
        descriptionText.text = "";
    }

    public void SetButtonTexts(bool active)
    {
        if (active)
        {
            useButton.SetActive(true);
            destroyButton.SetActive(true);

        }
        else
        {
            destroyButton.SetActive(false);

            useButton.SetActive(false);

        }
    }

    internal void SetupUnEquipButton(InventoryItem thisItem)
    {
        if (thisItem)
        {
            currentItemSelectedInEquipment = thisItem;
            unEquipButton.GetComponent<Button>().interactable = true;
        }
    }

    void OnEnable() //everytime it gets active in the scene
    {
        CleanDescription();
        UpdatePlayerData();

        SetExperienceSlider();

        UpdateInventoryUI();

        SetButtonTexts(false);

        UpdateGoldUI();

        UpdateEquipmentUI();
      
      

    }

    private void UpdateGoldUI()
    {
        PlayerStats plyerStats = GameObject.FindObjectOfType<PlayerStats>();
        if (plyerStats)
            GoldText.text = plyerStats.playerData.gold.ToString();
    }

    private void SetExperienceSlider()
    {
        SetLevelText();
       expSlider.maxValue= LevelSystem.totalExpForTheLevel;
       expSlider.value = LevelSystem.totalExpForTheLevel - LevelSystem.experienceForNextLevel;
                
    }

    private void SetLevelText()
    {
        levelText.text = LevelSystem.currentLevel.ToString();
    }

    private void UpdateExperienceBar(int gainedExperience)
    {
        expSlider.value += gainedExperience;
    }
    private void ResetExpBar(int expForLevelUp)
    {
        expSlider.value = 0;
        expSlider.maxValue = expForLevelUp;

        SetLevelText();
    }

    private void UpdatePlayerData()
    {
        InventoryItem.RecalculateStats(playerInventory.equipedItems);
        PlayerStats plyerStats = GameObject.FindObjectOfType<PlayerStats>();
        if (plyerStats)
        {
            attack.text = "Attack: "+ plyerStats.playerData.attack;
            attackSpeed.text = "Att.Speed: " + plyerStats.playerData.attackSpeed;
            armor.text = "Armor: "+ plyerStats.playerData.armor;
            maxHP.text = "Max HP: "+plyerStats.playerData.maxHealth;
            evasion.text = "Evasion: " +plyerStats.playerData.evasion;
        }
    }

    private void UpdateEquipmentUI()
    {
        InventoryItem head=null;
        InventoryItem chest = null;
        InventoryItem gloves = null;
        InventoryItem weapon = null;
        InventoryItem leggings = null;
        InventoryItem boots = null;
        if (playerInventory.equipedItems.ContainsKey(InventoryItem.Slot.head))
        {
             head = playerInventory.equipedItems[InventoryItem.Slot.head];
        }
        if (playerInventory.equipedItems.ContainsKey(InventoryItem.Slot.chest))
        {
            chest = playerInventory.equipedItems[InventoryItem.Slot.chest];

        }
        if (playerInventory.equipedItems.ContainsKey(InventoryItem.Slot.gloves))
        {
            gloves = playerInventory.equipedItems[InventoryItem.Slot.gloves];

        }
        if (playerInventory.equipedItems.ContainsKey(InventoryItem.Slot.weapon))
        {
            weapon = playerInventory.equipedItems[InventoryItem.Slot.weapon];

        }
        if (playerInventory.equipedItems.ContainsKey(InventoryItem.Slot.leggings))
        {
            leggings = playerInventory.equipedItems[InventoryItem.Slot.leggings];

        }

        if (playerInventory.equipedItems.ContainsKey(InventoryItem.Slot.boots))
        {
            boots = playerInventory.equipedItems[InventoryItem.Slot.boots];

        }

        if (head)
        {

            Image headImage =headSlot.transform.GetChild(0).GetComponentInChildren<Image>();
          
            headImage.sprite = head.itemImage;
            headImage.color = new Color(255, 255, 255, 255);

            InventorySlot slotInPlace = headSlot.GetComponent<InventorySlot>();
            slotInPlace.thisItem = head;

        }else
        {
            Image headImage = headSlot.transform.GetChild(0).GetComponentInChildren<Image>();

            headImage.sprite = null;
            headImage.color = new Color(0, 0, 0, 0);

        }
        if (chest)
        {

            Image chestImage = chestSlot.transform.GetChild(0).GetComponentInChildren<Image>();
            chestImage.sprite = chest.itemImage;
            chestImage.color = new Color(255, 255, 255, 255);

            InventorySlot slotInPlace = chestSlot.GetComponent<InventorySlot>();
            slotInPlace.thisItem = chest;
        }
        else
        {
            Image chestImage = chestSlot.transform.GetChild(0).GetComponentInChildren<Image>();

            chestImage.sprite = null;
            chestImage.color = new Color(0, 0, 0, 0);
        }
        if(gloves)
        {

            Image glovesImage = glovesSlot.transform.GetChild(0).GetComponentInChildren<Image>();
            glovesImage.sprite = gloves.itemImage;
            glovesImage.color = new Color(255, 255, 255, 255);

            InventorySlot slotInPlace = glovesSlot.GetComponent<InventorySlot>();
            slotInPlace.thisItem = gloves;
        }
        else
        {
            Image glovesImage = glovesSlot.transform.GetChild(0).GetComponentInChildren<Image>();

            glovesImage.sprite = null;
            glovesImage.color = new Color(0, 0, 0, 0);
        }
        if (weapon)
        {

            Image weaponImage = weaponSlot.transform.GetChild(0).GetComponentInChildren<Image>();
            weaponImage.sprite = weapon.itemImage;
            weaponImage.color = new Color(255, 255, 255, 255);

            InventorySlot slotInPlace = weaponSlot.GetComponent<InventorySlot>();
            slotInPlace.thisItem = weapon;
        }
        else
        {
            Image weaponImage = weaponSlot.transform.GetChild(0).GetComponentInChildren<Image>();

            weaponImage.sprite = null;
            weaponImage.color = new Color(0, 0, 0, 0);
        }
        if (leggings)
        {

            Image legginsImage = leggingsSlot.transform.GetChild(0).GetComponentInChildren<Image>();
            legginsImage.sprite = leggings.itemImage;
            legginsImage.color = new Color(255, 255, 255, 255);

            InventorySlot slotInPlace = leggingsSlot.GetComponent<InventorySlot>();
            slotInPlace.thisItem = leggings;
        }
        else
        {
            Image leggingsImage = leggingsSlot.transform.GetChild(0).GetComponentInChildren<Image>();

            leggingsImage.sprite = null;
            leggingsImage.color = new Color(0, 0, 0, 0);
        }
        if (boots)
        {

            Image bootsImage = bootsSlot.transform.GetChild(0).GetComponentInChildren<Image>();
            bootsImage.sprite = boots.itemImage;
            bootsImage.color = new Color(255, 255, 255, 255);

            InventorySlot slotInPlace = bootsSlot.GetComponent<InventorySlot>();
            slotInPlace.thisItem = boots;
        }
        else
        {
            Image bootsImage = bootsSlot.transform.GetChild(0).GetComponentInChildren<Image>();

            bootsImage.sprite = null;
            bootsImage.color = new Color(0, 0, 0, 0);
        }

    }

    public void MakeInventorySlots()
    {
        if (playerInventory)
        {

            foreach (var item in playerInventory.inventoryItems.ToList())
            {
                if( item !=null && item.numberHeld > 0)
                {
                    GameObject temp = Instantiate(blankInventorySlot, inventoryPanel.transform.position, Quaternion.identity);
                    temp.transform.SetParent(inventoryPanel.transform);
                    temp.gameObject.name = item.guid.ToString();
                    InventorySlot newSlot = temp.GetComponent<InventorySlot>();
                    newSlot.Setup(item, this);

                }
                else
                {
                    playerInventory.inventoryItems.Remove(item);

                }
            }  
            
           
        }
    }

    public void SetupDifferences(InventoryItem newItem)
    {
      
        
        useButton.SetActive(true);
        destroyButton.SetActive(true);

        currentItemSelectedInInventory = newItem;
        if (newItem.usable)
        {
            useButton.GetComponentInChildren<TextMeshProUGUI>().text = "Use";
            descriptionText.text = "Restores 25% of Max HP ";

        }
        else if (!newItem.usable)
        {
         

            InventoryItem itemInTheSlot = CheckIfSlotIsTaken(newItem);

            if (itemInTheSlot)
            {
                useButton.GetComponentInChildren<TextMeshProUGUI>().text = "Replace";

                if (newItem.equipableArmoryStats != null)
                {
                    int evasionDifference = itemInTheSlot.equipableArmoryStats.EvasionAmmount - newItem.equipableArmoryStats.EvasionAmmount;
                    int amorDiference = itemInTheSlot.equipableArmoryStats.ArmorAmmount - newItem.equipableArmoryStats.ArmorAmmount;
                    int healthDiference = itemInTheSlot.equipableArmoryStats.HealthAmmount - newItem.equipableArmoryStats.HealthAmmount; ;

                    descriptionText.text = "If equipped:";
                    if (amorDiference <= 0)
                        descriptionText.text += "\r\n Armor: +" + Mathf.Abs(amorDiference);
                    else
                        descriptionText.text += "\r\n Armor: -" + amorDiference;

                    if (healthDiference <= 0)
                        descriptionText.text += "\r\n HP: +" + Mathf.Abs(healthDiference);
                    else
                        descriptionText.text += "\r\n HP: -" + healthDiference;

                    if (evasionDifference <= 0)
                        descriptionText.text += "\r\n Evasion HP: +" + Mathf.Abs(evasionDifference);
                    else
                        descriptionText.text += "\r\n Evasion HP: -" + evasionDifference;



                }
                if (newItem.equipableWeaponryStats != null)
                {
                    int attack = itemInTheSlot.equipableWeaponryStats.Attack - newItem.equipableWeaponryStats.Attack;
                    int attackSpeed = itemInTheSlot.equipableWeaponryStats.AttackSpeed - newItem.equipableWeaponryStats.AttackSpeed;
                    descriptionText.text = "If equipped:";

                    if (attack <= 0)
                        descriptionText.text += "\r\n Attack: +" + Mathf.Abs(attack);
                    else
                        descriptionText.text += "\r\n Attack: -" + attack;

                    if (attackSpeed <= 0)
                        descriptionText.text += "\r\n Att.Speed: +" + Mathf.Abs(attackSpeed);
                    else
                        descriptionText.text += "\r\n Att.Speed: -" + attackSpeed;

                }
            }
            else
            {

                useButton.GetComponentInChildren<TextMeshProUGUI>().text = "Equip";
                if (newItem.equipableArmoryStats != null)
                {
                    descriptionText.text = "If equipped:";
                    descriptionText.text += "\r\n Armor: +" + newItem.equipableArmoryStats.ArmorAmmount;
                    descriptionText.text += "\r\n HP: +" + newItem.equipableArmoryStats.HealthAmmount;
                    descriptionText.text += "\r\n Evasion: +" + newItem.equipableArmoryStats.EvasionAmmount;



                }
                else if(newItem.equipableWeaponryStats != null)
                {
                    descriptionText.text = "If equipped:";
                    descriptionText.text += "\r\n Attack: +" + newItem.equipableWeaponryStats.Attack;
                    descriptionText.text += "\r\n Att.Speed: +" + newItem.equipableWeaponryStats.AttackSpeed;

                }
            }

          

        }
    }

    private InventoryItem CheckIfSlotIsTaken(InventoryItem newItem)
    {
        if (playerInventory)
        {
            InventoryItem itemEquiped = null;
            playerInventory.equipedItems.TryGetValue(newItem.slot, out itemEquiped);


            if (itemEquiped==null)
                return itemEquiped;
            else
                return itemEquiped;
        }
        return null;
    }

    void ClearInventorySlots()
    {
        for (int i = 0; i < inventoryPanel.transform.childCount; i++)
        {
            Destroy(inventoryPanel.transform.GetChild(i).gameObject);
        }
    }


    public void UnEquipButtonPressed()
    {
        if (currentItemSelectedInEquipment)
        {
            currentItemSelectedInEquipment.UnEquip(playerInventory,currentItemSelectedInEquipment);

            UpdateEquipmentUI();
            UpdateInventoryUI();

            foreach (Transform item in inventoryPanel.transform)
            {
                if(item.name == currentItemSelectedInEquipment.guid.ToString())
                {
                    item.GetComponent<Animator>().SetTrigger("Pop");
                }
            }

         

            unEquipButton.GetComponent<Button>().interactable = false;
            currentItemSelectedInEquipment = null;
        }
        UpdatePlayerData();
    }

    public void UseButtonPressed()
    {
        if (currentItemSelectedInInventory)
        {

            if (currentItemSelectedInInventory.equipable)
            {
                currentItemSelectedInInventory.Equip(playerInventory);
                SetButtonTexts( false);
                CleanDescription();
            }
            else if (currentItemSelectedInInventory.usable)
            {
                currentItemSelectedInInventory.Use(playerInventory);

                if (currentItemSelectedInInventory.numberHeld == 0)
                {
                    SetButtonTexts(false);
                    CleanDescription();
                }


            }
            UpdateInventoryUI();
            UpdateEquipmentUI();
            UpdatePlayerData();

        }

    }

    public void DestroyButtonPressed()
    {
        if (currentItemSelectedInInventory)
        {
            playerInventory.inventoryItems.Remove(currentItemSelectedInInventory);
            UpdateInventoryUI();

            SetButtonTexts( false);
            CleanDescription();
        }
    }

    public void PlayPopUpAndRefreshUI(InventoryItem itemPickedUp)
    {
        UpdateInventoryUI();

    }


    

    public void UpdateInventoryUI()
    {
        //clear all inventory slots
        ClearInventorySlots();
        //and refill invenotry slots with new numbers
        MakeInventorySlots();

        UpdateGoldUI();

    }
}
