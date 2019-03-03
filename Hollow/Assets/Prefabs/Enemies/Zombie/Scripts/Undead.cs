using UnityEngine;

public class Undead : EnemyStats, IEnemyType
{
    Animator animController;

    public override void Start ()
    {
        base.Start();
        animController = GetComponent<Animator>();
    }

    public void TakeDamage()
    {
        if (health <= 0)
        {
            DieAnim();
            return;
        }
        animController.SetTrigger("TakeDamage");
    }

    public void DieAnim()
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
    public void Die ()
    {

    }
}
