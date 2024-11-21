using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class Character : Unit
{
    private const float RespawnYThreshold = -12f;
    private const int MaxLives = 3;
    private const float JumpForce = 15.0f;
    private const float BulletOffsetY = 0.8f;
    private const float ReboundForce = 8.0f;

    [Header("Settings")]
    [SerializeField] private float _speed = 3.0f;
    [SerializeField] private float _shootCooldown = 0.5f;

    [Header("References")]
    [SerializeField] private Transform _respawnPoint;
    [SerializeField] private HealthBar _healthBar;
    [SerializeField] private DeathScreen _deathScreen;
    [SerializeField] private Bullet _bulletPrefab;
    [SerializeField] private Transform _bulletSpawnPoint;

    private int _lives = MaxLives;
    private Rigidbody2D _rigidbody;
    private Animator _animator;
    private SpriteRenderer _sprite;
    private bool _isGrounded;
    private Vector2 _moveInput;
    private float _lastShootTime;

   
    private InputAction _moveAction;
    private InputAction _jumpAction;
    private InputAction _shootAction;

    private CharState State
    {
        get => (CharState)_animator.GetInteger("State");
        set => _animator.SetInteger("State", (int)value);
    }

    public int Lives
    {
        get => _lives;
        set
        {
            if (value < 0 || value > MaxLives) return;
            _healthBar?.TakeDamage(_lives - value);
            _lives = value;
        }
    }

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _sprite = GetComponentInChildren<SpriteRenderer>();



    
        InitializeInputActions();

   
        ValidateReferences();
    }

    private void InitializeInputActions()
    {

        InputActionMap gameplayActionMap = InputSystem.actions.FindActionMap("Gameplay");

        if (gameplayActionMap != null)
        {

            _moveAction = gameplayActionMap["Move"];
            _jumpAction = gameplayActionMap["Jump"];
            _shootAction = gameplayActionMap["Shoot"];
        }
        else
        {
            Debug.LogError("Gameplay Action Map not found!");
        }
    }

    private void ValidateReferences()
    {
        if (_healthBar == null) Debug.LogError("HealthBar is not assigned in the Inspector!");
        if (_deathScreen == null) Debug.LogError("DeathScreen is not assigned in the Inspector!");
        if (_bulletPrefab == null) Debug.LogError("BulletPrefab is not assigned in the Inspector!");
        if (_bulletSpawnPoint == null) Debug.LogError("BulletSpawnPoint is not assigned in the Inspector!");
    }

    private void OnEnable()
    {
        _jumpAction.performed += OnJump;
        _jumpAction.Enable();
        _shootAction.performed += OnShoot;
       
        _moveAction.Enable();
    }

    private void OnDisable()
    {

        _jumpAction.performed -= OnJump;
        _jumpAction.Disable();
        _shootAction.performed -= OnShoot;
        _shootAction.Disable();
    }

    private void FixedUpdate()
    {
        CheckGround();
        Move();
    }

    private void Update()
    {
        if (_isGrounded) State = CharState.Idle;

        // Проверка на падение ниже уровня
        if (transform.position.y < RespawnYThreshold)
        {
            ReceiveDamage();
            Respawn();
        }
    }

    private void Move()
    {
        _moveInput = _moveAction.ReadValue<Vector2>();
        Debug.Log($"Move Input: {_moveInput}");

        Vector3 direction = new(_moveInput.x, 0, 0);

        // Используем Translate для движения
        transform.Translate(_speed * Time.deltaTime * direction);

        // Переключаем анимацию
        _sprite.flipX = direction.x < 0.0f;

        if (_isGrounded && Mathf.Abs(_moveInput.x) > Mathf.Epsilon)
        {
            State = CharState.Move;
        }
    }

    private void OnJump(InputAction.CallbackContext context)
    {
        if (_isGrounded)
        {
            // Применение силы прыжка
            _rigidbody.AddForce(Vector2.up * JumpForce, ForceMode2D.Impulse);
            State = CharState.Jump; // Изменение состояния при прыжке
        }
    }

    private void OnShoot(InputAction.CallbackContext context)
    {
        if (Time.time - _lastShootTime < _shootCooldown) return;

        Vector3 bulletPosition =
            (_bulletSpawnPoint != null) ?
            _bulletSpawnPoint.position :
            transform.position + new Vector3(0, BulletOffsetY, 0);

        Bullet newBullet = Instantiate(_bulletPrefab, bulletPosition, Quaternion.identity);

        newBullet.Parent = gameObject;
        newBullet.Direction = newBullet.transform.right * (_sprite.flipX ? -1.0f : 1.0f);

        _lastShootTime = Time.time;
    }

    public override void ReceiveDamage()
    {
        Lives--;

        if (Lives <= 0)
            PlayerDied();
        else
            HandleDamageRebound();
    }

    private void HandleDamageRebound()
    {

        _rigidbody.linearVelocity = Vector2.zero;
        _rigidbody.AddForce(Vector2.up * ReboundForce, ForceMode2D.Impulse);
    }

    private void CheckGround()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 0.3f);
        _isGrounded = colliders.Length > 1;

        if (!_isGrounded) State = CharState.Jump;
    }

    private void Respawn()
    {
        if (_respawnPoint != null)
        {
            transform.position = _respawnPoint.position;
            _rigidbody.linearVelocity = Vector2.zero;
        }
    }

    private void PlayerDied()
    {

        _deathScreen?.PlayerDied();
        Time.timeScale = 0;
    }

    public void RestartGame()
    {
        Time.timeScale = 1;
        Lives = MaxLives;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}

public enum CharState
{
    Idle,
    Move,
    Jump
}