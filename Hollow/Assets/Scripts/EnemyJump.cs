using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyJump : MonoBehaviour
{
    EnemyAI ai;
    [HideInInspector] public bool canJump = true;
    private float jumpCD = .3f;

    private void Start()
    {
        ai = GetComponentInParent<EnemyAI>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Ground" && canJump)
        {
            ai.Jump();
            StartCoroutine(JumpCoolDown());
        }
    }

    private IEnumerator JumpCoolDown ()
    {
        canJump = false;
        yield return new WaitForSeconds(jumpCD);
        canJump = true;
    }
}
