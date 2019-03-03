using System;
using UnityEngine;

[Serializable]
public class Healthpotion : PotionEffect
{
    public int healing = 5;
    PlayerController myPlayer;

    public override void Effect(PlayerController player)
    {
        myPlayer = player;

        myPlayer.GetComponentInChildren<PlayerHealth>().Heal(healing);

        RemovePotion();
    }

    public override void RemovePotion()
    {
        myPlayer.UsedPotion();
    }
}
