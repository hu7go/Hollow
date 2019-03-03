using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Movement directionHandler;
    public Transform target;

    public float smoothSpeed = 0.125f;
    public Vector3 offset;

    void LateUpdate ()
    {
        if (directionHandler.goingRight)
        {
            offset.x = 1.25f;
        }
        else
        {
            offset.x = -1.25f;
        }

        offset.y = 2;

        if (Input.GetKey(KeyCode.W))
        {
            offset.y += 1f;
        }
        if (Input.GetKey(KeyCode.S))
        {
            offset.y += -1f;
        }
        if (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.D))
        {
            offset.x = 0;
        }

        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed / 2);
        transform.position = smoothedPosition;
    }
}
