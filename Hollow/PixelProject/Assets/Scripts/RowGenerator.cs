using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RowGenerator : MonoBehaviour
{
    public GameObject roomPrefab;
    Camera mainCamera;

    public int maxNumberOFRooms;

    public int[] rooms;

    private float width;
    private float height;


	// Use this for initialization
	void Start ()
    {
        mainCamera = Camera.main;
        height = 2f * mainCamera.orthographicSize;
        width = height * mainCamera.aspect;

        rooms = new int[maxNumberOFRooms];
        GenerateRooms();
	}

    public void GenerateRooms ()
    {
        Vector3 roomPos = new Vector3(0, 0, 0);

        for (int i = 0; i < maxNumberOFRooms; i++)
        {
            roomPos += new Vector3(width, 0, 0);
            Instantiate(roomPrefab, transform.position + roomPos, transform.rotation);
        }
    }
}
