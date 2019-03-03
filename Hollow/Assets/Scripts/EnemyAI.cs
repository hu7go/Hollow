using UnityEngine;

public class EnemyAI : ChangeDirection
{
    Rigidbody2D rb2D;
    EnemyStats enemyStats;
    EnemyAttack attackZone;
    EnemyJump eJ;
    IEnemyType enemyType;

    AudioSource aS;

    [SerializeField] private float jumpThrust = 5f;
    [HideInInspector] public bool right = true;

    [SerializeField] private AudioClip jumpSound;
    [SerializeField] private AudioClip moveSound;
    [SerializeField] private float soundDelay = 2f;

    private float soundCD;

    private void Start()
    {
        aS = GetComponent<AudioSource>();

        eJ = GetComponentInChildren<EnemyJump>();
        rb2D = GetComponent<Rigidbody2D>();
        enemyStats = GetComponent<EnemyStats>();
        enemyType = GetComponent<IEnemyType>();
        attackZone = transform.GetComponentInChildren<EnemyAttack>();
    }

    public void Move(Transform target)
    {
        if (attackZone.inAttackRange)
            return;

        if (enemyStats.isDead)
            return;

        //Look left
        if (target.position.x < transform.position.x && transform.localScale.x < 0)
        {
            ChangeingDirection();
            right = true;
        }
        //Look Right
        else if (target.position.x > transform.position.x && transform.localScale.x > 0)
        {
            ChangeingDirection();
            right = false;
        }

        enemyType.Walking();
        Vector2 movement = new Vector2((transform.localScale.normalized.x * -1) * enemyStats.movementSpeed, rb2D.velocity.y);
        rb2D.velocity = movement;

        FootSteps();
    }

    public void FootSteps ()
    {
        if (eJ.canJump == false)
            return;

        if (rb2D.velocity.sqrMagnitude == 0)
            return;

        soundCD -= .1f;

        if (soundCD <= 0)
        {
            aS.pitch = Random.Range(0.6f, 0.9f);
            aS.clip = moveSound;
            aS.Play();

            soundCD = soundDelay;
        }
    }

    public void Jump()
    {
        aS.pitch = Random.Range(0.9f, 1.1f);
        aS.clip = jumpSound;
        aS.Play();

        Vector2 jump = new Vector2(rb2D.velocity.x, (transform.localScale.normalized.y) * enemyStats.movementSpeed) * jumpThrust;
        rb2D.velocity = jump;
    }

    public void StopMoving()
    {
        enemyType.StopWalking();
    }

    public void Attack()
    {
        enemyType.Attacking();
    }
}