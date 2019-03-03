using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuAnimations : MonoBehaviour
{
    [SerializeField] private Transform bat1;
    [SerializeField] private Transform bat2;
    [SerializeField] private Transform bat3;

    private Vector3 speed = new Vector3(0.04f, 0, 0);

    private float time;
    private float stopTime = 7;

    public void StartAnims ()
    {
        StartCoroutine(Move());
        bat3.gameObject.GetComponent<Animator>().speed -= .3f;
    }

    private IEnumerator Move ()
    {
        while (time < stopTime)
        {
            time += Time.deltaTime;

            bat1.position -= speed;
            bat2.position -= speed + new Vector3(.001f, 0, 0);
            bat3.position -= speed - new Vector3(.004f, 0, 0);
            yield return null;
        }
    }
}
