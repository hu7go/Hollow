using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{
    [SerializeField] private Text levels;
    [SerializeField] private Text monstersKilled;
    [SerializeField] private Text attacks;
    [SerializeField] private Text attacksHit;
    [SerializeField] private Text jumps;
    [SerializeField] private Text hitsTaken;
    [SerializeField] private Text hitsBlocked;
    [SerializeField] private Text totalMoney;
    public GameObject quitButton;

    private void Start()
    {
        GetStats();
    }

    public void GetStats ()
    {
        //The reason it looks like this is to easier map out how long the rows of text should be to look the best
        levels.text =           "Levels deep: " + GameManager.Instance.GetCurrentLevel();
        monstersKilled.text =   "Monsters killed: " + GameManager.Instance.TotalEnemiesKilled();
        attacks.text =          "Total attacks made: " + GameManager.Instance.TotalAttacks();
        attacksHit.text =       "Attacks hit: " + GameManager.Instance.AttacksHit();
        jumps.text =            "Total times jumped: " + GameManager.Instance.TotalJumps();
        hitsTaken.text =        "Hits taken: " + GameManager.Instance.HitsTaken();
        hitsBlocked.text =      "Hits blocked: " + GameManager.Instance.HitsBlocked();
        totalMoney.text =       "Total money earned: " + GameManager.Instance.TotalMoney();
    }

    public void Quit ()
    {
        GameManager.Instance.ExitToMain();
    }
}
