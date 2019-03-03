using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeDirection : MonoBehaviour
{
    public void ChangeingDirection()
    {
        var tmp = transform.localScale;
        tmp.x = tmp.x * -1;
        transform.localScale = tmp;
    }
}
