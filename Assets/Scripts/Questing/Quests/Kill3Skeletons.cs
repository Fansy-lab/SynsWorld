using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kill3Skeletons : Quest
{
    // Start is called before the first frame update
    void Start()
    {
        QuestID = 1;
        QuestName = "Kill le skeletons";
        QuestDescription = "You have to kill 3 skeletons";
        ExpReward = 15;
        GoldReward = 20;

        goals.Add(new KillGoal(this,1, "Kill 3 skeletons, it's all you have to do!", false, 0, 3));
        foreach (var item in goals)
        {
            item.Init();
        }
    }


}
