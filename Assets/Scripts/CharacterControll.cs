using UnityEngine;

public class CharacterControll : MonoBehaviour
{
    public float Speed = 3f; // Скорость передвижения
    public float jumpForce = 15f; // Сила прыжка
    public int lives = 3; // Текущие жизни

    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer sprite;
    private bool isGrounded;

    public HealthBar healthBar; // Ссылка на компонент HealthBar

    private CharState State
    {
        get { return (CharState)animator.GetInteger("State"); }
        set
        {
            animator.SetInteger("State", (int)value);
            Debug.Log("State changed to: " + value); // Логирование изменения состояния
        }
    }

    public enum CharState
    {
        Idle,
        Move,
        Jump
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sprite = GetComponentInChildren<SpriteRenderer>();

        if (healthBar != null)
        {
            healthBar.TakeDamage(0); // Инициализация здоровья
        }
    }

    private void FixedUpdate()
    {
        CheckGround();
    }

    void Update()
    {
        Move();

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            Jump();
        }

        if (isGrounded)
        {
            float moveInput = Input.GetAxis("Horizontal");
            if (Mathf.Abs(moveInput) > 0)
            {
                State = CharState.Move;
            }
            else
            {
                State = CharState.Idle;
            }
        }
    }

    void Move()
    {
        float moveInput = Input.GetAxis("Horizontal");
        Vector3 direction = transform.right * moveInput;

        transform.position = Vector3.MoveTowards(transform.position, transform.position + direction, Speed * Time.deltaTime);

        if (moveInput != 0)
        {
            sprite.flipX = moveInput < 0;
        }
    }

    void Jump()
    {
        rb.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
        State = CharState.Jump; // Устанавливаем состояние прыжка
    }

    public void TakeDamage(int amount)
    {
        if (healthBar != null)
        {
            healthBar.TakeDamage(amount); // Уменьшаем здоровье в HealthBar
            Debug.Log($"Current Health: {healthBar.CurrentHealth}"); // Выводим текущее здоровье
        }

        // Логика смерти персонажа
        if (healthBar != null && healthBar.CurrentHealth <= 0)
        {
            Die(); // Вызываем метод смерти
        }
    }

    public void AddLives(int amount)
    {
        lives += amount; // Увеличиваем количество жизней
        Debug.Log($"Lives increased! Current lives: {lives}");

        // Здесь вы можете добавить логику для обновления UI или других элементов игры.
    }

    void CheckGround()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 0.3f);
        isGrounded = colliders.Length > 1;

        if (!isGrounded)
        {
            State = CharState.Jump; // Устанавливаем состояние прыжка
        }
        else if (isGrounded && State == CharState.Jump) // Если персонаж приземляется из состояния Jump
        {
            State = Mathf.Abs(Input.GetAxis("Horizontal")) > 0 ? CharState.Move : CharState.Idle; // Возвращаемся в Idle или Move
        }
    }

    private void Die()
    {
        Debug.Log($"{gameObject.name} has died.");
        // Добавьте логику для смерти персонажа, например, перезапуск уровня или отображение меню.
    }
}