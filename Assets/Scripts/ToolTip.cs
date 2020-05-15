using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class ToolTip : MonoBehaviour
{

    public TextMeshProUGUI tooltipText;
    public RectTransform backgroundRect;


    public void ShowTooltip(string text,Vector3 position)
    {
      
        tooltipText.text = text;
        backgroundRect.transform.parent.gameObject.SetActive(true);

        gameObject.transform.position = position;
    }
    public void HideTooltip()
    {
        tooltipText.text = "";
        backgroundRect.transform.parent.gameObject.SetActive(false);
    }

   
}
