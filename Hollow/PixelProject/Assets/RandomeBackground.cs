using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomeBackground : MonoBehaviour
{
    public bool test = false;

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

    [Space(20)]
    public Sprite roofSpikes;

    void Start ()
    {
        //RandomSprite();
    }

    public void RandomSprite()
    {
        int randomNumber = Random.Range(0, 20);
        if (randomNumber >= 2)
        {
            myRenderer.sprite = sprite01;
        }
        else if (randomNumber == 1)
        {
            myRenderer.sprite = sprite02;
        }
        else if (randomNumber == 0)
        {
            myRenderer.sprite = sprite03;
        }
    }

    public void RandomPlant()
    {
        int randomNumber = Random.Range(0, 35);

        if (randomNumber == 0 || randomNumber == 1)
        {
            myRenderer.sprite = plant01;
        }
        else if (randomNumber == 2 || randomNumber == 3)
        {
            myRenderer.sprite = plant02;
        }
        else if (randomNumber == 4)
        {
            myRenderer.sprite = plant03;
        }
        else
        {
            RandomSprite();
        }
    }

    public void RoofSpikes()
    {
        int randomNumber = Random.Range(0, 10);

        if (randomNumber == 0)
        {
            test = true;
            //Destroy(this.gameObject);
            myRenderer.sprite = roofSpikes;
        }
        else
        {
            RandomSprite();
        }
    }
}
