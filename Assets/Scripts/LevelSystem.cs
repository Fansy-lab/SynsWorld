using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSystem: MonoBehaviour
{
    public static int currentLevel=1;
    public static int experience;
    public static int experienceForNextLevel;
    public static Levels levels = new Levels();

    public static Dictionary<int, int> levelsPerExp = levels.LevelPerExp;

    public static int totalExpForTheLevel;

    public static PlayerStats playerStats;

    private void Awake()
    {
        CalculateVariables();

    }

    public static void  AddExp(int AddedExp)
    {
        playerStats.playerData.experience += AddedExp;

        if ((experienceForNextLevel - AddedExp)<=0)
        {
            //level up
            currentLevel++;
            experienceForNextLevel = levelsPerExp[currentLevel];
            GlobalEvents.LeveledUp(experienceForNextLevel);

        }
        else
        {
            GlobalEvents.GainedExperience(AddedExp);
            experienceForNextLevel -=AddedExp;
        }

    }

    private void CalculateVariables()
    {
        currentLevel = 0;
        playerStats = GetComponent<PlayerStats>();
        experience = playerStats.playerData.experience;

        int totalExpRequired = 0;

        foreach (KeyValuePair<int, int> level in levelsPerExp)
        {
            int expRequiered = level.Value;
            totalExpRequired += expRequiered;
            int expLeft = experience - totalExpRequired;
            if (expLeft >= 0)
            {
                currentLevel++;
            }
            else
            {
                experienceForNextLevel = Mathf.Abs(expLeft);
                totalExpForTheLevel = level.Value;
                break;
            }
        }
    }
}
