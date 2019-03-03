﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordPickup : MonoBehaviour, I_InteractableObject
{
    Interact currentInteraction;
    GameObject currentOther;
    public SpriteRenderer myRenderer;
    public Weapon weapon;

    public void Start()
    {
        myRenderer.sprite = weapon.image;
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Character")
        {
            currentOther = other.gameObject;
            InRange(true);
        }
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Character")
        {
            InRange(false);
        }
    }

    public void InRange(bool inRange)
    {
        if (currentOther.GetComponent<Interact>())
        {
            currentInteraction = currentOther.GetComponent<Interact>();
            currentInteraction.inRange = inRange;
            if (inRange)
                currentInteraction.IO = this;
            else
                currentInteraction.IO = null;
        }
    }

    public void Interacted()
    {
        currentOther.GetComponentInChildren<Inventory>().AddToInventory(null, weapon);
        DestroyObject(transform.parent.gameObject);
    }
}