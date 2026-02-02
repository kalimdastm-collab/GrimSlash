using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    public float speed = 4f;

    private Animator anim;
    private bool isDead = false;
    private bool moveAnimSet = false;

    private Vector3 middleBottomScreen;
    private Vector3 bottomCenterTemple;
    private bool reachedMiddle = false;
    private bool spawnLeft;

    public float beforeMiddleOffset = 0.5f;

    void Start()
    {
        anim = GetComponent<Animator>();

        Vector3 bottomLeft = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0));
        Vector3 bottomRight = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, 0));
        float middleX = (bottomLeft.x + bottomRight.x) / 2 - beforeMiddleOffset;
        middleBottomScreen = new Vector3(middleX, bottomLeft.y + 0.5f, 0);

        Transform target = GameObject.Find("Temple").transform;
        SpriteRenderer sr = target.GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            Bounds b = sr.bounds;
            bottomCenterTemple = new Vector3(b.center.x, b.min.y, target.position.z);
        }
        else
        {
            bottomCenterTemple = target.position;
        }
    }

    void Update()
    {
        if (isDead) return;

        if (!moveAnimSet && anim != null)
        {
            anim.SetBool("MoveTrigger", true);
            moveAnimSet = true;
        }

        if (!reachedMiddle)
        {
            transform.position = Vector2.MoveTowards(transform.position, middleBottomScreen, speed * Time.deltaTime);
            if (Vector2.Distance(transform.position, middleBottomScreen) < 3f)
            {
                reachedMiddle = true;
            }
        }
        else
        {
            transform.position = Vector2.MoveTowards(transform.position, bottomCenterTemple, speed * Time.deltaTime);
        }
    }
    void CalculateTargets()
    {
        Vector3 bottomLeft = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0));
        Vector3 bottomRight = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, 0));

        float middleX = (bottomLeft.x + bottomRight.x) / 2;

        middleX += spawnLeft ? -beforeMiddleOffset : beforeMiddleOffset;

        middleBottomScreen = new Vector3(middleX, bottomLeft.y + 0.5f, 0);

        Transform temple = GameObject.Find("Temple").transform;
        SpriteRenderer sr = temple.GetComponent<SpriteRenderer>();

        if (sr != null)
        {
            Bounds b = sr.bounds;
            bottomCenterTemple = new Vector3(b.center.x, b.min.y, temple.position.z);
        }
        else
        {
            bottomCenterTemple = temple.position;
        }
    }
    public void SetDirection(bool fromLeft)
    {
        spawnLeft = fromLeft;
        CalculateTargets();
    }

    public void StopMoving()
    {
        isDead = true;
        if (anim != null)
            anim.SetBool("MoveTrigger", false);
    }

    public void SetSpeed(float newSpeed)
    {
        speed = newSpeed;
    }

}