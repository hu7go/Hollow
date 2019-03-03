using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    PlayerHealth currentPlayer;
    EnemyAI enemyAI;
    GameObject corpseHandler;
    GameObject currencyHandler;
    AllLoot lootList;
    IEnemyType enemyType;

    [Space(20)]
    [Range(1, 10)]
    public int level = 1;
    [Space(20)]

    public GameObject deadState;
    public GameObject hitEffect;

    public GameObject healthPickup;

    public float health;
    public float movementSpeed;
    public int damage;
    [Header("The time it takes for the body to be destroyed and the dead decal to be spawned")]
    public float destructionTime = 1.5f;

    [Header("Min and Max amount of loot droped when enemy dies")]
    public int min = 0;
    public int max = 10;

    [Space(20)]
    public float lootForce = 200;

    [HideInInspector]
    public bool isDead = false;

    public void Start()
    {
        corpseHandler = GameObject.FindGameObjectWithTag("CorpseHolder");
        currencyHandler = GameObject.FindGameObjectWithTag("LootHolder");
        lootList = currencyHandler.GetComponent<AllLoot>();

        enemyAI = GetComponent<EnemyAI>();
        enemyType = GetComponent<IEnemyType>();
        currentPlayer = FindObjectOfType<PlayerHealth>();
    }

    public void TakeDamage(float damage, int direction)
    {
        GameObject tmpEffect = Instantiate(hitEffect, transform);
        Destroy(tmpEffect, 1f);
        health -= damage;
        enemyType.TakeDamage();
        CheckIfAlive(direction);
    }

    public void CheckIfAlive (int direction)
    {
        if (health <= 0)
        {
            Dead(direction);
        }
    }

    public void Dead (int direction)
    {
        isDead = true;
        enemyAI.sightEndT01.position = enemyAI.sightStartT.position;
        enemyAI.sightBehindT.position = enemyAI.sightStartT.position;
        int randomAmountOfLoot = Random.Range(min, max);

        int maxCurrencyDrop = level * 25;
        int currentCurrencyDrop = 0;

        for (int i = 0; i < randomAmountOfLoot; i++)
        {
            if (currentCurrencyDrop < maxCurrencyDrop)
            {
                GameObject newLoot = Instantiate(lootList.allCurrency[Random.Range(0, lootList.allCurrency.Length)], transform.position, transform.rotation, currencyHandler.transform);
                currentCurrencyDrop += newLoot.GetComponent<Currency>().currencyPrefab.currencyWorth;

                newLoot.GetComponent<Rigidbody2D>().AddForce(ForceVector(direction) + new Vector2(i * 3, i * 3));
            }
        }

        OtherDrop(direction);

        gameObject.layer = 9;

        Invoke("SpawnDeadState", destructionTime);
        Destroy(gameObject, destructionTime);
    }

    public void SpawnDeadState()
    {
        Instantiate(deadState, transform.position, transform.rotation, corpseHandler.transform);
    }

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

