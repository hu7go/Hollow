using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArmorPoint : MonoBehaviour
{
    public Sprite deactive;
    public GameObject armorShatterEffect;
    public bool isShaking = false;
    Vector3 originalPos;

    public void Start()
    {
        originalPos = transform.localPosition;
    }

    public void GotRemoved()
    {
        isShaking = true;
        Invoke("StopShake", .1f);

        GetComponent<Image>().sprite = deactive;

        //need help with the particles not acting like i want them to
        /*
        GameObject tmpEffect = Instantiate(armorShatterEffect, transform.position, transform.rotation);
        Destroy(tmpEffect, 1);
        */
    }

    public void StopShake()
    {
        isShaking = false;
    }

    public void Update()
    {
        if (isShaking)
        {
            float x = Random.Range(-1f, 1f) * 5;
            float y = Random.Range(-1f, 1f) * 5;

            transform.localPosition += new Vector3(x, y, originalPos.z);
        }
        else
        {
            transform.localPosition = originalPos;
        }
    }
}
