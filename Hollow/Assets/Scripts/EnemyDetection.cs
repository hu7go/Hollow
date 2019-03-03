using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDetection : MonoBehaviour
{
    public EnemyAI myAI;
    public EnemyAttack attackAI;

    private Transform target;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Character")
        {
            GameManager.Instance.StartCombat();
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "Character")
        {
            target = other.gameObject.transform;
            if (attackAI.isAttacking)
                return;

            myAI.Move(target);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        myAI.StopMoving();
        if (other.gameObject.tag == "Character")
        {
            GameManager.Instance.StopCombat();
        }
    }
}
