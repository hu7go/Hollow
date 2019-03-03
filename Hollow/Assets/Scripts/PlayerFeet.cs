using UnityEngine;

public class PlayerFeet : MonoBehaviour
{
    public bool grounded = false;
    AudioSource aS;

    private void Start()
    {
        aS = GetComponent<AudioSource>();
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            aS.pitch = Random.Range(0.9f, 1.1f);
            aS.Play();
            grounded = true;
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            grounded = false;
        }
    }
}
