using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Shop : MonoBehaviour
{
    [SerializeField] private GameObject armor;
    [SerializeField] private GameObject weapon;
    [SerializeField] private GameObject potion;
    [SerializeField] private GameObject shopSlotArmor;
    [SerializeField] private GameObject shopSlotWeapon;
    [SerializeField] private GameObject shopSlotPotion;
    [SerializeField] private Text armorTextCost;
    [SerializeField] private Text armorTextStats;
    [SerializeField] private Text armorTextName;
    [SerializeField] private Text weaponTextCost;
    [SerializeField] private Text weaponTextStats;
    [SerializeField] private Text weaponTextName;
    [SerializeField] private Text potionTextCost;
    [SerializeField] private Text potionTextStats;
    [SerializeField] private Text potionTextName;
    [SerializeField] private Text playerMoney;

    [SerializeField] private float xPos = 600f;
    [SerializeField] private float deactivatedXPos = 1500f;

    [Space(20)]
    [SerializeField] private GameObject playerArmor;
    [SerializeField] private GameObject playerWeapon;
    [SerializeField] private GameObject playerPotion;
    [SerializeField] private GameObject playerShopSlotArmor;
    [SerializeField] private GameObject playerShopSlotWeapon;
    [SerializeField] private GameObject playerShopSlotPotion;
    [SerializeField] private Text playerArmorTextStats;
    [SerializeField] private Text playerArmorTextName;
    [SerializeField] private Text playerWeaponTextStats;
    [SerializeField] private Text playerWeaponTextName;
    [SerializeField] private Text playerPotionTextStats;
    [SerializeField] private Text playerPotionTextName;

    [SerializeField] private float xPosPlayer = -640;
    [SerializeField] private float deactivatedXPosPlayer = -1600;

    private int currentOption = 0;

    [Space(20)]
    [SerializeField] private GameObject continueButton;

    //all the armor and weapons the player can purchase through out the game
    public List<Armor> availableArmors;
    public List<Weapon> availableWeapons;
    public List<Potion> availablePotions;

    [SerializeField] List<Armor> currentArmors;
    [SerializeField] List<Weapon> currentWeapons;
    [SerializeField] List<Potion> currentPotions;

    [Space(20)]
    [SerializeField] private Button left;
    [SerializeField] private Button right;
    [SerializeField] private Button armorButton;
    [SerializeField] private Button weaponButton;
    [SerializeField] private Button potionButton;

    Armor purchaseableArmor;
    Weapon purchaseableWeapon;
    Potion purchaseablePotion;
    AudioSource aS;

    PlayerStruct playerOne;

    EventSystem es;

    Navigation nvgL;
    Navigation nvgR;

    int randomWeapon;
    int randomArmor;
    int randomPotion;

    private bool boughtArmor = false;
    private bool boughtWeapon = false;
    private bool boughtPotion = false;

    public void Start()
    {
        aS = GetComponent<AudioSource>();
        es = FindObjectOfType<EventSystem>();
        es.SetSelectedGameObject(continueButton);
        currentArmors = new List<Armor>();
        currentWeapons = new List<Weapon>();
        currentPotions = new List<Potion>();

        nvgL = left.navigation;
        nvgR = right.navigation;

        GetPlayer();
        GetWhichItems();
        DisplayItems();
        UpdateText();
    }

    //Get what the player has for stuff currently
    public void GetPlayer ()
    {
        playerOne = GameManager.Instance.LoadPlayer();
    }

    //Get which items the player can buy currently
    public void GetWhichItems ()
    {
        foreach (Armor armor in availableArmors)
        {
            if (armor.cost < playerOne.currentMoney)
            {
                currentArmors.Add(armor);
            }
        }
        randomArmor = Random.Range(0, currentArmors.Count);

        foreach (Weapon weapon in availableWeapons)
        {
            if (weapon.cost < playerOne.currentMoney)
            {
                currentWeapons.Add(weapon);
            }
        }
        randomWeapon = Random.Range(0, currentWeapons.Count);

        foreach (Potion potion in availablePotions)
        {
            if (potion.cost < playerOne.currentMoney)
            {
                currentPotions.Add(potion);
            }
        }
        randomPotion = Random.Range(0, currentPotions.Count);

        if (currentArmors.Count >= 1)
            purchaseableArmor = currentArmors[randomArmor];

        if (currentWeapons.Count >= 1)
            purchaseableWeapon = currentWeapons[randomWeapon];

        if (currentPotions.Count >= 1)
            purchaseablePotion = currentPotions[randomPotion];
    }

    //TODO: A state for if the player does not have enough money for any equipment!
    public void DisplayItems ()
    {
        if (purchaseableArmor != null)
        {
            shopSlotArmor.GetComponent<Image>().sprite = purchaseableArmor.image;
            shopSlotArmor.GetComponent<Image>().color = Color.white;

            armorTextCost.text = "Cost: " + purchaseableArmor.cost.ToString();
            armorTextStats.text = "Armor value: " + purchaseableArmor.armorValue.ToString();
            armorTextName.text = purchaseableArmor.itemName;
        }
        else
        {
            armorTextCost.text = "Cost: 0";
            armorTextStats.text = "No armor availible.";
            armorTextName.text = "";
        }

        if (purchaseableWeapon != null)
        {
            shopSlotWeapon.GetComponent<Image>().sprite = purchaseableWeapon.image;
            shopSlotWeapon.GetComponent<Image>().color = Color.white;

            weaponTextCost.text = "Cost: " + purchaseableWeapon.cost.ToString();
            weaponTextStats.text = "Damage light attack: " + purchaseableWeapon.damage.ToString() + "\n" +
                "Damage heavy attack: " + purchaseableWeapon.heavyAttackDamage.ToString();
            weaponTextName.text = purchaseableWeapon.itemName;
        }
        else
        {
            weaponTextCost.text = "Cost: 0";
            weaponTextStats.text = "No weapon availible.";
            weaponTextName.text = "";
        }

        if (purchaseablePotion != null)
        {
            shopSlotPotion.GetComponent<Image>().sprite = purchaseablePotion.image;
            shopSlotPotion.GetComponent<Image>().color = Color.white;

            potionTextCost.text = "Cost: " + purchaseablePotion.cost.ToString();
            potionTextStats.text = purchaseablePotion.description;
            potionTextName.text = purchaseablePotion.itemName;
        }
        else
        {
            potionTextCost.text = "Cost: 0";
            potionTextStats.text = "No potion availible.";
            potionTextName.text = "";
        }

        DisplayPlayerItems();
    }

    //Displays what the player currently has for items!
    private void DisplayPlayerItems ()
    {
        ///Player Armor
        playerShopSlotArmor.GetComponent<Image>().sprite = playerOne.currentArmor.image;
        playerShopSlotArmor.GetComponent<Image>().color = Color.white;

        playerArmorTextStats.text = "Armor value: " + playerOne.currentArmor.armorValue.ToString();
        playerArmorTextName.text = playerOne.currentArmor.itemName;
        ///

        ///Player Weapon
        playerShopSlotWeapon.GetComponent<Image>().sprite = playerOne.currentWeapon.image;
        playerShopSlotWeapon.GetComponent<Image>().color = Color.white;

        playerWeaponTextStats.text = "Damage light attack: " + playerOne.currentWeapon.damage.ToString() + "\n" +
            "Damage heavy attack: " + playerOne.currentWeapon.heavyAttackDamage.ToString();
        playerWeaponTextName.text = playerOne.currentWeapon.itemName;
        ///

        if (playerOne.currentPotion != null)
        {
            ///Player Potion
            playerShopSlotPotion.GetComponent<Image>().sprite = playerOne.currentPotion.image;
            playerShopSlotPotion.GetComponent<Image>().color = Color.white;

            playerPotionTextStats.text = playerOne.currentPotion.description;
            playerPotionTextName.text = playerOne.currentPotion.itemName;
            ///
        }
    }

    public void UpdateText ()
    {
        playerMoney.text = "Money: " + playerOne.currentMoney.ToString();
    }

    public void BuyArmor ()
    {
        if (playerOne.currentMoney >= purchaseableArmor.cost && boughtArmor == false)
        {
            playerOne.currentArmor = purchaseableArmor;
            playerOne.currentArmorValue = purchaseableArmor.armorValue;
            playerOne.currentMoney -= purchaseableArmor.cost;
            UpdatePlayer();
            UpdateText();
            boughtArmor = true;

            DisplayPlayerItems();

            aS.pitch = Random.Range(0.8f, 1.2f);
            aS.Play();
        }
        else
        {
            //TODO: this happens if you dont have enough money!
        }
    }

    public void BuyWeapon ()
    {
        if (playerOne.currentMoney >= purchaseableWeapon.cost && boughtWeapon == false)
        {
            playerOne.currentWeapon = purchaseableWeapon;
            playerOne.currentMoney -= purchaseableWeapon.cost;
            UpdatePlayer();
            UpdateText();
            boughtWeapon = true;

            DisplayPlayerItems();

            aS.pitch = Random.Range(0.8f, 1.2f);
            aS.Play();
        }
        else
        {
            //TODO: Same as in BuyArmor().
        }
    }

    public void BuyPotion ()
    {
        if (playerOne.currentMoney >= purchaseablePotion.cost && boughtPotion == false)
        {
            playerOne.currentPotion = purchaseablePotion;
            playerOne.currentMoney -= purchaseablePotion.cost;
            UpdatePlayer();
            UpdateText();
            boughtPotion = true;

            DisplayPlayerItems();

            aS.pitch = Random.Range(0.8f, 1.2f);
            aS.Play();
        }
        else
        {
            //TODO: same as in the weapon and armor
        }
    }

    public void UpdatePlayer ()
    {
        GameManager.Instance.SavePlayer(playerOne);
    }

    public void Continue ()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.StartNextLevel();
        else
            Debug.Log("No game manager is currently in this scene");
    }

    public void Left ()
    {
        currentOption--;
        ChangeShop();
    }

    public void Right ()
    {
        currentOption++;
        ChangeShop();
    }

    private void ChangeShop ()
    {
        if (currentOption > 2)
            currentOption = 0;

        if (currentOption < 0)
            currentOption = 2;

        ///Armor is visible!
        if (currentOption == 0)
        {
            armor.GetComponent<RectTransform>().localPosition = new Vector3(xPos, 240, 0);
            playerArmor.GetComponent<RectTransform>().localPosition = new Vector3(xPosPlayer, 240, 0);

            nvgL.selectOnRight = armorButton;
            nvgR.selectOnLeft = armorButton;

            weapon.GetComponent<RectTransform>().localPosition = new Vector3(deactivatedXPos, 240, 0);
            potion.GetComponent<RectTransform>().localPosition = new Vector3(deactivatedXPos, 240, 0);
            playerWeapon.GetComponent<RectTransform>().localPosition = new Vector3(deactivatedXPosPlayer, 240, 0);
            playerPotion.GetComponent<RectTransform>().localPosition = new Vector3(deactivatedXPosPlayer, 240, 0);
        }
        ///Weapon is visible!
        if (currentOption == 1)
        {
            weapon.GetComponent<RectTransform>().localPosition = new Vector3(xPos, 240, 0);
            playerWeapon.GetComponent<RectTransform>().localPosition = new Vector3(xPosPlayer, 240, 0);

            nvgL.selectOnRight = weaponButton;
            nvgR.selectOnLeft = weaponButton;

            armor.GetComponent<RectTransform>().localPosition = new Vector3(deactivatedXPos, 240, 0);
            potion.GetComponent<RectTransform>().localPosition = new Vector3(deactivatedXPos, 240, 0);
            playerArmor.GetComponent<RectTransform>().localPosition = new Vector3(deactivatedXPos, 240, 0);
            playerPotion.GetComponent<RectTransform>().localPosition = new Vector3(deactivatedXPosPlayer, 240, 0);
        }
        ///Potion is visible!
        if (currentOption == 2)
        {
            potion.GetComponent<RectTransform>().localPosition = new Vector3(xPos, 240, 0);
            playerPotion.GetComponent<RectTransform>().localPosition = new Vector3(xPosPlayer, 240, 0);

            nvgL.selectOnRight = potionButton;
            nvgR.selectOnLeft = potionButton;

            weapon.GetComponent<RectTransform>().localPosition = new Vector3(deactivatedXPos, 240, 0);
            armor.GetComponent<RectTransform>().localPosition = new Vector3(deactivatedXPos, 240, 0);
            playerWeapon.GetComponent<RectTransform>().localPosition = new Vector3(deactivatedXPosPlayer, 240, 0);
            playerArmor.GetComponent<RectTransform>().localPosition = new Vector3(deactivatedXPos, 240, 0);
        }

        left.navigation = nvgL;
        right.navigation = nvgR;
    }
}
