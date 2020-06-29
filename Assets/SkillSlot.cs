using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillSlot : MonoBehaviour
{
    // Start is called before the first frame update
    Button thisButton;
    public KeyCode code;
    void Start()
    {
        thisButton = GetComponent<Button>();

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(code))
        {
            thisButton.onClick.Invoke();
            //test comment delete whenever
        }
    }
}
