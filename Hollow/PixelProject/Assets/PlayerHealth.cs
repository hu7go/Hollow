using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public UIManager uiManager;
    playerController playerController;
    PlayerArmor playerArmor;

    [Space(20)]
    public int maxHealth = 10;
    public int startingHealth = 10;
    public int currentHealth;

    public void Start()
    {
        playerController = GetComponentInParent<playerController>();
        playerArmor = GetComponent<PlayerArmor>();
        currentHealth = startingHealth;
    }

    public void TakeDamage(int damage, bool isArmorPiercing)
    {
        if (playerController.isBlocking == false)
        {
            if (isArmorPiercing && playerArmor.currentArmorValue > 0)
            {
                currentHealth -= damage / 2;
                playerArmor.currentArmorValue -= damage / 2;
                HealthChange(damage / 2, true);
                uiManager.ArmorDamage(damage / 2);
            }
            else
            {
                damage -= playerArmor.currentArmorValue;
                if (damage < 0)
                {
                    damage = 1;
                }
                currentHealth -= damage;
                HealthChange(damage, true);
            }
            CheckIfAlive();
        }
    }

    public void HealthChange(int ammount, bool damage)
    {
        uiManager.HealthChange(ammount, damage);
    }

    public void CheckIfAlive()
    {
        if (currentHealth <= 0)
        {
            Destroy(this.gameObject);
        }
    }
}
