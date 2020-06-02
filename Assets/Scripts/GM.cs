using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class GM : MonoBehaviour
{



    [SerializeField] private GameObject questsUI;
    [SerializeField] private GameObject inventoryUI;
    [SerializeField] public GameObject PrivateChestInventoryUI;
    [SerializeField] private GameObject playerStatsUI;
    [SerializeField] private GameObject mainMenuUI;
    [SerializeField] private GameObject settingsUI;
    [SerializeField] private GameObject enterLeaveLocationUI;

    public GameObject Player;




    private static GM instance;
    private static int m_referenceCount = 0;


    public static GM Instance
    {
        get
        {
            return instance;
        }
    }

    void Awake()
    {
        m_referenceCount++;
        if (m_referenceCount > 1)
        {
            DestroyImmediate(this.gameObject);
            return;
        }

        instance = this;
        // Use this line if you need the object to persist across scenes
        DontDestroyOnLoad(this.gameObject);
    }

    void OnDestroy()
    {
        m_referenceCount--;
        if (m_referenceCount == 0)
        {
            instance = null;
        }

    }

    internal void RespawnPlayer()
    {
        StartCoroutine(SpawnPlayerNextToChestAfter3Seconds());
    }

    private IEnumerator SpawnPlayerNextToChestAfter3Seconds()
    {
        Transform chestLocation = GameObject.Find("SaveChest").transform;
        yield return new WaitForSeconds(3);
        SpawnNewPlayerToPosition(chestLocation);

    }

    public void SpawnNewPlayerToPosition(Transform location)
    {
        GameObject newPlayer = Instantiate(Player, location.position, Quaternion.identity) as GameObject;
        CinemachineVirtualCamera cam = GetComponentInChildren<CinemachineVirtualCamera>();
        cam.Follow = newPlayer.transform;
    }

    private void Start()
    {
        InitialSetup();

    }

    public void InitialSetup()
    {
        mainMenuUI.SetActive(true);
        inventoryUI.SetActive(false);
        questsUI.SetActive(false);
        settingsUI.SetActive(false);

        enterLeaveLocationUI.SetActive(true);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (questsUI.activeInHierarchy)
            {
                questsUI.SetActive(false);
                return;
            }



            if (inventoryUI.activeInHierarchy)
            {
                CloseInventoryAndPrivateChest();
                return;
            }


            if (settingsUI.activeInHierarchy)
            {
                settingsUI.SetActive(false);
                return;
            }



            bool mainMenuStatus = mainMenuUI.activeSelf;
            mainMenuUI.SetActive(!mainMenuStatus);
        }
    }

    public void CloseInventoryAndPrivateChest()
    {
        ClosePrivateChest();
        inventoryUI.SetActive(false);
    }

    private void ClosePrivateChest()
    {
        if (PrivateChestInventoryUI.activeInHierarchy)
        {
            PrivateChestInventoryUI.SetActive(false);

        }
    }

    internal void ToggleInventoryPanel()
    {
        playerStatsUI.SetActive(true);
        PrivateChestInventoryUI.SetActive(false);
        bool a = inventoryUI.activeInHierarchy;
        inventoryUI.SetActive(!a);
    }




    public void ToggleQuests()
    {
        bool a = questsUI.activeInHierarchy;
        if (a)
        {
            SoundEffectsManager.instance.PlayCloseQuestSound();
        }
        else
        {
            SoundEffectsManager.instance.PlayOpenQuestSound();
        }
        questsUI.SetActive(!a);
    }
    public void ToggleSettings()
    {
        mainMenuUI.SetActive(false);
        bool a = settingsUI.activeInHierarchy;
        settingsUI.SetActive(!a);
    }

    public void PrintHi()
    {
        print("hi");
    }

    public void SaveGame()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        PlayerStats stats = player.GetComponent<PlayerStats>();
        SaveGameComponents components = new SaveGameComponents(player.transform.position.x, player.transform.position.y,
            SceneManager.GetActiveScene().buildIndex,
            InventoryManager.instance.playerInventory.inventoryItems,
            InventoryManager.instance.playerInventory.equipedItems,
            InventoryManager.instance.privateChestInventory.inventoryItems,
            stats.gold, stats.experience);

        StartMenu.instance.SaveGame(components);
    }

    public void OpenPrivateChestInventory()
    {

        TogglePrivateChest();
    }

    private void TogglePrivateChest()
    {

        inventoryUI.SetActive(true);
        playerStatsUI.SetActive(false);
        PrivateChestInventoryUI.SetActive(true);
    }

    internal void CallMethod(string methodToCallInGm, List<string> parameters)
    {
        Type thisType = this.GetType();
        MethodInfo theMethodToCall = thisType.GetMethod(methodToCallInGm);
        object[] objects;
        objects = parameters.Cast<object>().ToArray();

        theMethodToCall.Invoke(this, objects);
    }
}
