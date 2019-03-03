using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interact : MonoBehaviour
{
    //IO = interactable object
    [HideInInspector]
    public I_InteractableObject IO;
    PlayerArmor playerArmor;
    PlayerWeapon playerWeapon;
    Inventory inventory;

    public bool inRange = false;

    private void Start()
    {
        inventory = GetComponentInChildren<Inventory>();
        playerArmor = GetComponentInChildren<PlayerArmor>();
        playerWeapon = GetComponentInChildren<PlayerWeapon>();
    }

    void Update ()
    {
        if (Input.GetKeyDown(KeyCode.E) || Input.GetButtonDown("Interact"))
        {
            if (inRange)
            {
                IO.Interacted();
                //QuickEquip();
            }
        }
    }

    //public void QuickEquip ()
    //{
    //    if (playerWeapon.currentWeapon != inventory.storedWeapons[inventory.storedWeapons.Count - 1])
    //    {
    //        playerWeapon.currentWeapon = inventory.storedWeapons[inventory.storedWeapons.Count - 1];
    //        playerArmor.EquipArmor();
    //    }
    //    if (playerArmor.currentArmor != inventory.storedArmor[inventory.storedArmor.Count - 1])
    //    {
    //        playerArmor.currentArmor = inventory.storedArmor[inventory.storedArmor.Count -1];
    //        playerArmor.EquipArmor();
    //    }
    //}
}
