using UnityEngine;

[RequireComponent(typeof(EnemyStats))]
public class Skeleton : MonoBehaviour, IEnemyType
{
    EnemyStats enemyController;
    EnemyAI sightController;
    Animator animController;

    public void Start()
    {
        animController = GetComponent<Animator>();
        enemyController = GetComponent<EnemyStats>();
    }

    public void TakeDamage ()
    {
        if (enemyController.health <= 0)
        {
            DieAnim();
            return;
        }
        animController.SetTrigger("TakeDamage");
    }

    public void DieAnim ()
    {
        animController.SetTrigger("Die");
    }

    public void Walking()
    {
        animController.SetBool("Walking", true);
    }

    public void StopWalking()
    {
        animController.SetBool("Walking", false);
    }

    public void Attacking()
    {
        animController.SetTrigger("Attack");
    }
}
