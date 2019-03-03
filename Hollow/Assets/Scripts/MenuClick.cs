using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class MenuClick : MonoBehaviour, ISelectHandler
{
    AudioSource aS;

    private void Start()
    {
        aS = GetComponent<AudioSource>();
    }

    public void OnSelect(BaseEventData eventData)
    {
        aS.Play();
    }
}
