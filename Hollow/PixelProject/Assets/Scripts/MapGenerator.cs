using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public GameObject room;

    [Header("Its not recomended to go over 10!")]
    [Header("Use with caution")]
    [Range(1, 100)]
    public int width = 3;
    public int height = 1;

    private float offsetX = 19;
    private float offsetY = 14.285f;

    public List<RoomGenerator> rooms;

	void Awake ()
    {
        SpawnMap();
	}

    public void SpawnMap()
    {
        int tmpX = 0;
        int tmpY = 0;

        float chance = 100 / width;

        bool foundOpening = false;
        bool stopLooking = false;

        for (int y = 0; y < width; y++)
        {
            for (int x = 0; x < height; x++)
            {
                //This code happens when a new row begins
                if (tmpX >= width)
                {
                    tmpX = 0;
                    tmpY--;
                    chance = 100 / width;
                    foundOpening = false;
                    stopLooking = false;
                }

                if (!foundOpening && !stopLooking)
                {
                    if (GetOpening(chance))
                    {
                        foundOpening = true;
                    }
                    chance += 100 / width;
                }

                GameObject newRoom = Instantiate(room, transform.position, transform.rotation, transform);
                RoomGenerator RG = newRoom.GetComponent<RoomGenerator>();
                RG.mapOffset = new Vector3(offsetX * tmpX, offsetY * tmpY, 0);
                rooms.Add(RG);

                if (tmpX == 0)
                {
                    RG.leftCorner = true;
                }
                if (tmpX == width - 1)
                {
                    RG.rightCorner = true;
                }
                if (!foundOpening)
                {
                    RG.openDown = false;
                }
                if (foundOpening)
                {
                    RG.openDown = true;
                    foundOpening = false;
                    stopLooking = true;
                }

                //if (true)
                //{
                //    rooms[tmpX].openUp = true;
                //    Debug.Log(rooms[tmpX].mapOffset);
                //}

                tmpX++;
            }
        }

        rooms[0].startArea = true;
    }

    public bool GetOpening(float chance)
    {
        bool openingFound = false;
        int tmpRandomNumber = 0;

        tmpRandomNumber = Random.Range(0, 100);

        if (tmpRandomNumber < chance)
        {
            openingFound = true;
        }

        return openingFound;
    }
}
