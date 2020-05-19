using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="PlayerData",menuName ="Player/PlayerData")]
public class PlayerData : ScriptableObject
{
    public int DPS;

    public int maxHealth;
    public int armor;
    public int evasion;

    public int currentHealth;

    public int gold;
    public int experience;


}
