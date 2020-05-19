using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RNGGod 
{
   

    public static EquipableWeaponryStats GetRandonWeaponStats()
    {
        EquipableWeaponryStats toReturn = new EquipableWeaponryStats();
        toReturn.AttackMinDamage = UnityEngine.Random.Range(3, 7);
        toReturn.AttackMaxDamage = UnityEngine.Random.Range(7, 20);
        toReturn.AttackSpeed = UnityEngine.Random.Range(5, 10);

        return toReturn;
    }

    public static EquipableArmoryStats GetRandomArmoryStats()
    {
        EquipableArmoryStats toReturn = new EquipableArmoryStats();

        toReturn.ArmorAmmount = UnityEngine.Random.Range(1, 15);
        toReturn.HealthAmmount = UnityEngine.Random.Range(5, 20);
        toReturn.EvasionAmmount = UnityEngine.Random.Range(1, 10);

        return toReturn;
    }

    internal static int GetGoldAmmount()
    {
        return UnityEngine.Random.Range(1, 5);
    }

    internal static float GetRandomPitch()
    {
        return UnityEngine.Random.Range(0.9f, 1.1f); 
    }
}
