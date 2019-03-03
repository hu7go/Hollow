using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyStats : MonoBehaviour
{
    //Get all needed references
    PlayerHealth currentPlayer;
    EnemyAI enemyAI;
    GameObject corpseHandler;
    GameObject currencyHandler;
    AllLoot lootList;
    IEnemyType enemyType;

    //The level of the enemy
    [Space(20)]
    [SerializeField] public int firstLevel;
    [Range(1, 10)]
    [SerializeField] private int level = 1;
    [SerializeField] private int damagePerLevel = 1;
    [SerializeField] private int hpPerLevel = 1;
    [SerializeField] private int goldPerLevel = 10;
    [Space(20)]

    //All kinds of variables
    [SerializeField] private GameObject deadState;
    [SerializeField] private GameObject hitEffect;
    [SerializeField] private GameObject healthPickup;

    [SerializeField] protected float health;
    public float movementSpeed;
    public int damage;
    [Header("The time it takes for the body to be destroyed and the dead decal to be spawned")]
    [SerializeField] private float destructionTime = 1.5f;

    [Header("Min and Max amount of loot droped when enemy dies")]
    [SerializeField] private int min = 0;
    [SerializeField] private int max = 10;

    [Space(20)]
    [SerializeField] private float lootForce = 200;
    [SerializeField] private bool dropOther = false;

    [HideInInspector]
    public bool isDead = false;

    private int hitDirection = 0;

    AudioSource aS;
    [Space(20)]
    [SerializeField] private AudioClip hit;
    
    //Set all references
    public virtual void Start()
    {
        aS = GetComponent<AudioSource>();

        corpseHandler = GameObject.FindGameObjectWithTag("CorpseHolder");
        currencyHandler = GameObject.FindGameObjectWithTag("LootHolder");
        lootList = currencyHandler.GetComponent<AllLoot>();

        enemyAI = GetComponent<EnemyAI>();
        enemyType = GetComponent<IEnemyType>();
        currentPlayer = FindObjectOfType<PlayerHealth>();

        SetLevel();
        damage += (damagePerLevel * level) - 1;
        health += (hpPerLevel * level) - 1;
    }

    private void SetLevel ()
    {
        if (GameManager.Instance != null)
            level = GameManager.Instance.GetCurrentLevel();
        else
            level = 1;
    }

    //This function is called when this enemy takes damage
    public void TakeDamage(float damage, int direction)
    {
        if (isDead)
            return;

        aS.pitch = Random.Range(0.9f, 1.1f);
        aS.clip = hit;
        aS.Play();

        GameObject tmpEffect = Instantiate(hitEffect, transform);
        Destroy(tmpEffect, 1f);
        health -= damage;
        enemyType.TakeDamage();
        hitDirection = direction;
        CheckIfAlive();
    }

    public void CheckIfAlive ()
    {
        if (health <= 0)
        {
            Dead();
        }
    }

    public void Dead ()
    {
        GameManager.Instance.EnemyDied();

        isDead = true;

        DropLoot();

        gameObject.layer = 9;

        enemyAI.StopMoving();

        Invoke("SpawnDeadState", destructionTime);
        Destroy(gameObject, destructionTime);
    }

    public void SpawnDeadState()
    {
        Instantiate(deadState, transform.position, transform.rotation, corpseHandler.transform);
    }

    //The loot drop is based on some variables and the level of the enemy
    public void DropLoot ()
    {
        int randomAmountOfLoot = Random.Range(min, max);

        int maxCurrencyDrop = randomAmountOfLoot + (10 * (level - 1));
        int currentCurrencyDrop = 0;

        for (int i = 0; i < randomAmountOfLoot; i++)
        {
            if (currentCurrencyDrop < maxCurrencyDrop)
            {
                GameObject newLoot = Instantiate(lootList.allCurrency[Random.Range(0, lootList.allCurrency.Length)], transform.position, transform.rotation, currencyHandler.transform);
                currentCurrencyDrop += newLoot.GetComponent<Currency>().currencyPrefab.currencyWorth;

                newLoot.GetComponent<Rigidbody2D>().AddForce(ForceVector(hitDirection) + new Vector2(i * 3, i * 3));
            }
        }

        if (dropOther)
            OtherDrop(hitDirection);
    }

    //Other drop is in this case health pickups
    public void OtherDrop(int direction)
    {
        if (currentPlayer.currentHealth < currentPlayer.maxHealth)
        {
            int tmpNumber = Random.Range(0, 10);
            if (tmpNumber < (10 - currentPlayer.currentHealth))
            {
                GameObject tmp = Instantiate(healthPickup, transform.position, transform.rotation, currencyHandler.transform);
                tmp.GetComponent<Rigidbody2D>().AddForce(ForceVector(direction));
            }
        }
    }

    //A calculation for a vector that the loot is shot out in
    Vector2 ForceVector (int direction)
    {
        float forceOffsetX = Random.Range(-40, 40);
        float forceOffsetY = Random.Range(-40, 40);

        float forceX = forceOffsetX + lootForce;
        float forceY = forceOffsetY + lootForce;

        Vector2 force = new Vector2(direction * forceX, forceY);

        return force;
    }
}

