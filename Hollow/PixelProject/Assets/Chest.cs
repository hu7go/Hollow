﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour, I_InteractableObject
{
    Interact currentInteraction;
    GameObject currentOther;
    Animator animController;
    GameObject currencyHolder;
    AllLoot currencyList;
    private bool interacted = false;

    [Header("The min and max amount of loot")]
    public int min = 5;
    public int max = 10;

    void Start()
    {
        currencyHolder = GameObject.FindGameObjectWithTag("LootHolder");
        currencyList = currencyHolder.GetComponent<AllLoot>();
        animController = GetComponentInParent<Animator>();
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
        if (!interacted)
        {
            animController.SetTrigger("Open");
            Loot();
            interacted = true;
        }
    }

    public void Loot()
    {
        int randomAmountOfLoot = Random.Range(min, max);
        int currentCurrencyDrop = 0;
        int maxCurrencyDrop = 200;

        for (int i = 0; i < randomAmountOfLoot; i++)
        {
            if (currentCurrencyDrop < maxCurrencyDrop)
            {
                GameObject newLoot = Instantiate(currencyList.allCurrency[Random.Range(0, currencyList.allCurrency.Length)], transform.position + new Vector3(0, .4f, 0), transform.rotation, currencyHolder.transform);
                currentCurrencyDrop += newLoot.GetComponent<Currency>().currencyPrefab.currencyWorth;

                newLoot.GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-40, 40), Random.Range(150, 250)));
            }
        }
    }
}