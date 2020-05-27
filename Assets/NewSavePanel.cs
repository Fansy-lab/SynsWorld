using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NewSavePanel : MonoBehaviour
{
    // Start is called before the first frame update
    public TMP_InputField InputField;
    public StartMenu startMenu;
    public Button buttonSave;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (InputField.text != "")
            {
                SaveAndStartGame();

            }
        }
    }

    public void SaveAndStart()
    {
        if (InputField.text != "")
        {
            SaveAndStartGame();

        }
    }

    private void SaveAndStartGame()
    {
        startMenu.CreateSaveAndLoadThatNewSave(InputField.text);
        buttonSave.interactable = true;
        gameObject.SetActive(false);
    }
}
