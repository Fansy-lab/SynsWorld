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
    public TextMeshProUGUI maxHP;
    public TextMeshProUGUI armor;
    public TextMeshProUGUI evasion;
    [Header("Player Experience")]

    public Slider expSlider;
    public TextMeshProUGUI levelText;



    public PlayerInventory playerInventory;
    public PlayerInventory privateChestInventory;
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
    [SerializeField] private GameObject PrivateChestPanel;

    [SerializeField] private TextMeshProUGUI maxItemsText;
    [SerializeField] private TextMeshProUGUI maxItemsTextPrivateChest;


    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private GameObject useButton;
    [SerializeField] private GameObject unEquipButton;
    [SerializeField] private GameObject destroyButton;
    [SerializeField] private GameObject retrieveButton;

    public InventoryItem currentItemSelectedInInventory;
    public InventoryItem currentItemSelectedInEquipment;
    public InventoryItem currentItemSelectedInPrivateChest;

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
        GlobalEvents.OnLevelUp += UpdatePlayerData;


    }

    internal void SetupDescription(string description, InventoryItem newItem)
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
                descriptionText.text += "\r\n  Min Damage: +" + newItem.equipableWeaponryStats.AttackMinDamage;
                descriptionText.text += "\r\n  Max Damage: +" + newItem.equipableWeaponryStats.AttackMaxDamage;

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

    internal void SetupToTransferToIventory(InventoryItem thisItem)
    {
        if (thisItem != null)
        {
            currentItemSelectedInPrivateChest = thisItem;
            retrieveButton.GetComponent<Button>().interactable = true;
            retrieveButton.GetComponent<Button>().onClick.RemoveAllListeners();
            retrieveButton.GetComponent<Button>().onClick.AddListener(() => RetrieveButtonPressed(thisItem));

        }
    }

    internal void SetupUnEquipButton(InventoryItem thisItem, InventorySlot slot)
    {
        if (thisItem != null)
        {
            currentItemSelectedInEquipment = thisItem;
            unEquipButton.GetComponent<Button>().interactable = true;
            unEquipButton.GetComponent<Button>().onClick.AddListener(() => UnEquipButtonPressed(slot));
        }
    }

    void OnEnable() //everytime it gets active in the scene
    {
        CleanDescription();
        UpdatePlayerData(null);

        SetExperienceSlider();
        ClearPrivateChestInventorySlots();
        UpdatePrivateChestUI();

        UpdateInventoryUI();

        SetButtonTexts(false);

        UpdateGoldUI();

        UpdateEquipmentUI();

        UpdateSlotsTaken();






    }

    private void UpdateSlotsTaken()
    {
        PlayerStats plyerStats = GameObject.FindObjectOfType<PlayerStats>();
        if (plyerStats)
        {
            int currentItems = playerInventory.inventoryItems.Count;
            int maxItems = plyerStats.maxItemsCanHold;

            maxItemsText.text = currentItems + "/" + maxItems;
        }
    }
    private void UpdateSlotsTakenPrivateStash()
    {
        PlayerStats plyerStats = GameObject.FindObjectOfType<PlayerStats>();
        if (plyerStats)
        {
            int currentItems = privateChestInventory.inventoryItems.Where(x => x != null).Count();
            int maxItems = plyerStats.maxItemsCanHoldInPrivateStash;

            maxItemsTextPrivateChest.text = currentItems + "/" + maxItems;
        }


    }
    public bool ThereAreFreeSlotsInInventory()
    {
        PlayerStats plyerStats = GameObject.FindObjectOfType<PlayerStats>();
        if (plyerStats)
        {
            int currentItemsInInventory = playerInventory.inventoryItems.Where(x => x != null).Count();
            int maxItems = plyerStats.maxItemsCanHold;

            if (currentItemsInInventory + 1 <= maxItems)
            {
                return true;
            }

        }
        return false;
    }
    public bool ThereAreFreeSlotsInPrivateChest()
    {
        PlayerStats plyerStats = GameObject.FindObjectOfType<PlayerStats>();
        if (plyerStats)
        {
            int currentItemsInPrivateChest = privateChestInventory.inventoryItems.Where(x => x != null).Count();
            int maxItems = plyerStats.maxItemsCanHoldInPrivateStash;

            if (currentItemsInPrivateChest + 1 <= maxItems)
            {
                return true;
            }

        }
        return false;
    }

    private void UpdateGoldUI()
    {
        PlayerStats plyerStats = GameObject.FindObjectOfType<PlayerStats>();
        if (plyerStats)
            GoldText.text = plyerStats.gold.ToString();
    }

    private void SetExperienceSlider()
    {
        SetLevelText();
        expSlider.maxValue = LevelSystem.totalExpForTheLevel;
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
    private void ResetExpBar(int? expForLevelUp)
    {
        expSlider.value = 0;
        expSlider.maxValue = expForLevelUp.Value;

        SetLevelText();
    }

    private void UpdatePlayerData(int? exp)
    {
        InventoryItem.RecalculateStats(playerInventory.equipedItems);
        PlayerStats plyerStats = GameObject.FindObjectOfType<PlayerStats>();
        if (plyerStats)
        {
            attack.text = "DPS: " + plyerStats.DPS;
            armor.text = "Armor: " + plyerStats.armor;
            maxHP.text = "Max HP: " + plyerStats.maxHealth;
            evasion.text = "Evasion: " + plyerStats.evasion;
        }
    }
    private void UpdatePrivateChestUI()
    {
        if (privateChestInventory != null)
        {

            foreach (var item in privateChestInventory.inventoryItems.ToList())
            {
                if (item != null && item.numberHeld > 0)
                {
                    GameObject temp = Instantiate(blankInventorySlot, PrivateChestPanel.transform.position, Quaternion.identity);
                    temp.transform.SetParent(PrivateChestPanel.transform);
                    temp.gameObject.name = item.guid.ToString();
                    InventorySlot newSlot = temp.GetComponent<InventorySlot>();
                    Button button = temp.GetComponentInChildren<Button>();
                    button.onClick = new Button.ButtonClickedEvent();
                    button.onClick.AddListener(() => newSlot.ClickedOnInPrivateChest());

                    newSlot.Setup(item, this);

                }
                else
                {
                    playerInventory.inventoryItems.Remove(item);

                }
            }
        }
    }

    private void UpdateEquipmentUI()
    {
        InventoryItem head = null;
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

        if (head != null)
        {

            Image headImage = headSlot.transform.GetChild(0).GetComponentInChildren<Image>();

            headImage.sprite = head.itemImage;
            headImage.color = new Color(255, 255, 255, 255);

            InventorySlot slotInPlace = headSlot.GetComponent<InventorySlot>();
            slotInPlace.thisItem = head;

        }
        else
        {
            Image headImage = headSlot.transform.GetChild(0).GetComponentInChildren<Image>();

            headImage.sprite = null;
            headImage.color = new Color(0, 0, 0, 0);

        }
        if (chest != null)
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
        if (gloves != null)
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
        if (weapon != null)
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
        if (leggings != null)
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
        if (boots != null)
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
        if (playerInventory != null)
        {

            foreach (var item in playerInventory.inventoryItems.ToList())
            {
                if (item != null && item.numberHeld > 0)
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

        if (newItem.isTrash)
        {
            if (GM.Instance.PrivateChestInventoryUI.activeInHierarchy)
            {
                useButton.SetActive(true);
                SetButtonText("Transfer");
            }
            else
            {
                useButton.SetActive(false);
            }
        }

        currentItemSelectedInInventory = newItem;

        if (newItem.usable)
        {
            SetButtonText("Use");
            descriptionText.text = "Restores 25% of Max HP ";

        }
        else if (newItem.isTrash)
        {
            descriptionText.text = newItem.itemDescription;
        }
        else if (!newItem.usable)
        {


            InventoryItem itemInTheSlot = CheckIfSlotIsTakenAndReturnItemIfOcupied(newItem.slot);

            if (itemInTheSlot != null)
            {


                SetButtonText("Replace");
                if (newItem.slot != InventoryItem.Slot.weapon)
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
                else if (newItem.slot == InventoryItem.Slot.weapon)
                {
                    int minAttack = itemInTheSlot.equipableWeaponryStats.AttackMinDamage;
                    int maxAttack = itemInTheSlot.equipableWeaponryStats.AttackMaxDamage;
                    int attackSpeed = itemInTheSlot.equipableWeaponryStats.AttackSpeed;
                    int currentCalculatedDPS = ((minAttack + maxAttack) / 2) * attackSpeed;

                    int newMinAttack = newItem.equipableWeaponryStats.AttackMinDamage;
                    int newMaxAttack = newItem.equipableWeaponryStats.AttackMaxDamage;
                    int newAttackSpeed = newItem.equipableWeaponryStats.AttackSpeed;
                    int newCurrentCalculatedDPS = ((newMinAttack + newMaxAttack) / 2) * newAttackSpeed;

                    descriptionText.text = "If equipped:";
                    int difference = currentCalculatedDPS - newCurrentCalculatedDPS;

                    if (difference <= 0)
                        descriptionText.text += "\r\n DPS: +" + Mathf.Abs(difference);
                    else
                        descriptionText.text += "\r\n DPS: -" + difference;

                }
            }
            else
            {

                SetButtonText("Equip");
                if (newItem.slot != InventoryItem.Slot.weapon)
                {
                    descriptionText.text = "If equipped:";
                    descriptionText.text += "\r\n Armor: +" + newItem.equipableArmoryStats.ArmorAmmount;
                    descriptionText.text += "\r\n HP: +" + newItem.equipableArmoryStats.HealthAmmount;
                    descriptionText.text += "\r\n Evasion: +" + newItem.equipableArmoryStats.EvasionAmmount;



                }
                else if (newItem.slot == InventoryItem.Slot.weapon)
                {

                    int newMinAttack = newItem.equipableWeaponryStats.AttackMinDamage;
                    int newMaxAttack = newItem.equipableWeaponryStats.AttackMaxDamage;
                    int newAttackSpeed = newItem.equipableWeaponryStats.AttackSpeed;
                    int newCurrentCalculatedDPS = ((newMinAttack + newMaxAttack) / 2) * newAttackSpeed;
                    descriptionText.text = "If equipped:";
                    descriptionText.text += "\r\n DPS: +" + newCurrentCalculatedDPS;

                }
            }



        }

    }

    private void SetButtonText(string text)
    {

        if (GM.Instance.PrivateChestInventoryUI.activeInHierarchy)
        {
            useButton.GetComponentInChildren<TextMeshProUGUI>().text = "Transfer";

        }
        else
        {
            useButton.GetComponentInChildren<TextMeshProUGUI>().text = text;

        }

    }

    public InventoryItem CheckIfSlotIsTakenAndReturnItemIfOcupied(InventoryItem.Slot slot)
    {
        if (playerInventory != null)
        {
            InventoryItem itemEquiped = null;
            playerInventory.equipedItems.TryGetValue(slot, out itemEquiped);


            if (itemEquiped == null)
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
    void ClearPrivateChestInventorySlots()
    {
        for (int i = 0; i < PrivateChestPanel.transform.childCount; i++)
        {
            Destroy(PrivateChestPanel.transform.GetChild(i).gameObject);
        }
    }


    public void RetrieveButtonPressed(InventoryItem itemToRetrieve)
    {
        if (currentItemSelectedInPrivateChest != null)
        {
            if (ThereAreFreeSlotsInInventory())
            {
                currentItemSelectedInPrivateChest.TransferToInventory(playerInventory, privateChestInventory);
                retrieveButton.GetComponent<Button>().interactable = false;
                UpdatePrivateChestUI();
                UpdateInventoryUI();
            }


        }
    }
    public void UnEquipButtonPressed(InventorySlot slot)
    {
        if (currentItemSelectedInEquipment != null)
        {
            currentItemSelectedInEquipment.UnEquip(playerInventory, currentItemSelectedInEquipment);
            UpdateEquipmentUI();
            UpdateInventoryUI();
            slot.thisItem = null;
            foreach (Transform item in inventoryPanel.transform)
            {
                if (item.name == currentItemSelectedInEquipment.guid.ToString())
                {
                    item.GetComponent<Animator>().SetTrigger("Pop");
                }
            }



            unEquipButton.GetComponent<Button>().interactable = false;
            currentItemSelectedInEquipment = null;
        }
        UpdatePlayerData(null);
    }

    public void UseButtonPressed()
    {
        if (currentItemSelectedInInventory != null)
        {

            if (GM.Instance.PrivateChestInventoryUI.activeInHierarchy)//chest is open
            {
                //transfer  to private chest
                if (ThereAreFreeSlotsInPrivateChest())
                {
                    currentItemSelectedInInventory.TransferToPrivateChest(playerInventory, privateChestInventory);
                    SetButtonTexts(false);
                    CleanDescription();
                    UpdatePrivateChestUI();
                }

            }
            else
            {
                if (currentItemSelectedInInventory.equipable)
                {
                    currentItemSelectedInInventory.Equip(playerInventory);
                    SetButtonTexts(false);
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
            }



            UpdateInventoryUI();
            UpdateEquipmentUI();

            UpdatePlayerData(null);

        }

    }



    public void DestroyButtonPressed()
    {
        if (currentItemSelectedInInventory != null)
        {
            playerInventory.inventoryItems.Remove(currentItemSelectedInInventory);
            UpdateInventoryUI();

            SetButtonTexts(false);
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

        UpdateSlotsTaken();


        if (PrivateChestPanel.activeInHierarchy)
        {
            ClearPrivateChestInventorySlots();
            UpdatePrivateChestUI();
            UpdateSlotsTakenPrivateStash();

        }

    }
}
