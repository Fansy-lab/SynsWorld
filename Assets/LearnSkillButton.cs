using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LearnSkillButton : MonoBehaviour
{
    // Start is called before the first frame update
    public TextMeshProUGUI numberText;
    PlayerStats playerStats;
    int currentLevelOfSkill = 0;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }


    public void LevelUpSkill()
    {
        if(playerStats == null)
        {
            playerStats = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>();
        }

        if (playerStats)
        {
            if (playerStats.skillPointsToSpend > 0)
            {
                currentLevelOfSkill += 1;
                numberText.text = currentLevelOfSkill + "";
                playerStats.skillPointsToSpend--;
            }
        }


    }
}
