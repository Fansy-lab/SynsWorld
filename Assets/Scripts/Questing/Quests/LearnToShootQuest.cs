using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LearnToShootQuest : Quest
{

    void Start()
    {


        goals.Add(new KillGoal(this, 0, "Shoot down 2 dummies!", false,0, 2));
        foreach (var item in goals)
        {
            item.Init();
        }
    }

}
