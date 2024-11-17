using UnityEngine;
using UnityEngine.SceneManagement; // Для работы с загрузкой сцен

public class Character : Unit
{
    [SerializeField]
    private int lives = 3;

    public int Lives
    {
        get { return lives; }
        set
        {
            if (value < 3) lives = value;
            HealthBar.TakeDamage(lives - value); // Обновление здоровья
        }
    }

    public HealthBar HealthBar;
    public DeathScreen deathScreen; 

    public float speed = 3.0F;
    private float jumpForce = 15.0F;
    private bool isGrounded = false;
    private Bullet bullet;

    private CharState State
    {
        get { return (CharState)animator.GetInteger("State"); }
        set { animator.SetInteger("State", (int)value); }
    }

    new private Rigidbody2D rigidbody;
    private Animator animator;
    private SpriteRenderer sprite;

    private void Awake()
    {
        HealthBar = FindFirstObjectByType<HealthBar>();
        if (HealthBar == null)
        {
        
        }
        rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sprite = GetComponentInChildren<SpriteRenderer>();

        bullet = Resources.Load<Bullet>("Bullet");

        deathScreen = FindFirstObjectByType<DeathScreen>();
        if (deathScreen == null)
        {
 
        }
    }

    private void FixedUpdate()
    {
        CheckGround();
    }

    private void Update()
    {
        if (isGrounded) State = CharState.Idle;

        if (Input.GetButtonDown("Fire1")) Shoot();
        if (Input.GetButton("Horizontal")) Move();
        if (isGrounded && Input.GetButtonDown("Jump")) Jump();

        // Проверка на падение с платформы
        if (transform.position.y < -12)
        {
            PlayerDied();
        }
    }

    private void Move()
    {
        Vector3 direction = transform.right * Input.GetAxis("Horizontal");

        transform.position = Vector3.MoveTowards(transform.position, transform.position + direction, speed * Time.deltaTime);

        sprite.flipX = direction.x < 0.0F;

        if (isGrounded) State = CharState.Move;
    }

    private void Jump()
    {
        rigidbody.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
    }

    private void Shoot()
    {
        Vector3 position = transform.position; position.y += 0.8F;
        Bullet newBullet = Instantiate(bullet, position, bullet.transform.rotation) as Bullet;

        newBullet.Parent = gameObject;
        newBullet.Direction = newBullet.transform.right * (sprite.flipX ? -1.0F : 1.0F);
    }

    public override void ReceiveDamage()
    {
        HealthBar.TakeDamage(1); // Уменьшаем здоровье на 1

        lives--; // Уменьшаем количество жизней

        if (lives <= 0)
        {
            deathScreen.PlayerDied();
            return; 
        }

        rigidbody.linearVelocity = Vector2.zero; // Останавливаем движение
        rigidbody.AddForce(transform.up * 8.0F, ForceMode2D.Impulse);

        Debug.Log(lives);
    }

    private void CheckGround()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 0.3F);

        isGrounded = colliders.Length > 1;

        if (!isGrounded) State = CharState.Jump;
    }

    public void OnTriggerEnter2D(Collider2D collider)
    {
        Bullet bullet = collider.gameObject.GetComponent<Bullet>();
        if (bullet && bullet.Parent != gameObject)
        {
            ReceiveDamage(); // Обработка урона от пули
        }
    }

    private void PlayerDied()
    {
        if (deathScreen != null)
        {
            
            deathScreen.PlayerDied(); // Вызываем экран смерти
            Time.timeScale = 0; // Останавливаем время при смерти
         
            return; // Прерываем выполнение метода после вызова экрана смерти
        }


        Time.timeScale = 1; // Устанавливаем time scale в 1 только если deathScreen отсутствует
    }

    public void RestartGame()
    {
        
        Time.timeScale = 1; 
        lives = 3; 
       

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); 
    }
}

public enum CharState
{
    Idle,
    Move,
    Jump
}
