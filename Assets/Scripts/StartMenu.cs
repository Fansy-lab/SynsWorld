using Cinemachine;
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

    AsyncOperation asyncLoadLevel;

    public GameObject newSaveGamePanel;
    public GameObject loadPanel;
    public GameObject loadPanelGrid;
    [SerializeField] CinemachineVirtualCamera cinemachineVirtualCamera;
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
        SaveData.current.scene = 1;
        SaveData.current.saveName = nameSave;
        SerializationManager.Save(nameSave, SaveData.current);


        LoadGame(nameSave);
    }
    public void QuitGame()
    {
        Application.Quit();
    }
    public void SaveGame()
    {
        SaveData.current.xPosition = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>().position.x;
        SaveData.current.yPosition = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>().position.y;
        SaveData.current.scene = SceneManager.GetActiveScene().buildIndex; 
        SerializationManager.Save(SaveData.current.saveName, SaveData.current);
    }
    public void LoadGamePanelClicked()
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
            gO.GetComponentInChildren<Button>().onClick.AddListener(() => LoadSave(nombre));

        }

        loadPanel.SetActive(true);
    }

    private  void LoadSave(string load)
    {
        LoadGame(load);
    }

    private  void LoadGame(string load)
    {
        SaveData.current = (SaveData)SerializationManager.Load(Application.persistentDataPath + "/saves/" + load + ".save");
       


     
         SceneManager.LoadScene(SaveData.current.scene);

        StartCoroutine("waitForSceneLoad", SaveData.current.scene);
        


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
    IEnumerator waitForSceneLoad(int sceneNumber)
    {
        yield return SceneManager.LoadSceneAsync(sceneNumber);

        LoadData();

           
        
     
       
    }

    private void LoadData()
    {
        Instantiate(GM.Instance.Player, GM.Instance.Player.transform.position, Quaternion.identity);

        GameObject Player = GameObject.FindGameObjectWithTag("Player");
        Player.GetComponent<Transform>().position = new Vector3(SaveData.current.xPosition, SaveData.current.yPosition);

        cinemachineVirtualCamera.Follow = Player.transform;
     

        CloseLoadMenuAndremoveChildren();
        gameObject.SetActive(false);
    }
}
