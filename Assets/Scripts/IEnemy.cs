using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemy
{
    int ID { get; set; }
    string Name { get; set; }
    int MaxHealth { get; set; }
    int Experience { get; set; }
    LootTable lootTable { get; set; }
    bool CanBeDamaged { get; set; }
    bool TakesReducedDamage { get; set; }
    void Die();
    void TakeDamage(int damage);
    void RegenHealthToMax();
    Spawner spawner { get; set; }
    AudioClip DoDamageSoundEffect { get; set; }
    AudioClip MissAttackSoundEffect { get; set; }



}
