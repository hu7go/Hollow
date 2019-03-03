using UnityEngine;

public class Currency : MonoBehaviour
{
    public CurrencySOBJ currencyPrefab;
    [SerializeField] private AudioSource aS;

    public void Start()
    {
        Invoke("ActivateCoin", .1f);
    }

    public void ActivateCoin()
    {
        gameObject.tag = "Coin";
    }

    public void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Ground")
        {
            aS.pitch = Random.Range(0.9f, 1.5f);
            aS.Play();
        }
    }
}
