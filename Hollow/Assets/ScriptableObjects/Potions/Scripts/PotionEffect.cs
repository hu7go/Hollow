using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PotionEffect : MonoBehaviour
{
    abstract public void Effect(PlayerController player);

    abstract public void RemovePotion();
}
