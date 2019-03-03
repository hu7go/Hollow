using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerArmor : MonoBehaviour
{
    public UIManager uiManager;
    public Armor currentArmor;
    playerController playerController;

    public int maxArmorValue = 0;
    public int currentArmorValue = 0;

	void Start ()
    {
        playerController = GetComponentInParent<playerController>();
        EquipArmor();
    }

    public void EquipArmor()
    {
        //First equip a new armor, then send the stats
        currentArmorValue = currentArmor.armorValue;
        SendStats();
    }

    public void SendStats ()
    {
        uiManager.SetArmor(currentArmorValue);
    }
}
