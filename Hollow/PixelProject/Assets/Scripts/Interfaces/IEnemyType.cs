using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemyType
{
    void Walking();
    void StopWalking();
    void Attacking();
    void TakeDamage();
}
