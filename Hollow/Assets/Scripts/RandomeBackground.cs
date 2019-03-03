using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomeBackground : MonoBehaviour
{
    public bool canSpawnWater = false;

    [Space(20)]

    public bool isAboveGround = false;

    public SpriteRenderer myRenderer;

    public Sprite sprite01;
    public Sprite sprite02;
    public Sprite sprite03;

    [Space(20)]
    [Header("Unique backgrounds")]
    public Sprite plant01;
    public Sprite plant02;
    public Sprite plant03;
    public Sprite plant04;
    public Sprite plant05;
    public Sprite plant06;
    public Sprite shrooms;
    public Sprite skull;

    [Space(20)]
    public Sprite roofSpikes;

    public void RandomSprite()
    {
        int randomNumber = Random.Range(0, 20);

        if (randomNumber >= 2)
            myRenderer.sprite = sprite01;

        else if (randomNumber == 1)
            myRenderer.sprite = sprite02;

        else if (randomNumber == 0)
            myRenderer.sprite = sprite03;
    }

    public void RandomPlant()
    {
        int randomNumber = Random.Range(0, 100);

        int tmpRandom = Random.Range(0, 2);
        if (tmpRandom == 0)
            myRenderer.flipX = true;

        if (randomNumber <= 5)
            myRenderer.sprite = plant01;

        else if (randomNumber <= 10 && randomNumber > 5)
            myRenderer.sprite = plant02;

        else if (randomNumber <= 13 && randomNumber > 10)
            myRenderer.sprite = plant03;

        else if (randomNumber <= 16 && randomNumber > 13)
            myRenderer.sprite = plant04;

        else if (randomNumber <= 18 && randomNumber > 16)
            myRenderer.sprite = plant05;

        else if (randomNumber <= 20 && randomNumber > 18)
            myRenderer.sprite = plant06;

        else if (randomNumber <= 21 && randomNumber > 20)
            myRenderer.sprite = shrooms;

        else if (randomNumber <= 22 && randomNumber > 21)
            myRenderer.sprite = skull;

        else
            RandomSprite();
    }

    public void RoofSpikes()
    {
        int randomNumber = Random.Range(0, 10);

        int tmpRandom = Random.Range(0, 2);
        if (tmpRandom == 0)
            myRenderer.flipX = true;

        if (randomNumber == 0)
            myRenderer.sprite = roofSpikes;
        else
            RandomSprite();
    }
}
