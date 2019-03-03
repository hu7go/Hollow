using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterSpawner : MonoBehaviour
{
    public GameObject tmpMarker;

    public int maxWidth = 4;
    public int maxDepth = 3;

    public GameObject waterPrefab;

    [HideInInspector]
    public RoomGenerator roomGen;
    public List<RandomeBackground> spawnLocations;

    int myX;
    int myY;

    public List<List<GameObject>> waterRows = new List<List<GameObject>>();
    private List<GameObject> allWater = new List<GameObject>();
    private List<GameObject> tmpList = new List<GameObject>();

    void Start ()
    {
        roomGen = GetComponentInParent<RoomGenerator>();
	}

    public void SetList(int x, int y)
    {
        myX = x;
        myY = y;
        //This debugs where the water starts its calculation in world space
        //Debug.Log(x + " " + y);

        //Marks where it starts!
        //GameObject tmp2 = Instantiate(tmpMarker, (new Vector3(-roomGen.width / 2 + x + 0.5f, -roomGen.height / 2 + y + 0.5f, 0)) / 2.1f, transform.rotation, transform);

        CheckDown();
    }

    public void CheckDown()
    {
        if (roomGen.map[myX, myY - 1] == 0)
        {
            myY = myY - 1;
            CheckDown();
        }
        else
        {
            //TODO: Add Random left-right

            CheckDirection();
        }
    }

    public void CheckDirection()
    {
        //for (int x = 0; x < 10; x++)
        //{
        //    int pX = 0;
        //    int nX = -1;

        //    if (roomGen.map[myX + pX, myY -1] == 0)
        //    {
        //        pX++;
        //    }
        //    else
        //    {
        //        //myX += pX;
        //        //myY--;
        //        FillWater();
        //    }

        //    if (roomGen.map[myX + nX, myY -1] == 0)
        //    {
        //        nX--;
        //    }
        //    else
        //    {
        //        //myX += nX;
        //        //myY--;
        //        FillWater();
        //    }
        //}

        //This checks left
        if (roomGen.map[myX - 1, myY] == 0)
        {
            myX = myX - 1;
            CheckDown();
        }
        else
        {
            //Debug.Log("left " + myX + " " + myY);
            FillWater();
        }
    }

    public void FillWater()
    {
        for (int y = 0; y < maxDepth; y++)
        {
            int currentWidth = 0;
            int positiveX = 0;
            int negativeX = -1;

            for (int x = 0; x < maxWidth && currentWidth < maxWidth; x++)
            {
                if (roomGen.map[myX + positiveX, myY + y] == 0)
                {
                    ///This code checks one under the currently placed water tile
                    if (roomGen.map[myX + positiveX, myY + y - 1] == 0)
                    {
                        roomGen.map[myX + positiveX, myY + y] = 0;

                        //Catches a error that sometimes happens that stops the water from spawning correctly,
                        //now the water should spawn in rows but wont always be correct acording to the variables.
                        try
                        {
                            //Debug.Log(waterRows[waterRows.Count - 1].Count);

                            foreach (GameObject waterTile in waterRows[waterRows.Count - 1])
                            {
                                Destroy(waterTile);
                            }
                        }
                        catch (System.Exception e)
                        {
                            //Debug.Log(e.Message);
                        }

                        myX += positiveX;
                        myY += y;
                        CheckDown();
                        return;
                    }
                    ///
                    SavePos(myX + positiveX, myY + y);
                    positiveX++;
                    currentWidth++;
                }

                if (roomGen.map[myX + negativeX, myY + y] == 0)
                {
                    ///This code checks one under the currently placed water tile
                    if (roomGen.map[myX + negativeX, myY + y - 1] == 0)
                    {
                        roomGen.map[myX + negativeX, myY + y] = 0;
                        foreach (GameObject waterTile in waterRows[waterRows.Count - 1])
                        {
                            Destroy(waterTile);
                        }
                        myX += negativeX;
                        myY += y;
                        CheckDown();
                        return;
                    }
                    ///
                    SavePos(myX + negativeX, myY + y);
                    negativeX--;
                    currentWidth++;
                }
            }

            waterRows.Add(tmpList);

            if (currentWidth == maxWidth)
            {
                //Removes a row of water if its to long
                foreach (GameObject waterTile in waterRows[waterRows.Count - 1])
                {
                    Destroy(waterTile);
                }
                break;
            }
            tmpList.Clear();
        }
    }

    public void InstantiateWater(Vector3 pos)
    {
        //GameObject tmpWater = Instantiate(waterPrefab, pos, transform.rotation, transform);
        GameObject tmp = Instantiate(waterPrefab, pos, transform.rotation, transform);
        //GameObject tmp2 = Instantiate(tmpMarker, pos, transform.rotation, transform);

        //tmpList.Add(tmp);
        allWater.Add(tmp);
    }

    public void SavePos(int x, int y)
    {
        //This makes it into a water tile
        roomGen.map[x, y] = 2;
        InstantiateWater((new Vector3(-roomGen.width / 2 + x + 0.5f, -roomGen.height / 2 + y + 0.5f, 0)) * GameManager.globalScale);
    }
}
