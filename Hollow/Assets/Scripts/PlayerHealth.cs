using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    Camera mainCamera;
    UIManager uiManager;
    PlayerController playerController;
    PlayerArmor playerArmor;
    AudioSource aS;

    [Space(20)]
    public int maxHealth = 10;
    public int startingHealth = 10;
    public int currentHealth;

    [Space(20)]
    [SerializeField] private AudioClip hit;
    [SerializeField] private AudioClip armorBreak;

    private int hitsTaken;

    public void Start()
    {
        aS = GetComponent<AudioSource>();

        if (uiManager == null)
            uiManager = FindObjectOfType<UIManager>();

        mainCamera = FindObjectOfType<Camera>();
        playerController = GetComponentInParent<PlayerController>();
        playerArmor = GetComponent<PlayerArmor>();

        //This might have to change
        if (GameManager.Instance != null)
        {
            if (GameManager.Instance.GetCurrentLevel() == 1)
                currentHealth = startingHealth;
        }
    }

    public void TakeDamage(int damage, bool isArmorPiercing)
    {
        if (currentHealth <= 0)
        {
            CheckIfAlive();
            return;
        }

        aS.pitch = Random.Range(0.9f, 1.1f);

        if (playerController.isBlocking == false)
        {
            aS.clip = hit;
            aS.Play();

            hitsTaken++;

            if (isArmorPiercing && playerArmor.currentArmorValue > 0)
            {
                aS.clip = armorBreak;
                aS.Play();

                currentHealth -= damage / 2;
                playerArmor.currentArmorValue -= damage / 2;
                HealthChange(damage / 2, true);
                uiManager.ArmorDamage(damage / 2);
            }
            else
            {
                damage -= playerArmor.currentArmorValue;
                if (damage <= 0)
                    damage = 1;

                currentHealth -= damage;
                HealthChange(damage, true);
            }
            mainCamera.GetComponent<MyPostProcessing>().enabled = true;
            Invoke("TurnOfAberration", .2f);
        }

        CheckIfAlive();
    }

    public void TurnOfAberration()
    {
        mainCamera.GetComponent<MyPostProcessing>().enabled = false;
    }

    public void HealthChange(int ammount, bool damage)
    {
        uiManager.HealthChange(ammount, damage);
    }

    public void CheckIfAlive()
    {
        if (currentHealth <= 0)
            GameManager.Instance.PlayerDied();
    }

    public int GetCurrentHealth ()
    {
        return currentHealth;
    }

    public void SetCurrentHealth (int newHealth)
    {
        uiManager = FindObjectOfType<UIManager>();
        currentHealth = newHealth;
        HealthChange(maxHealth - newHealth, true);
    }

    public int HitsTaken ()
    {
        return hitsTaken;
    }

    public void Heal (int healing)
    {
        currentHealth += healing;
        HealthChange(healing, false);
        if (currentHealth > maxHealth)
            currentHealth = maxHealth;
    }
}
