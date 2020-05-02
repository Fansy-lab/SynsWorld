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

    public PhysicalInventoryItem LootItem()
    {
        float cumulativeProb = 0;
        float currentProb = Random.Range(0.0f, 100f);
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
}
