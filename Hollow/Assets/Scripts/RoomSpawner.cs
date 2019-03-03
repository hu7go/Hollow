using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomSpawner : MonoBehaviour
{
    public bool spawnEnemies = true;
    public bool spawnChests = true;
    public bool spawnDoor = true;
    public bool spawnBats = false;

    [Space(20)]
    [Header("Player booleans")]
    public bool twoPlayers = false;
    public bool spawnPlayer = false;

    [Space(20)]

    public GameObject enemySpawnerPrefab;
    public GameObject SilverChest;
    public GameObject player01;
    public GameObject portal;
    public GameObject doorObj;
    public GameObject batPrefab;
    Transform parent;
    public int maxChests = 3;
    public int maxEnemySpawners = 1;
    public List<Transform> spawnPoints;

    private List<Vector3> ceilingSpawns;

    private GameObject enemySpawn;
    private GameObject objectParent;

    private GameObject player;

    public void Start()
    {
        parent = new GameObject("Parent").transform;
        objectParent = new GameObject("Object parent");
    }

    public void StartSpawning(List<Transform> tmp)
    {
        spawnPoints = tmp;
        ceilingSpawns = GetComponent<RoomGenerator>().CeilList();

        if (spawnEnemies)
            SpawnEnemySpawners();

        if (spawnChests)
            SpawnChests();

        if (spawnDoor)
            SpawnExit(tmp);

        if (spawnPlayer)
        {
            if (twoPlayers)
                SpawnPlayer(tmp);

            SpawnPlayer(tmp);
        }

        if (spawnBats)
            SpawnBats();
    }
	
    private void SpawnEnemySpawners()
    {
        enemySpawn = Instantiate(enemySpawnerPrefab);
        enemySpawn.GetComponent<MonsterSpawner>().SetList(spawnPoints);
    }

    private void SpawnChests()
    {
        for (int i = 0; i < maxChests - Random.Range(0, maxChests); i++)
        {
            SpawnFunction(SilverChest);
        }
    }

    private void SpawnExit (List<Transform> tmp)
    {
        Transform spawnPoint = tmp[tmp.Count - 1];
        Instantiate(doorObj, spawnPoint.position, spawnPoint.rotation, parent);
        //SpawnFunction(doorObj);
    }

    public void SpawnPlayer(List<Transform> tmp)
    {
        Transform spawnPoint = tmp[0];

        if (tmp[0] != null)
            spawnPoint = tmp[0];


        GameObject newPlayer = Instantiate(player01, spawnPoint.position + new Vector3(0, .5f, 0), spawnPoint.rotation, parent);
        player = newPlayer;
        StartCoroutine(Tmp());

        GameObject newPortal = Instantiate(portal, spawnPoint.position - new Vector3(0, .5f, 0), spawnPoint.rotation, parent);
        newPortal.GetComponent<Portal>().Close();

        CameraFollow.Instance.player = newPlayer.transform;

        if (GameManager.Instance != null)
            GameManager.Instance.SetPlayer(newPlayer);
        else
            Debug.Log("No Game manager");
    }

    public void SpawnBats ()
    {
        int min = 2;
        int max = 10;

        if (GameManager.Instance.GetName() == "batman")
        {
            min = 2000;
            max = 3000;
        }

        for (int i = 0; i < Random.Range(min, max); i++)
            Instantiate(batPrefab, ceilingSpawns[Random.Range(0, ceilingSpawns.Count - 1)], transform.rotation, parent);
    }

    public void SpawnFunction(GameObject whatToSpawn)
    {
        int randomNumber = Random.Range(0, spawnPoints.Count);
        Instantiate(whatToSpawn, spawnPoints[randomNumber].position, transform.rotation, parent);
        spawnPoints.Remove(spawnPoints[randomNumber]);
    }

    public List<Vector3> SendCeilingList ()
    {
        return ceilingSpawns;
    }

    private IEnumerator Tmp ()
    {
        yield return new WaitForSeconds(.2f);
        player.GetComponent<Rigidbody2D>().AddForce(new Vector2(800, 200));
    }
}
