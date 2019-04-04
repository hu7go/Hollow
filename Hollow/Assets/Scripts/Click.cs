using UnityEngine;

public class Click : MonoBehaviour
{
    AudioSource aS;

    public void Start()
    {
        aS = GetComponent<AudioSource>();
    }

    public void Clicking ()
    {
        if (aS.isPlaying)
            return;

        aS.pitch = Random.Range(0.9f, 1.1f);
        aS.Play();
    }
}
