using UnityEngine;
using System.Collections;

public class MovedMonster : Unit
{
    public float speed = 2.0F;
    public Transform[] points; // ������ ����� ��� ��������
    public float visionRadius = 5.0F; // ������ ��������� �������
    public LayerMask groundLayer; // ���� ����� ��� ��������
    public Animator animator; // ������ �� ��������� Animator
    private int currentPointIndex = 0;
    private Vector3 _direction;
    private bool isWaiting = false;
    private Transform target; // ���� ��� �������������
    private Rigidbody2D rb; // ������ �� Rigidbody2D

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();


        if (points.Length > 0)
        {
            transform.position = points[0].position;
            SetNextPoint();
        }
    }

    private void Update()
    {
        if (target != null)
        {
            ChaseTarget();
        }
        else if (points.Length > 0 && !isWaiting)
        {
            Move();
        }
        CheckForTarget();

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
                character.HealthBar.TakeDamage(1);
            }
        }
    }

    private void Move()
    {
        transform.position = Vector3.MoveTowards(transform.position, points[currentPointIndex].position, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, points[currentPointIndex].position) < 0.1F)
        {
            StartCoroutine(WaitAtPoint());
        }
    }

    private void ChaseTarget()
    {
        transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, target.position) < 0.1F)
        {
            // ������ ����� ���������
            Character character = target.GetComponent<Character>();
            if (character != null)
            {
                character.HealthBar.TakeDamage(1);
            }
        }
    }

    private IEnumerator WaitAtPoint()
    {
        isWaiting = true;
        yield return new WaitForSeconds(2.0f); 
        SetNextPoint();
        isWaiting = false;
    }

    private void SetNextPoint()
    {
        currentPointIndex = Random.Range(0, points.Length); 
        _direction = (points[currentPointIndex].position - transform.position).normalized;
    }

    private void CheckForTarget()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, visionRadius);
        foreach (var hit in hits)
        {
            Character character = hit.GetComponent<Character>();
            if (character != null)
            {
                target = character.transform; // ������������� ���� ��� �������������
                break;
            }
        }
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, visionRadius); // ����������� ������� ��������� � ���������
    }

    // ������ ��� ������������ �������
    public void OnStartMoving()
    {
        animator.SetBool("isMoving", true);
    }

    public void OnStopMoving()
    {
        animator.SetBool("isMoving", false);
    }
}
