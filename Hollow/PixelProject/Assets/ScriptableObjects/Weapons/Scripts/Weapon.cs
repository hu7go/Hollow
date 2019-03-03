﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new Weapon", menuName = "Weapon")]
public class Weapon : ScriptableObject
{
    public int damage;
    public int heavyAttackDamage;
    public float damageForce;
    public Sprite image;
}
