using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RNGGod 
{
   

    public static EquipableWeaponryStats GetRandonWeaponStats()
    {
        EquipableWeaponryStats toReturn = new EquipableWeaponryStats();
        toReturn.Attack = Random.Range(5, 15);
        toReturn.AttackSpeed = Random.Range(5, 10);

        return toReturn;
    }

    public static EquipableArmoryStats GetRandomArmoryStats()
    {
        EquipableArmoryStats toReturn = new EquipableArmoryStats();

        toReturn.ArmorAmmount = Random.Range(0, 15);
        toReturn.HealthAmmount = Random.Range(5, 20);
        toReturn.EvasionAmmount = Random.Range(1, 10);

        return toReturn;
    }
}
