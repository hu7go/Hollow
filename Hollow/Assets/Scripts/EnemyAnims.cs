using UnityEngine;

public class EnemyAnims : EnemyStats, IEnemyType
{
    Animator animController;

    public override void Start()
    {
        base.Start();
        animController = GetComponent<Animator>();
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

    public void TakeDamage()
    {
        if (health <= 0)
        {
            Die();
            return;
        }
        animController.SetTrigger("TakeDamage");
    }

    public void Die ()
    {
        animController.SetTrigger("Die");
    }
}
