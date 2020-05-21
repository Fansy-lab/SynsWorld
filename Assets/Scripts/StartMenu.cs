using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartMenu : MonoBehaviour
{
    public static StartMenu instance;
    private static int m_referenceCount = 0;


    public GameObject newSaveGamePanel;
    public GameObject loadPanel;
    public GameObject loadPanelGrid;

    public GameObject loadButton;

    public string[] saveFiles;
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

        //UnityEngine.SceneManagement.SceneManager.LoadScene(1);
        //gameObject.SetActive(false);
        newSaveGamePanel.SetActive(true);
        //buttonsPanel.SetActive(false);
    }

    public void CreateSaveAndStartGame(string nameSave)
    {
        SaveData.current.xPosition = 0;
        SaveData.current.yPosition = 0;
        SaveData.current.saveName = nameSave;
        SerializationManager.Save(nameSave, SaveData.current);

        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
        gameObject.SetActive(false);
    }
    public void QuitGame()
    {
        Application.Quit();
    }
    public void SaveGame()
    {
        SaveData.current.xPosition = GameObject.Find("Player").GetComponent<Transform>().position.x;
        SaveData.current.yPosition = GameObject.Find("Player").GetComponent<Transform>().position.y;
        SerializationManager.Save(SaveData.current.saveName, SaveData.current);
    }
    public void LoadGameClicked()
    {
        if(!Directory.Exists(Application.persistentDataPath + "/saves/"))
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/saves/");
        }
        saveFiles = Directory.GetFiles(Application.persistentDataPath + "/saves/");

        foreach (var save in saveFiles)
        {
            string nombre = Path.GetFileNameWithoutExtension(save);
            DirectoryInfo info = new DirectoryInfo(save);
            string creationDate = info.CreationTime.ToString();
            GameObject gO =  Instantiate(loadButton, loadPanelGrid.transform) as GameObject;
            gO.GetComponentInChildren<TextMeshProUGUI>().text = nombre+"\r\n" +creationDate;
        }

        loadPanel.SetActive(true);
    }

    private static void LoadSave()
    {
        SaveData.current = (SaveData)SerializationManager.Load(Application.persistentDataPath + "/saves/" + SaveData.current.saveName + ".save");

        GameObject.Find("Player").GetComponent<Transform>().position = new Vector3(SaveData.current.xPosition, SaveData.current.yPosition);
    }

    public void Settings()
    {
        GM.Instance.ToggleSettings();
    }
    public void CloseLoadMenu()
    {
        foreach (Transform child in loadPanelGrid.transform)
        {
            Destroy(child.gameObject);
        }
        loadPanel.SetActive(false);
    }
}
