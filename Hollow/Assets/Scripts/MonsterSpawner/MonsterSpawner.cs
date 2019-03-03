using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    public GameObject player;

    public List<GameObject> enemyPrefabs;

    public float spawnCooldown;
    [Space(20)]

    private List<Transform> spawnPoints = new List<Transform>();
    private List<GameObject> currentEnemies = new List<GameObject>();

    public void SetList(List<Transform> list)
    {
        spawnPoints = list;
        SpawnCheck();
    }

    public void SpawnCheck()
    {
        if (GameManager.Instance != null)
        {
            if (GameManager.Instance.TotalEnemiesSpawned() <= GameManager.Instance.GetMaxNumberOfEnemies() && GameManager.Instance.stopSpawn == false)
            {
                Invoke("SpawnMonster", spawnCooldown);
            }
        }
    }

    public void SpawnMonster()
    {
        int randomEnemy = Random.Range(0, enemyPrefabs.Count);
        int randomPosition = Random.Range(0, spawnPoints.Count - 1);

        //This spawning should change according to how many types of monsters there is
        GameObject newEnemy = Instantiate(enemyPrefabs[randomEnemy], spawnPoints[randomPosition].position, transform.rotation, transform);

        //If the enemy spawned level cap is higher then the current level.
        if (newEnemy.GetComponent<EnemyStats>().firstLevel > GameManager.Instance.GetCurrentLevel())
        {
            Destroy(newEnemy);
            SpawnMonster();
            return;
        }

        GameManager.Instance.AddEnemy();
        currentEnemies.Add(newEnemy);
        SpawnCheck();
    }

    public List<GameObject> SendList ()
    {
        return currentEnemies;
    }
}
