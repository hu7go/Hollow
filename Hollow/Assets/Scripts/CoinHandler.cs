using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoinHandler : MonoBehaviour
{
    public int currentMoney = 0;
    public Text moneyText;

    private int totalMoney;

    AudioSource audioS;

    public void Start()
    {
        audioS = GetComponent<AudioSource>();

        if (moneyText == null)
            moneyText = GameObject.Find("MoneyText").GetComponent<Text>();

        //if (GameManager.Instance != null)
        //    totalMoney = GameManager.Instance.GetMoney();
        //else
        //    Debug.Log("There is currently no game manager in this scene!");

        UpdateText();
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Coin")
        {
            audioS.pitch = Random.Range(0.9f, 1.1f);
            audioS.Play();

            currentMoney += other.gameObject.GetComponent<Currency>().currencyPrefab.currencyWorth;
            Destroy(other.gameObject);
            PlayCoinSound();
            UpdateText();

            totalMoney += other.gameObject.GetComponent<Currency>().currencyPrefab.currencyWorth;
        }
    }

    public void PlayCoinSound()
    {
        //TODO: Make it so that it plays a coin pickup sound here.
    }

    public void UpdateText()
    {
        moneyText.text = "Money: " + currentMoney.ToString();
    }

    private void OnDisable()
    {
        //if (GameManager.Instance != null)
        //    GameManager.Instance.SetMoney(totalMoney);
        //else
        //    Debug.Log("There is currently no game manager in this scene!");
    }

    public int GetCurrentMoney()
    {
        return currentMoney;
    }

    public void SetMoney(int newMoney)
    {
        moneyText = GameObject.Find("MoneyText").GetComponent<Text>();

        currentMoney = newMoney;
        UpdateText();
    }

    public int TotalMoney ()
    {
        return totalMoney;
    }
}
