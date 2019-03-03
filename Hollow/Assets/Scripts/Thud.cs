using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thud : MonoBehaviour
{
    AudioSource aS;

	void Start ()
    {
        aS = GetComponent<AudioSource>();
	}

    public void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Ground")
        {
            aS.pitch = Random.Range(0.9f, 1.1f);
            aS.Play();
        }
    }
}
