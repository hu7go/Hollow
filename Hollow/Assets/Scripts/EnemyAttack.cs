using System.Collections;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    EnemyStats enemyStats;
    EnemyAI enemyAI;
    public EnemyDetection AI;

    public bool isArmorPiercing = false;

    public float minAttackCooldown = 1.5f;
    public float maxAttackCooldown = 3f;

    [Space(20)]
    public float swingTime = 0.5f;

    [HideInInspector]
    public bool inAttackRange = false;

    public GameObject currentPlayer;
    public bool isAttacking = false;

    AudioSource aS;
    [Space(20)]
    [SerializeField] private AudioClip blocked;
    [SerializeField] private AudioClip swing;

    public void Start()
    {
        aS = GetComponent<AudioSource>();
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
            aS.pitch = Random.Range(0.9f, 1.1f);
            aS.clip = swing;
            aS.Play();
            if (inAttackRange)
            {
                if (currentPlayer.GetComponent<PlayerController>().isBlocking)
                {
                    aS.clip = blocked;
                    aS.Play();
                    currentPlayer.GetComponent<PlayerController>().KnockBack(true , enemyStats.damage / 2, enemyAI.right);
                }
                else
                {
                    currentPlayer.GetComponent<PlayerController>().KnockBack(false, enemyStats.damage / 2, enemyAI.right);
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
