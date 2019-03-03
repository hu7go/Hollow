using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public GameObject armor;

    playerController playerController;
    public PlayerArmor currentArmor;
    public PlayerWeapon currentWeapon;

    public List<Armor> storedArmor;
    public List<Weapon> storedWeapons;

	void Start ()
    {
        playerController = GetComponentInParent<playerController>();
        currentArmor = armor.GetComponent<PlayerArmor>();
        currentWeapon = playerController.gameObject.GetComponentInChildren<PlayerWeapon>();

        storedArmor.Add(currentArmor.currentArmor);
        storedWeapons.Add(currentWeapon.currentWeapon);
	}

    public void AddToInventory(Armor newArmor, Weapon newWeapon)
    {
        if (newArmor != null)
        {
            storedArmor.Add(newArmor);
        }
        if (newWeapon != null)
        {
            storedWeapons.Add(newWeapon);
        }
    }
}
