using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnterLeaveLocationManager : MonoBehaviour
{
    public static EnterLeaveLocationManager instance;
    private static int m_referenceCount = 0;

    [SerializeField] TextMeshProUGUI textToDisplay;
    private void Awake()
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

    // Update is called once per frame
    
    public void ShowWhereYouEnter(string name)
    {
        textToDisplay.text = "Entering \r\n"+ name;
        Invoke("Appear", 1);
    }
    public void ShowWhereYouLeave(string name)
    {
        textToDisplay.text = "Leaving \r\n" + name;
        Invoke("Appear", 1);

    }
    void Appear()
    {
        textToDisplay.GetComponent<Animator>().SetTrigger("Appear");
    }
}
