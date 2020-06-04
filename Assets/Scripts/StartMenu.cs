using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartMenu : MonoBehaviour
{
    public static StartMenu instance;
    private static int m_referenceCount = 0;

    AsyncOperation asyncLoadLevel;

    public GameObject newSaveGamePanel;
    public GameObject loadPanel;
    public GameObject loadPanelGrid;
    public GameObject loadButton;

    public FileInfo[] saveFiles;
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

    public void NewGamePressed()
    {
        SoundEffectsManager.instance.PlayMenuClickSound();

        newSaveGamePanel.SetActive(true);
    }

    public void CreateSaveAndLoadThatNewSave(string nameSave)
    {

        //set default location of player start
        var saveGameComponents = new SaveGameComponents(-72,-30, 1, new List<InventoryItem>(),new Dictionary<InventoryItem.Slot, InventoryItem>(),new List<InventoryItem>(),
            0, 0,false,false,GM.Instance.GetCurrentAndDoneKillGoals(),GM.Instance.GetCurrentAndDoneReachGoals(),GM.Instance.GetCurrentAndDonePickGoals(),new List<int>(),new List<int>());

        SaveData.current.data = saveGameComponents;
        SaveData.current.saveName = nameSave;
        SerializationManager.Save(nameSave, SaveData.current);


        LoadSave(nameSave);
    }
    public void QuitGame()
    {
        Application.Quit();
    }
    public void SaveGame(SaveGameComponents data)
    {

        SaveData.current.data = data;
        SerializationManager.Save(SaveData.current.saveName, SaveData.current);
    }
    public void LoadGamePanelClicked()
    {
        SoundEffectsManager.instance.PlayMenuClickSound();

        if (!Directory.Exists(Application.persistentDataPath + "/saves/"))
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/saves/");
        }
        var dir = new DirectoryInfo(Application.persistentDataPath + "/saves/");
        saveFiles = dir.GetFiles();
        saveFiles = saveFiles.OrderByDescending(x => x.LastWriteTime).ToArray();
        foreach (var save in saveFiles)
        {
            string nombre = Path.GetFileNameWithoutExtension(save.Name);
            string creationDate = save.CreationTime.ToString("dd/MM/yy HH:mm");
            GameObject gO = Instantiate(loadButton, loadPanelGrid.transform) as GameObject;
            gO.GetComponentInChildren<TextMeshProUGUI>().text = nombre + "\r\n" + creationDate;
            gO.GetComponentInChildren<Button>().onClick.AddListener(() => LoadSave(nombre));
            gO.transform.GetChild(1).GetComponent<Button>().onClick.AddListener(() => DeleteSave(save.Name));

        }

        loadPanel.SetActive(true);
    }



    private void LoadSave(string load)
    {
        SoundEffectsManager.instance.PlayMenuClickSound();

        TransitionManager.instance.ShowNormalTransition();

        SaveData.current = (SaveData)SerializationManager.Load(Application.persistentDataPath + "/saves/" + load + ".save");


        SceneManager.LoadScene(SaveData.current.data._scene);

        StartCoroutine("waitForSceneLoad", SaveData.current.data);

        TransitionManager.instance.EndNormalTransition();


    }

    public void Settings()
    {
        SoundEffectsManager.instance.PlayMenuClickSound();

        GM.Instance.ToggleSettings();
    }
    public void CloseLoadMenu()
    {
        SoundEffectsManager.instance.PlayMenuClickSound();

        CloseLoadMenuAndremoveChildren();
    }

    private void CloseLoadMenuAndremoveChildren()
    {
        foreach (Transform child in loadPanelGrid.transform)
        {
            Destroy(child.gameObject);
        }
        loadPanel.SetActive(false);
    }
    IEnumerator waitForSceneLoad()
    {
        yield return SceneManager.LoadSceneAsync(SaveData.current.data._scene);

        UseDataToSetScenario();

    }

    private void UseDataToSetScenario()
    {
        ResetInventories();

        GM.Instance.SpawnNewPlayerToPosition(GM.Instance.Player.transform);

        GameObject Player = GameObject.FindGameObjectWithTag("Player");
        PlayerStats stats = Player.GetComponent<PlayerStats>();
        LevelSystem levelSystem = Player.GetComponent<LevelSystem>();

        Player.GetComponent<Transform>().position = new Vector3(SaveData.current.data._xPosition, SaveData.current.data._yPosition);
        stats.gold = SaveData.current.data._gold;
        stats.experience = SaveData.current.data._experience;
        stats.insideALocation = SaveData.current.data._inside;
        stats.learnedToShoot = SaveData.current.data._learnedToShoot;
        levelSystem.CalculateVariables();
        Loadinventories();
        LoadQuests();
        CloseLoadMenuAndremoveChildren();
        gameObject.SetActive(false);
    }

    private void LoadQuests()
    {
        foreach (var item in UIManager.Instance.questsService.currentQuests)
        {
            item.UnsubscribeFromEvents();
        }
        foreach (var quest in UIManager.Instance.questsService.currentQuests)
        {
            foreach (var item in quest.KillGoals)
            {
                item.UnsubscribeFromEvents();
            }

            foreach (var item in quest.PickGoals)
            {
                item.UnsubscribeFromEvents();
            }
        }


        UIManager.Instance.questsService.currentQuests = new List<Quest>();
        UIManager.Instance.questsService.completedQuests = new List<Quest>();

        UIManager.Instance.RemoveAllQuestScrollsFromUI();
        List<KillGoalData> killGoalData = SaveData.current.data._killGoalsData;
        List<ReachGoalData> reachGoalData = SaveData.current.data._reachGoalsData;
        List<PickGoalData> pickGoalData = SaveData.current.data._pickGoalsData;

        List<int> currentQuestIDs = SaveData.current.data._currentQuestIDs;
        List<int> doneQuestIDs = SaveData.current.data._doneQuestIDs;

        if(currentQuestIDs.Count>0 || doneQuestIDs.Count>0)
            UIManager.Instance.questsService.LoadQuestLists(killGoalData, reachGoalData, pickGoalData, currentQuestIDs, doneQuestIDs);

    }

    private static void Loadinventories()
    {
        InventoryManager.instance.playerInventory.inventoryItems = new InventoryToSave().DeSerializeInventory(SaveData.current.data._inventory);
        InventoryManager.instance.privateChestInventory.inventoryItems = new InventoryToSave().DeSerializePrivateInventory(SaveData.current.data._privateChest);
        InventoryManager.instance.playerInventory.equipedItems = new InventoryToSave().DeSerializeEquipedItems(SaveData.current.data._equipedItems);
    }



    private static void ResetInventories()
    {
        SaveGameComponents.ResetInventories();
    }
    public void DeleteSave(string save)
    {
        if (Directory.Exists(Application.persistentDataPath + "/saves/"))
        {
            try
            {
                File.Delete(Application.persistentDataPath + "/saves/"+save);

            }
            catch (System.Exception)
            {


            }
            foreach (Transform item in loadPanelGrid.transform)
            {
                Destroy(item.gameObject);
            }
            LoadGamePanelClicked();
        }
    }
}
