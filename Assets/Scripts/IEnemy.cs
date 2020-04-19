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
    int GoldReward { get; set; }
    void Die();
    void TakeDamage(int damage);
 

}
