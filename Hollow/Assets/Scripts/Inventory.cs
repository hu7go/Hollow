using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
     UIInventory inventoryUI;

    public GameObject armor;

    PlayerController playerController;
    public PlayerArmor currentArmor;
    public PlayerWeapon currentWeapon;

    public List<Armor> storedArmor;
    public List<Weapon> storedWeapons;

	void Start ()
    {
        playerController = GetComponentInParent<PlayerController>();
        currentArmor = armor.GetComponent<PlayerArmor>();
        currentWeapon = playerController.gameObject.GetComponentInChildren<PlayerWeapon>();
        inventoryUI = FindObjectOfType<UIInventory>();

        storedArmor.Add(currentArmor.currentArmor);
        storedWeapons.Add(currentWeapon.currentWeapon);
	}

    public void AddToInventory(Armor newArmor, Weapon newWeapon)
    {
        if (newArmor != null)
        {
            storedArmor.Add(newArmor);
            inventoryUI.AddArmor(newArmor);
        }
        if (newWeapon != null)
        {
            storedWeapons.Add(newWeapon);
            inventoryUI.AddWeapon(newWeapon);
        }
    }

    public void Equip(Armor newArmor, Weapon newWeapon)
    {
        if (newArmor != null)
        {
            currentArmor.currentArmor = newArmor;
            currentArmor.EquipArmor();
        }
    }
}
