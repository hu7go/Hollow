using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    public Image slotImage;
    public Weapon weapon;
    public Armor armor;

    Inventory player;

    public void Awake()
    {
        player = FindObjectOfType<Inventory>();
    }

    public void Equip()
    {
        player.Equip(armor, weapon);
    }
}
   
