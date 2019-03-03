using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireLight : MonoBehaviour
{
    Light myLight;
    [SerializeField] private float max = 10f;
    [SerializeField] private float min = 3f;
    [SerializeField] private float updateTime = .15f;

    void Start()
    {
        myLight = GetComponent<Light>();
        Light();
    }

    private void Light ()
    {
        StartCoroutine(LightUpdate());
    }

    private IEnumerator LightUpdate ()
    {
        yield return new WaitForSeconds(updateTime);
        myLight.intensity = Random.Range(min, max);
        Light();
    }
}
