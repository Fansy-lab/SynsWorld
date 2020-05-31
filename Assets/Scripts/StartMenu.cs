using Cinemachine;
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
    [SerializeField] CinemachineVirtualCamera cinemachineVirtualCamera;
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
        newSaveGamePanel.SetActive(true);
    }

    public void CreateSaveAndLoadThatNewSave(string nameSave)
    {

        //set default location of player start
        var saveGameComponents = new SaveGameComponents(-54, 3.6f, 1, new List<InventoryItem>(),new Dictionary<InventoryItem.Slot, InventoryItem>(),new List<InventoryItem>(), 0, 0);

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
            string creationDate = save.CreationTime.ToString();
            GameObject gO = Instantiate(loadButton, loadPanelGrid.transform) as GameObject;
            gO.GetComponentInChildren<TextMeshProUGUI>().text = nombre + "\r\n" + creationDate;
            gO.GetComponentInChildren<Button>().onClick.AddListener(() => LoadSave(nombre));
            gO.transform.GetChild(1).GetComponent<Button>().onClick.AddListener(() => DeleteSave(save.Name));

        }

        loadPanel.SetActive(true);
    }



    private void LoadSave(string load)
    {
        TransitionManager.instance.ShowNormalTransition();

        SaveData.current = (SaveData)SerializationManager.Load(Application.persistentDataPath + "/saves/" + load + ".save");


        SceneManager.LoadScene(SaveData.current.data._scene);

        StartCoroutine("waitForSceneLoad", SaveData.current.data);

        TransitionManager.instance.EndNormalTransition();


    }

    public void Settings()
    {
        GM.Instance.ToggleSettings();
    }
    public void CloseLoadMenu()
    {
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

        Instantiate(GM.Instance.Player, GM.Instance.Player.transform.position, Quaternion.identity);
        GameObject Player = GameObject.FindGameObjectWithTag("Player");
        PlayerStats stats = Player.GetComponent<PlayerStats>();
        LevelSystem levelSystem = Player.GetComponent<LevelSystem>();

        Player.GetComponent<Transform>().position = new Vector3(SaveData.current.data._xPosition, SaveData.current.data._yPosition);
        stats.gold = SaveData.current.data._gold;
        stats.experience = SaveData.current.data._experience;
        levelSystem.CalculateVariables();
        InventoryManager.instance.playerInventory.inventoryItems = new InventoryToSave().DeSerializeInventory(SaveData.current.data._inventory);
        InventoryManager.instance.privateChestInventory.inventoryItems = new InventoryToSave().DeSerializePrivateInventory(SaveData.current.data._privateChest);
        InventoryManager.instance.playerInventory.equipedItems = new InventoryToSave().DeSerializeEquipedItems(SaveData.current.data._equipedItems);
        cinemachineVirtualCamera.Follow = Player.transform;

        CloseLoadMenuAndremoveChildren();
        gameObject.SetActive(false);
    }

    private static void ResetInventories()
    {
        InventoryManager.instance.playerInventory.equipedItems = new Dictionary<InventoryItem.Slot, InventoryItem>();
        InventoryManager.instance.playerInventory.inventoryItems = new List<InventoryItem>();
        InventoryManager.instance.privateChestInventory.inventoryItems = new List<InventoryItem>();
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
