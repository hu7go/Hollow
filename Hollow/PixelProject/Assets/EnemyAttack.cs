using System.Collections;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    EnemyStats enemyStats;
    EnemyAI enemyAI;

    public bool isArmorPiercing = false;

    public float minAttackCooldown = 1.5f;
    public float maxAttackCooldown = 3f;

    [Space(20)]
    public float swingTime = 0.5f;

    [HideInInspector]
    public bool inAttackRange = false;

    private GameObject currentPlayer;
    public bool isAttacking = false;

    public void Start()
    {
        enemyStats = GetComponentInParent<EnemyStats>();
        enemyAI = GetComponentInParent<EnemyAI>();
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Character")
        {
            currentPlayer = other.gameObject;
            inAttackRange = true;
            enemyAI.StopMoving();
            if (!isAttacking)
            {
                Attacking();
            }
        }
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Character")
        {
            inAttackRange = false;
        }
    }

    public void Attacking()
    {
        if (!enemyStats.isDead)
        {
            enemyAI.Attack();
            Invoke("DealDamage", swingTime);
            StartCoroutine(AttackAgain());
        }
    }

    private void DealDamage()
    {
        if (!enemyStats.isDead)
        {
            if (inAttackRange)
            {
                if (currentPlayer.GetComponent<playerController>().isBlocking)
                {
                    currentPlayer.GetComponent<playerController>().KnockBack(true , enemyStats.damage, enemyAI.left);
                }
                else
                {
                    currentPlayer.GetComponent<playerController>().KnockBack(false, enemyStats.damage, enemyAI.left);
                    currentPlayer.GetComponentInChildren<PlayerHealth>().TakeDamage(enemyStats.damage, isArmorPiercing);
                }
            }
        }
    }

    private IEnumerator AttackAgain()
    {
        isAttacking = true;
        float tmpNumber = AttackCooldown();
        yield return new WaitForSeconds(tmpNumber);
        isAttacking = false;
        if (inAttackRange)
        {
            Attacking();
        }
    }

    private float AttackCooldown()
    {
        float tmpNumber = Random.Range(minAttackCooldown, maxAttackCooldown);
        return tmpNumber;
    }
}
