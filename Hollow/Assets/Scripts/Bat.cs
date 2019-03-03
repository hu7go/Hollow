using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bat : MonoBehaviour
{
    [SerializeField] private float speed = 1f;
    [SerializeField] private float soundDelay = 1f;

    private Vector3 startPos;
    private Vector3 endPos;
    private List<Vector3> positions;
    [SerializeField] private bool moveing = false;

    SpriteRenderer mr;
    Animator anim;
    AudioSource aS;
    float soundCD;

    public void Start()
    {
        aS = GetComponent<AudioSource>();
        anim = GetComponent<Animator>();
        mr = GetComponent<SpriteRenderer>();
        GetEndPos();
    }

    public void Update()
    {
        if (moveing)
        {
            soundCD -= 0.1f;

            if (soundCD <= 0)
            {
                aS.pitch = Random.Range(0.8f, 1.2f);
                aS.Play();

                soundCD = soundDelay;
            }
        }
    }

    public void GetEndPos()
    {
        positions = FindObjectOfType<RoomSpawner>().SendCeilingList();
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Character")
        {
            Move();
            anim.SetTrigger("Triggerd");
        }
    }

    public void Move ()
    {
        if (moveing)
            return;

        StartCoroutine(Moveing());
    }

    public IEnumerator Moveing ()
    {
        Vector3 tmpV = positions[Random.Range(0, positions.Count - 1)];
        float currentSpeed = speed;

        if (tmpV.x < transform.position.x)
            mr.flipX = true;
        else
            mr.flipX = false;

        while (transform.position != tmpV)
        {
            currentSpeed += 0.05f;
            if (Vector2.Distance(transform.position, tmpV) < 0.05f)
            {
                transform.position = tmpV;
                break;
            }
            moveing = true;
            transform.position = Vector2.MoveTowards(transform.position, tmpV, Time.deltaTime * currentSpeed);
            yield return null;
        }
        moveing = false;
        anim.SetTrigger("Sleep");
    }
}
