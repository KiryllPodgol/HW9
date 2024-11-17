using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

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
            healthBar.TakeDamage(lives - value); // Обновление здоровья
        }
    }

    [FormerlySerializedAs("HealthBar")] public HealthBar healthBar;
    public DeathScreen deathScreen; 

    public float speed = 3.0F;
    private float _jumpForce = 15.0F;
    private bool _isGrounded = false;
    private Bullet _bullet;

    private CharState State
    {
        get { return (CharState)_animator.GetInteger("State"); }
        set { _animator.SetInteger("State", (int)value); }
    }

   private Rigidbody2D _rigidbody;
    private Animator _animator;
    private SpriteRenderer _sprite;

    private void Awake()
    {
        healthBar = FindFirstObjectByType<HealthBar>();
        if (healthBar == null)
        {
        
        }
        _rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _sprite = GetComponentInChildren<SpriteRenderer>();

        _bullet = Resources.Load<Bullet>("Bullet");

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
        if (_isGrounded) State = CharState.Idle;

        if (Input.GetButtonDown("Fire1")) Shoot();
        if (Input.GetButton("Horizontal")) Move();
        if (_isGrounded && Input.GetButtonDown("Jump")) Jump();

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

        _sprite.flipX = direction.x < 0.0F;

        if (_isGrounded) State = CharState.Move;
    }

    private void Jump()
    {
        _rigidbody.AddForce(transform.up * _jumpForce, ForceMode2D.Impulse);
    }

    private void Shoot()
    {
        Vector3 position = transform.position; position.y += 0.8F;
        Bullet newBullet = Instantiate(_bullet, position, _bullet.transform.rotation) as Bullet;

        newBullet.Parent = gameObject;
        newBullet.Direction = newBullet.transform.right * (_sprite.flipX ? -1.0F : 1.0F);
    }

    public override void ReceiveDamage()
    {
        healthBar.TakeDamage(1); // Уменьшаем здоровье на 1

        lives--; // Уменьшаем количество жизней

        if (lives <= 0)
        {
            deathScreen.PlayerDied();
            return; 
        }

        _rigidbody.linearVelocity = Vector2.zero; // Останавливаем движение
        _rigidbody.AddForce(transform.up * 8.0F, ForceMode2D.Impulse);

        
    }

    private void CheckGround()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 0.3F);

        _isGrounded = colliders.Length > 1;

        if (!_isGrounded) State = CharState.Jump;
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
