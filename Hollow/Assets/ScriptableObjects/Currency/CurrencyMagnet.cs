using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrencyMagnet : MonoBehaviour
{
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Character")
        {
            transform.Translate(Vector3.Lerp(transform.position, other.transform.position, .1f));
        }
    }
}
