using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Loot
{
    public PhysicalInventoryItem thisLoot;
    public float lootChance;
}

[CreateAssetMenu(fileName ="Loot",menuName ="Loots/Loot")]
public class LootTable : ScriptableObject
{
    public Loot[] loots;

    public PhysicalInventoryItem CumulativeLootItem()
    {
        float cumulativeProb = 0;
        float currentProb = UnityEngine.Random.Range(0.0f, 100f);
        for (int i = 0; i < loots.Length; i++)
        {
            cumulativeProb += loots[i].lootChance;
            if (currentProb <= cumulativeProb)
            {
                return loots[i].thisLoot;
            }
        }
        return null;
    }
    public PhysicalInventoryItem LootItem()
    {
        for (int i = 0; i < loots.Length; i++)
        {
            float currentProb = UnityEngine.Random.Range(0.0f, 100f);

            if (currentProb <= loots[i].lootChance)
            {
                return loots[i].thisLoot;
            }
        }
        return null;
    }

    internal PhysicalInventoryItem LootGold()
    {
        foreach (var item in loots)
        {
            if (item.thisLoot.thisItem.isCurrency)
            {
                float currentProb = UnityEngine.Random.Range(0.0f, 100f);

                if (currentProb <= item.lootChance)
                {
                    return item.thisLoot;
                }

            }
           
        }
        return null;
    }
}
