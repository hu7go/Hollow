using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    public GameObject player;

    public List<GameObject> enemies;

    public int maxNumberOfEnemies;
    public float spawnCooldown;
    [Space(20)]
    public int currentNumberOfMonsters = 0;

    public void Start()
    {
        SpawnCheck();
    }

    public void SpawnCheck()
    {
        if (currentNumberOfMonsters < maxNumberOfEnemies && (transform.position - player.transform.position).sqrMagnitude > 20)
        {
            Invoke("SpawnMonster", spawnCooldown);
        }
    }

    public void SpawnMonster()
    {
        int tmpNumber = Random.Range(0, enemies.Count);

        //This spawning should change according to how many types of monsters there is
        GameObject newEnemy = Instantiate(enemies[tmpNumber], transform.position, transform.rotation, transform);
        currentNumberOfMonsters++;
        SpawnCheck();
    }
}
