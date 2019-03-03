using UnityEngine;

public class Currency : MonoBehaviour
{
    public CurrencySOBJ currencyPrefab;

    public void Start()
    {
        Invoke("ActivateCoin", .1f);
    }

    public void ActivateCoin()
    {
        gameObject.tag = "Coin";
    }
}
