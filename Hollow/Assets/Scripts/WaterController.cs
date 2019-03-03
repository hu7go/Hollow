using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterController : MonoBehaviour
{
    Rigidbody2D rb2D;
    [SerializeField] private float waterGravity = 0.35f;
    private float baseGravity = 1;
    private float baseDrag = 0;
    private float waterDrag = 1.6f;

    [HideInInspector]
    public bool inWater = false;

    public void OnEnable()
    {
        rb2D = GetComponent<Rigidbody2D>();
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Water")
        {
            inWater = true;
            rb2D.gravityScale = waterGravity;
            rb2D.drag = waterDrag;
        }
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Water")
        {
            inWater = false;
            rb2D.gravityScale = baseGravity;
            rb2D.drag = baseDrag;
        }
    }
}
