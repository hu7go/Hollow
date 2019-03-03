using UnityEngine;

public class Wall : MonoBehaviour
{
    public SpriteRenderer myRenderer;
    BoxCollider2D myCollider;

    public Sprite[] wallSprites;
    public Sprite[] cornerSprites;
    public Sprite[] endSprites;
    public Sprite[] soloSprites;
    public Sprite openLeftAndRight;
    public Sprite openUpAndDown;

    private Quaternion right = Quaternion.Euler(0, 0, 270);
    private Quaternion down = Quaternion.Euler(0, 0, 180);
    private Quaternion left = Quaternion.Euler(0, 0, 90);

    [Space(20)]
    [Header("Sprites")]
    public Sprite[] innerWallSprites;

    public Sprite topLeftCorner;
    public Sprite topRightCorner;
    public Sprite bottomRightCorner;
    public Sprite bottomLeftCorner;

    public Sprite up01, up02;
    [Space(20)]

    public bool turnDown = false;
    public bool innerWall = false;

    private Quaternion rotation01 = new Quaternion(0, 0, 0, 0);
    private Quaternion rotationDown = new Quaternion(0, 0, 0, 0);

    public void Start()
    {
        if (innerWall)
        {
            myCollider = GetComponent<BoxCollider2D>();
            myCollider.enabled = false;

            Rotate(Random.Range(0,4), false);
            int randomNumber = Random.Range(0, 20);
            if (randomNumber > 1)
            {
                myRenderer.sprite = innerWallSprites[0];
            }
            if (randomNumber == 0)
            {
                myRenderer.sprite = innerWallSprites[1];
            }
            if (randomNumber == 1)
            {
                myRenderer.sprite = innerWallSprites[2];
            }
        }

        if (turnDown)
        {
            transform.rotation = rotation01;
        }
    }

    public void Rotate(int direction, bool wall)
    {
        int tmpNumber = Random.Range(0, wallSprites.Length);
        if (wall)
        {
            myRenderer.sprite = wallSprites[tmpNumber];
        }

        if (direction == 1)
        {
            return;
        }

        if (direction == 2)
        {
            transform.rotation = right;
            return;
        }

        if (direction == 3)
        {
            transform.rotation = down;
            return;
        }

        if (direction == 4)
        {
            transform.rotation = left;
            return;
        }
    }

    public void CornerLogic(int direction)
    {
        int tmpNumber = Random.Range(0, cornerSprites.Length);
        myRenderer.sprite = cornerSprites[tmpNumber];

        if (direction == 1)
            return;
        if (direction == 2)
            transform.rotation = right;
        if (direction == 3)
            transform.rotation = down;
        if (direction == 4)
            transform.rotation = left;
        if (direction == 5)
            myRenderer.sprite = openUpAndDown;
        if (direction == 6)
            myRenderer.sprite = openLeftAndRight;
    }

    public void SoloWall()
    {
        myRenderer.sprite = soloSprites[Random.Range(0, soloSprites.Length)];
    }

    public void EndWall(int direction)
    {
        myRenderer.sprite = endSprites[Random.Range(0, endSprites.Length)];

        if (direction == 1)
            return;
        if (direction == 2)
            transform.rotation = right;
        if (direction == 3)
            transform.rotation = down;
        if (direction == 4)
            transform.rotation = left;
    }
}
