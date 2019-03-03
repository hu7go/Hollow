using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemyType
{
    //The different animation states the all enemies need to have.
    void Walking();
    void StopWalking();
    void Attacking();
    void TakeDamage();
    void Die();
}
