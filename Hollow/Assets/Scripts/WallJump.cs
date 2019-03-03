using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallJump : MonoBehaviour
{
    public bool canWallJump = false;

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Ground")
            canWallJump = true;
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Ground")
            canWallJump = false;
    }
}
