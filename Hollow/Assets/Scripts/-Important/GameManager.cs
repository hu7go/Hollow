using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Audio;

public class GameManager : MonoBehaviour
{
    public const float globalScale = 0.48f;

    private PlayerStruct currentPlayer;

    [SerializeField] private int level = 1;

    public GameObject playerOne;

    [SerializeField] private string playerName;

    public GameObject playerTwo;
    private Vector3 deathPoint;
    private int totalMoney;
    [Header("How many enemies that spawn on the first level")]
    [SerializeField] private int maxNumberOfEnemies;
    [SerializeField] private int currentNumberOfEnemies;
    [SerializeField] private int totalEnemiesSpawned;

    [Space(20)]
    [SerializeField] private GameObject endScreenPrefab;

    private int tmp;

    private bool zoom = false;
    public bool hasDied = false;
    public bool stopSpawn = false;

    private Camera currentCam;
    private bool startMusic = true;

    //End game statistics
    private int totalEnemiesKilled;
    private int totalAttacks;
    private int attacksHit;
    private int totalJumps;
    private int endMoney;
    private int hitsTaken;
    private int hitsBlocked;
    //

    //TODO: try to make this a inheritable class so this is not on the game managers script
    private static bool created = false;

    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        tmp = maxNumberOfEnemies;
        hasDied = false;

        if (Instance == null)
            Instance = this;
        else if (Instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);

        if (!created)
        {
            created = true;
        }
    }
    //

    public void SaveName (string name)
    {
        playerName = name.ToLower();
    }

    public string GetName ()
    {
        return playerName;
    }

    public void SetPlayer (GameObject newPlayer)
    {
        currentCam = Camera.main;

        playerOne = newPlayer;

        if (playerOne != null)
            playerOne.GetComponent<PlayerController>().StartScene();
    }

    public int GetCurrentLevel ()
    {
        return level;
    }

    public void SetCurrentLevel ()
    {
        level++;
    }

    public int GetMoney()
    {
        return totalMoney;
    }

    public void SetMoney(int newMoney)
    {
        totalMoney += newMoney;
    }

    public void StartMusic ()
    {
        if (startMusic)
        {
            GetComponent<AudioSource>().Play();
            startMusic = false;
        }
    }

    public void StartCombat ()
    {
        currentCam.GetComponent<Animator>().SetBool("Combat", true);
    }

    public void StopCombat ()
    {
        currentCam.GetComponent<Animator>().SetBool("Combat", false);
    }

    public void StartNextLevel ()
    {
        //hasDied = false;
        if (playerOne != null)
        {
            AddStats();
            playerOne.GetComponent<PlayerController>().QuitScene();
        }

        SetCurrentLevel();
        if (GetCurrentLevel() % 3 == 0)
        {
            SceneManager.LoadScene("ShopRoom");
            return;
        }
        
        maxNumberOfEnemies = tmp + level;
        currentNumberOfEnemies = 0;
        totalEnemiesSpawned = 0;

        SceneManager.LoadScene("LoadingScreen");
    }

    public void ExitToMain()
    {
        GetComponent<AudioSource>().Stop();
        startMusic = true;

        //If the name of the main menu scene changes this must be changed accordingly to!
        SceneManager.LoadScene("MainMenu");
        Time.timeScale = 1;
        hasDied = false;
        stopSpawn = false;
        ResetManager();
    }

    public void ToggleHints (Image hints, bool toggle)
    {
        hints.enabled = toggle;
    }

    public void SavePlayer (PlayerStruct newStats)
    {
        currentPlayer = newStats;
    }

    public PlayerStruct LoadPlayer ()
    {
        return currentPlayer;
    }

    public int GetMaxNumberOfEnemies ()
    {
        return maxNumberOfEnemies;
    }

    public void AddEnemy ()
    {
        currentNumberOfEnemies++;
        totalEnemiesSpawned++;
    }

    public void EnemyDied ()
    {
        currentNumberOfEnemies--;
        totalEnemiesKilled++;
    }

    public int GetCurrentNmbrOfEnemies ()
    {
        return currentNumberOfEnemies;
    }

    public int TotalEnemiesSpawned ()
    {
        return totalEnemiesSpawned;
    }

    public void PlayerDied()
    {
        if (hasDied)
            return;

        hasDied = true;

        AddStats();

        currentCam = Camera.main;

        deathPoint = playerOne.transform.position + new Vector3(0, 0, -10);

        Time.timeScale = .55f;

        currentCam.GetComponent<CameraFollow>().enabled = false;

        playerOne.GetComponent<SpriteRenderer>().enabled = false;
        playerOne.GetComponent<PlayerController>().enabled = false;
        playerOne.GetComponent<Movement>().enabled = false;

        Instantiate(endScreenPrefab);

        StartCoroutine(PlayerDiedZoom());

        MonsterSpawner mSpawner = FindObjectOfType<MonsterSpawner>();

        FindObjectOfType<EventSystem>().SetSelectedGameObject(FindObjectOfType<GameOver>().quitButton);

        stopSpawn = true;
        List<GameObject> enemies = mSpawner.SendList();
        foreach (var enemie in enemies)
        {
            enemie.GetComponentInChildren<EnemyDetection>().enabled = false;
            enemie.GetComponent<EnemyAI>().enabled = false;
            enemie.GetComponentInChildren<EnemyAttack>().isAttacking = false;
            enemie.GetComponentInChildren<EnemyAttack>().enabled = false;
        }
    }

    private IEnumerator PlayerDiedZoom ()
    {
        while (currentCam.transform.position != deathPoint)
        {
            currentCam.transform.position = Vector2.Lerp(currentCam.transform.position, deathPoint, Time.deltaTime * 1f);
            currentCam.transform.position += new Vector3(0, 0, -1);

            if (currentCam.orthographicSize > 2)
                currentCam.orthographicSize -= 0.02f;
            
            yield return null;
        }
    }

    private void ResetManager ()
    {
        level = 1;
        currentPlayer = new PlayerStruct();
        currentNumberOfEnemies = 0;
        totalEnemiesSpawned = 0;
        totalEnemiesKilled = 0;
        totalAttacks = 0;
        attacksHit = 0;
        totalJumps = 0;
        hitsTaken = 0;
        hitsBlocked = 0;
        endMoney = 0;
    }

    ///Stats for the end game///

    public int TotalEnemiesKilled ()
    {
        return totalEnemiesKilled;
    }

    public void AddStats ()
    {
        totalAttacks += playerOne.GetComponent<PlayerController>().TotalAttacks();
        attacksHit += playerOne.GetComponent<PlayerController>().AttacksHit();
        totalJumps += playerOne.GetComponent<Movement>().TotalJumps();
        hitsTaken += playerOne.GetComponentInChildren<PlayerHealth>().HitsTaken();
        hitsBlocked += playerOne.GetComponent<PlayerController>().HitsBlocked();
        endMoney += playerOne.GetComponentInChildren<CoinHandler>().TotalMoney();
    }

    public int TotalAttacks ()
    {
        return totalAttacks;
    }

    public int AttacksHit ()
    {
        return attacksHit;
    }

    public int TotalJumps ()
    {
        return totalJumps;
    }

    public int HitsTaken ()
    {
        return hitsTaken;
    }

    public int HitsBlocked ()
    {
        return hitsBlocked;
    }

    public int TotalMoney ()
    {
        return endMoney;
    }

    ///
}
