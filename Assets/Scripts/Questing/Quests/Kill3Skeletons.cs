using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kill3Skeletons : Quest
{
    // Start is called before the first frame update
    void Start()
    {

        QuestName = "Kill le skeletons";
        QuestDescription = "You have to kill 3 skeletons";
        ExpReward = 15;
        GoldReward = 20;

        goals.Add(new KillGoal(this,0, "kill skels", false, 0, 3));
        foreach (var item in goals)
        {
            item.Init();
        }
    }


}
