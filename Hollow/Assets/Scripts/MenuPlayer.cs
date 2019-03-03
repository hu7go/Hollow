using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuPlayer : MonoBehaviour
{
    Animator anim;
    SpriteRenderer sr;

    [SerializeField] private Vector3 dest;
    [SerializeField] private Vector3 origin;

    private float speed = 1.5f;

    public void Start()
    {
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
    }

    public void Move ()
    {
        StartCoroutine(Moveing(true));
        sr.flipX = false;
        anim.SetBool("Moving", true);
    }

    public void MoveBack ()
    {
        StartCoroutine(Moveing(false));
        sr.flipX = true;
        anim.SetBool("Moving", true);
    }

    private IEnumerator Moveing (bool forward)
    {
        if (forward)
        {
            while (transform.position != dest)
            {
                if (Vector2.Distance(transform.position, dest) <0.05f)
                {
                    transform.position = dest;
                    anim.SetBool("Moving", false);
                    break;
                }

                transform.position = Vector2.MoveTowards(transform.position, dest, Time.deltaTime * speed);
                yield return null;
            }
        }
        else
        {
            while (transform.position != origin)
            {
                if (Vector2.Distance(transform.position, origin) < 0.05f)
                {
                    transform.position = origin;
                    anim.SetBool("Moving", false);
                    break;
                }

                transform.position = Vector2.MoveTowards(transform.position, origin, Time.deltaTime * speed);
                yield return null;
            }
        }
    }
}
