using UnityEngine;

public class EnemyAI : ChangeDirection
{
    Rigidbody2D rb2D;
    EnemyStats enemyStats;
    EnemyAttack attackZone;
    IEnemyType enemyType;

    [HideInInspector]
    public bool left = true;

    public Transform sightStartT;

    public Transform sightEndT01;
    public Transform sightEndT02;
    public Transform sightEndT03;

    public Transform sightBehindT;

    public Vector3 sightRange;
    public Vector3 eyeHeight = new Vector3(0, .6f, 0);

    private bool spotted = false;
    public bool spottedBehind = false;

    private void Start()
    {
        sightEndT01.position += sightRange;
        sightEndT02.position += sightRange;
        sightEndT03.position += sightRange;
        sightBehindT.position += (sightRange / 2) * -1;

        rb2D = GetComponent<Rigidbody2D>();
        enemyStats = GetComponent<EnemyStats>();
        enemyType = GetComponent<IEnemyType>();
        attackZone = transform.GetComponentInChildren<EnemyAttack>();
    }

    void Update ()
    {
        DetectPlayer();
        if (spottedBehind)
        {
            ChangeingDirection();
        }
        if (spotted)
        {
            PlayerSpotted(true);
        }
        else if (spotted == false)
        {
            PlayerSpotted(false);
        }
	}

    public void DetectPlayer()
    {
        Debug.DrawLine(sightStartT.position, sightEndT01.position, Color.red);
        Debug.DrawLine(sightStartT.position, sightBehindT.position, Color.green);

        Debug.DrawLine(sightStartT.position, sightEndT02.position, Color.red);
        Debug.DrawLine(sightStartT.position, sightEndT03.position, Color.red);

        spotted = Physics2D.Linecast(sightStartT.position, sightEndT01.position, 1 << LayerMask.NameToLayer("Player"));
        spotted = Physics2D.Linecast(sightStartT.position, sightEndT02.position, 1 << LayerMask.NameToLayer("Player"));
        spotted = Physics2D.Linecast(sightStartT.position, sightEndT03.position, 1 << LayerMask.NameToLayer("Player"));
        spottedBehind = Physics2D.Linecast(sightStartT.position, sightBehindT.position, 1 << LayerMask.NameToLayer("Player"));

        if (spottedBehind)
        {
            left = !left;
        }
    }

    public void PlayerSpotted(bool isWalking)
    {
        if (isWalking == false)
        {
            StopMoving();
        }
        if (attackZone.inAttackRange)
        {
            StopMoving();
            return;
        }
        else
        {
            if (isWalking)
            {
                //TODO: Make a reaction timer for the first time he spots you so that he doesent just charge at you instantly
                //Invoke("Move", 1f);
                Move();
            }
        }
    }

    public void Move()
    {
        enemyType.Walking();
        Vector2 movement = new Vector2((transform.localScale.normalized.x * -1) * enemyStats.movementSpeed, rb2D.velocity.y);
        rb2D.velocity = movement;
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