using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RNGGod 
{
   

    public static EquipableWeaponryStats GetRandonWeaponStats()
    {
        EquipableWeaponryStats toReturn = new EquipableWeaponryStats();
        toReturn.Attack = UnityEngine.Random.Range(5, 15);
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
}
