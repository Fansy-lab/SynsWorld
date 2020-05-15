using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyButton : MonoBehaviour
{
    // Start is called before the first frame update
     Button thisButton;
    public KeyCode code;
    void Awake()
    {
        thisButton = GetComponent<Button>();

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(code))
        {
            thisButton.onClick.Invoke();
        }
    }
}
