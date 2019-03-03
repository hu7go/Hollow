using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private float zOffset = -10f;
    [SerializeField] private float speed = 5f;
    [SerializeField] public Transform player;

    private Vector3 newPos;

    public static CameraFollow Instance { get; private set; }

    public void Awake()
    {
        Instance = this;
    }

    void LateUpdate ()
    {
        newPos = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical") + 2, zOffset) + player.position;

        Vector3 smoothedPosition = Vector3.Lerp(transform.position, newPos, Time.deltaTime * speed);
        transform.position = smoothedPosition;
    }
}
