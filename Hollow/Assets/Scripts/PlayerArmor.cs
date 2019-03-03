using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerArmor : MonoBehaviour
{
    UIManager uiManager;
    public Armor currentArmor;
    PlayerController playerController;

    public int maxArmorValue = 0;
    public int currentArmorValue = 0;

	void Start ()
    {
        if (uiManager == null)
            uiManager = FindObjectOfType<UIManager>();

        playerController = GetComponentInParent<PlayerController>();
        if (GameManager.Instance.GetCurrentLevel() == 1)
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

    public int SaveArmor ()
    {
        return currentArmorValue;
    }

    public void LoadArmor (int newArmorValue)
    {
        uiManager = FindObjectOfType<UIManager>();
        EquipArmor();
        //uiManager.SetArmor(currentArmorValue);
        currentArmorValue = newArmorValue;
        uiManager.ArmorDamage(currentArmor.armorValue - newArmorValue);
    }
}
