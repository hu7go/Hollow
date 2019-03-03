using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    public Weapon currentWeapon;
    PlayerController playerController;

    public float force;

    private int myDamage;

    public void Start()
    {
        playerController = GetComponentInParent<PlayerController>();
    }

    public void Attack (GameObject enemy, bool lightAttack)
    {
        if (lightAttack)
        {
            myDamage = currentWeapon.damage;
        }
        else
        {
            myDamage = currentWeapon.heavyAttackDamage;
            force = force * 1.5f;
        }

        int tmpForce = playerController.currentForce;

        enemy.GetComponent<EnemyStats>().TakeDamage(myDamage, tmpForce);
        enemy.GetComponent<Rigidbody2D>().AddForce(new Vector2(force, force));
    }
}
