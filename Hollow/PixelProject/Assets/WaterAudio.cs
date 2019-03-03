using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class WaterAudio : MonoBehaviour
{
    public AudioMixer musicMixer;

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Head")
        {
            musicMixer.SetFloat("Frequency", 0.05f);
        }
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Head")
        {
            musicMixer.SetFloat("Frequency", 1f);
        }
    }
}
