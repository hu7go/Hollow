using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIInventory : MonoBehaviour
{
    GameController gameController;

    public InventorySlot[] inventorySlots;

    public Canvas inventoryUI;

    private bool paused = false;
    private int nmbrInventorySlots = 6;

    public void Start()
    {
        gameController = FindObjectOfType<GameController>();
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.I) || Input.GetButtonDown("Inventory"))
        {
            if (gameController.isPaused)
            {
                return;
            }

            if (!paused)
            {
                inventoryUI.gameObject.SetActive(true);
                gameController.Pause();
                gameController.inInventory = true;
                paused = true;
            }
            else
            {
                UnPause();
            }
        }
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetButtonDown("Menu") && gameController.inInventory)
        {
            UnPause();
        }
    }

    public void UnPause()
    {
        inventoryUI.gameObject.SetActive(false);
        gameController.UnPause();
        gameController.inInventory = false;
        paused = false;
    }

    public void AddWeapon(Weapon newWeapon)
    {
        //This will later drop the last another piece of equipment to pick up a new one
        if (nmbrInventorySlots <= 0)
            return;


        inventorySlots[nmbrInventorySlots -1].weapon = newWeapon;
        inventorySlots[nmbrInventorySlots -1].slotImage.sprite = newWeapon.image;
        inventorySlots[nmbrInventorySlots -1].slotImage.gameObject.SetActive(true);

        nmbrInventorySlots--;
    }

    public void AddArmor(Armor newArmor)
    {
        //This will later drop the last another piece of equipment to pick up a new one
        if (nmbrInventorySlots <= 0)
            return;


        inventorySlots[nmbrInventorySlots - 1].armor = newArmor;
        inventorySlots[nmbrInventorySlots - 1].slotImage.sprite = newArmor.image;
        inventorySlots[nmbrInventorySlots - 1].slotImage.gameObject.SetActive(true);

        nmbrInventorySlots--;
    }
}
