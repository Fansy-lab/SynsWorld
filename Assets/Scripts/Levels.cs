using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Levels
{
    public Dictionary<int, int> LevelPerExp = new Dictionary<int, int>();
    public Levels()
    {
        for (int i = 2; i < 101; i++)
        {
            LevelPerExp.Add(i, (2 / Convert.ToInt32(Mathf.Pow(i + 1, 2)) + 2 /Convert.ToInt32(Mathf.Pow(i + 1, 2))+ Convert.ToInt32(Mathf.Pow(i+1,2))));
        }

    }
}
