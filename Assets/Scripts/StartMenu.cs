using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{
    public static StartMenu instance;
    private static int m_referenceCount = 0;

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

    public void NewGame()
    {

        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
        gameObject.SetActive(false);
    }
    public void QuitGame()
    {
        Application.Quit();
    }
    public void SaveGame()
    {

    }
    public void LoadGame()
    {
    }
}
