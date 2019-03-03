using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomSpawner : MonoBehaviour
{
    public GameObject enemySpawner;
    public GameObject SilverChest;
    public GameObject player01;
    public int maxChests = 3;
    public int maxEnemySpawners = 1;
    public List<Transform> spawnPoints;

    public void SpawnPlayer (List<Transform> tmp)
    {
        /*
        spawnPoints = tmp;
        int randomNumber = Random.Range(0, spawnPoints.Count);
        Instantiate(player01, spawnPoints[randomNumber].position, transform.rotation);
        spawnPoints.Remove(spawnPoints[randomNumber]);
        */
    }

    public void StartSpawning(List<Transform> tmp)
    {
        spawnPoints = tmp;
        SpawnEnemySpawners();
        SpawnChests();
    }
	
    private void SpawnEnemySpawners()
    {
        for (int i = 0; i < maxEnemySpawners - Random.Range(0, (maxEnemySpawners - 2)); i++)
        {
            SpawnFunction(enemySpawner);
        }
    }

    private void SpawnChests()
    {
        for (int i = 0; i < maxChests - Random.Range(0, maxChests); i++)
        {
            SpawnFunction(SilverChest);
        }
    }

    public void SpawnFunction(GameObject whatToSpawn)
    {
        int randomNumber = Random.Range(0, spawnPoints.Count);
        Instantiate(whatToSpawn, spawnPoints[randomNumber].position, transform.rotation, transform);
        spawnPoints.Remove(spawnPoints[randomNumber]);
    }
}
