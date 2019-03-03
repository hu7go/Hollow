using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Stats : MonoBehaviour
{
    [Range(0, 20)]
    public int strength = 10;
    [Range(0, 20)]
    public int dexterity = 10;
    [Range(0, 20)]
    public int constitution = 10;
    [Range(0, 20)]
    public int intelligence = 10;
    [Range(0, 20)]
    public int wisdom = 10;
    [Range(0, 20)]
    public int charisma = 10;

    public int GetModyfier (int stat)
    {
        int modyfier = (stat -10) - (stat % 2);

        return modyfier;
    }
}
