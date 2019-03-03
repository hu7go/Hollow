using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CoinHandler : MonoBehaviour
{
    public int totalMoney = 0;
    public TMP_Text currencyText;

    public void Start()
    {
        UpdateText();
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Coin")
        {
            totalMoney += other.gameObject.GetComponent<Currency>().currencyPrefab.currencyWorth;
            Destroy(other.gameObject);
            PlayCoinSound();
            UpdateText();
        }
    }

    public void PlayCoinSound()
    {
        //TODO: Make it so that it plays a coin pickup sound here.
    }

    public void UpdateText()
    {
        currencyText.text = "Money: " + totalMoney.ToString();
    }
}
