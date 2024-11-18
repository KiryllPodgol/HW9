using UnityEngine;
using System.Collections;

public class MovedRobot : Unit
{
    public float speed = 2.0F;
    public Transform[] points;
    public float visionRadius = 5.0F;
    public LayerMask groundLayer;
    public Animator animator;


    private int currentPointIndex = 0;
    private bool isWaiting = false;
    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;

        if (points.Length > 0)
        {
            transform.position = points[0].position;
            SetNextPoint();
        }
    }

    private void Update()
    {
        if (!isWaiting && points.Length > 0)
        {
            Move();
        }
    }

    private void OnTriggerEnter2D(Collider2D boxCollider2D)
    {
        Character character = boxCollider2D.GetComponent<Character>();

        if (character != null)
        {
            if (Mathf.Abs(character.transform.position.x - transform.position.x) < 0.3F &&
                character.transform.position.y > transform.position.y)
            {
                Destroy(gameObject);
            }
            else
            {
               
                character.ReceiveDamage(); 
            }
        }
    }

    private void Move()
    {
        if (IsOnTile())
        {
            OnStartMoving();
            Vector3 targetPosition = points[currentPointIndex].position;

            transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

            if (Vector3.Distance(transform.position, targetPosition) < 0.2F)
            {
                StartCoroutine(WaitAtPoint());
            }
        }
        else
        {
            OnStopMoving();
        }
    }

    private IEnumerator WaitAtPoint()
    {
        OnStopMoving();
        isWaiting = true;
        yield return new WaitForSeconds(2.0f);
        SetNextPoint();
        isWaiting = false;
    }

    private void SetNextPoint()
    {
        currentPointIndex = (currentPointIndex + 1) % points.Length;
    }

    private bool IsOnTile()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 1.5f, groundLayer);
        return hit.collider != null;
    }



    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, visionRadius);

        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * 1.5f);
    }

    public void OnStartMoving()
    {
        animator.SetBool("isMoving", true);
    }

    public void OnStopMoving()
    {
        animator.SetBool("isMoving", false);
    }
}