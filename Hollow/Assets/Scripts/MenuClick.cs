using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class MenuClick : MonoBehaviour, ISelectHandler
{
    public AudioSource aS;

    private void Start()
    {
        aS = GetComponent<AudioSource>();
    }

    public void OnSelect(BaseEventData eventData)
    {
        if (aS == null)
        {
            aS = GetComponent<AudioSource>();
        }

        if (aS.isPlaying == false)
            aS.Play();
    }
}
