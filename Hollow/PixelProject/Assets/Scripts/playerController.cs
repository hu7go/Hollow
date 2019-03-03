using System.Collections;
using UnityEngine;

public class playerController : ChangeDirection
{
    Movement movement;
    AnimationController animController;
    BoxCollider2D mainCollider;
    PlayerWeapon swordAttackOne;
    PlayerWeapon swordAttackTwo;

    public float stunDuration = .5f;
    private float stunned = 0f;

    [Header("Physics materials")]
    public PhysicsMaterial2D mainMaterial;
    public PhysicsMaterial2D blockMaterial;
    [Space(20)]

    GameObject attackOneHB;
    GameObject attackTwoHB;
    public float attackCooldown = 1f;

    [Space(20)]
    public Vector3 hitBoxPos01;
    public Vector3 hitBoxPos02;

    [HideInInspector]
    public int currentForce;

    public bool isBlocking = false;

    [Space(20)]
    public GameObject blockSparks;
    public GameObject bloodEffect;

    public void Start()
    {
        mainCollider = GetComponent<BoxCollider2D>();
        movement = GetComponent<Movement>();
        animController = GetComponent<AnimationController>();
        attackOneHB = transform.Find("AttackOneHitBox").gameObject;
        attackTwoHB = transform.Find("AttackTwoHitBox").gameObject;
        stunned = stunDuration;

        swordAttackOne = attackOneHB.GetComponent<PlayerWeapon>();
        swordAttackTwo = attackTwoHB.GetComponent<PlayerWeapon>();
    }

    public void Update()
    {
        if (movement.stunned)
        {
            stunned = stunned - 1 * Time.deltaTime;
        }
        if (stunned <= 0)
        {
            movement.stunned = false;
            stunned = stunDuration;
        }

        attackCooldown -= 1.5f * Time.deltaTime;

        if (attackCooldown < 0)
        {
            attackCooldown = 0;
        }

        CheckWhichDirection();

        if (Input.GetMouseButtonDown(0) && attackCooldown <= 0 && isBlocking == false)
        {
            attackCooldown = 1;
            animController.Attack01();
            StartCoroutine(AttackBox(.15f, attackOneHB));
        }
        if (Input.GetMouseButtonDown(1) && attackCooldown <= 0 && isBlocking == false)
        {
            attackCooldown = 1;
            animController.Attack02();
            StartCoroutine(AttackBox(.25f, attackTwoHB));
        }
        if (Input.GetKey(KeyCode.Q))
        {
            mainCollider.sharedMaterial = blockMaterial;
            isBlocking = true;
            animController.BlockAnim(isBlocking);
        }
        else
        {
            //This needs to change a bit, currently running every frame.
            mainCollider.sharedMaterial = mainMaterial;
            isBlocking = false;
            animController.BlockAnim(isBlocking);
        }
    }

    private IEnumerator AttackBox(float timeToAttack, GameObject hitBox)
    {
        yield return new WaitForSeconds(timeToAttack);
        hitBox.SetActive(true);
        StartCoroutine(StopAttack(timeToAttack, hitBox));
    }

    private IEnumerator StopAttack(float time, GameObject hitbox)
    {
        yield return new WaitForSeconds(time);
        hitbox.SetActive(false);
    }
    
    private void CheckWhichDirection()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            if (transform.localScale.x < 0)
            {
                ChangeingDirection();
            }
            currentForce = 1;
            swordAttackOne.force = swordAttackOne.currentWeapon.damageForce;
            swordAttackTwo.force = swordAttackTwo.currentWeapon.damageForce * 1.5f;
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            if (transform.localScale.x > 0)
            {
                ChangeingDirection();
            }
            currentForce = -1;
            swordAttackOne.force = -1 * swordAttackOne.currentWeapon.damageForce;
            swordAttackTwo.force = -1.5f *swordAttackTwo.currentWeapon.damageForce;
        }
    }

    public void StopAttack(int whichAttack)
    {
        if (whichAttack == 1)
        {
            attackOneHB.SetActive(false);
        }
        if (whichAttack == 2)
        {
            attackTwoHB.SetActive(false);
        }
    }

    public void KnockBack(bool blocked, float knockbackForce, bool left = true)
    {
        int tmpNumber;
        if (left)
            tmpNumber = 1;
        else
            tmpNumber = -1;

        if (blocked)
        {
            GameObject tmpEffect = Instantiate(blockSparks, transform.position + new Vector3(.5f, .5f), transform.rotation, transform);
            Destroy(tmpEffect, 1f);
        }
        else
        {
            GameObject tmpEffect = Instantiate(bloodEffect, transform.position + new Vector3(.5f, .5f), transform.rotation, transform);
            Destroy(tmpEffect, 1f);
        }

        GetComponent<Rigidbody2D>().AddForce(new Vector2(tmpNumber* (-2f * knockbackForce),(1f * knockbackForce)), ForceMode2D.Impulse);

        movement.stunned = true;
    }
}

