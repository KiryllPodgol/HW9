using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class Character : Unit
{
    private const float RespawnYThreshold = -12f;
    private const int MaxLives = 3;
    private const float JumpForce = 15.0f;
    private const float ReboundForce = 8.0f;
  
    InputAsset _input;

    [Header("Settings")]
    [SerializeField] private float _speed = 3.0f;
    [SerializeField] private float _shootCooldown = 0.2f;

    [Header("References")]
    [SerializeField] private Transform _respawnPoint;
    [SerializeField] private HealthBar _healthBar;
    [SerializeField] private DeathScreen _deathScreen;
    [SerializeField] private Bullet _bullet;
    [SerializeField] private Transform _bulletFirePoint;

    private int _lives = MaxLives;
    private Rigidbody2D _rigidbody;
    private Animator _animator;
    private SpriteRenderer _sprite;
    private bool _isGrounded;
    private Vector2 _moveInput;
    private float _lastShootTime;

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
        ValidateReferences();
    }
    private void ValidateReferences()
    {
        if (_healthBar == null) Debug.LogError("HealthBar is not assigned in the Inspector!");
        if (_deathScreen == null) Debug.LogError("DeathScreen is not assigned in the Inspector!");
        if (_bullet == null) Debug.LogError("BulletPrefab is not assigned in the Inspector!");
        if (_bulletFirePoint == null) Debug.LogError("BulletSpawnPoint is not assigned in the Inspector!");
    }

    private void OnEnable()
    {
        if (_input == null)
        {
            _input = new InputAsset();
        }

        _input.Enable();
        _input.Gameplay.Jump.performed += Jump_performed;
        _input.Gameplay.Shoot.performed += Shoot_performed;
    }

    private void Shoot_performed(InputAction.CallbackContext obj)
    {


        if (Time.time - _lastShootTime < _shootCooldown)
            return;

        _lastShootTime = Time.time;

        if (_bullet == null)
        {
            Debug.LogError("Префаб пули не назначен.");
            return;
        }

        Bullet newBullet = Instantiate(_bullet, _bulletFirePoint.position, Quaternion.identity).GetComponent<Bullet>();

        if (newBullet != null)
        {
            Vector3 bulletDirection = _sprite.flipX ? Vector3.left : Vector3.right;
            newBullet.Direction = bulletDirection;
            newBullet.Parent = gameObject;
        }
        else
        {
            Debug.LogError("Не удалось создать экземпляр пули.");
        }
    }

    private void Jump_performed(InputAction.CallbackContext ctn)
    {
        if (_isGrounded)
        {

            _rigidbody.AddForce(Vector2.up * JumpForce, ForceMode2D.Impulse);
            State = CharState.Jump;
        }
    }

    private void OnDisable()
    {

        if (_input == null)
        {
            return;
        }
        _input.Gameplay.Jump.performed -= Jump_performed;
        _input.Gameplay.Shoot.performed -= Shoot_performed;
        _input.Disable();
    }

    private void FixedUpdate()
    {
        CheckGround();

        float currentVerticalSpeed = _rigidbody.linearVelocity.y;
        _moveInput = _input.Gameplay.Move.ReadValue<Vector2>();
        Vector2 velocity = new Vector2(_moveInput.x * _speed, currentVerticalSpeed);
        _rigidbody.linearVelocity = velocity;
        if (_moveInput.x != 0)
            _sprite.flipX = _moveInput.x < 0.0f;

        if (_isGrounded && Mathf.Abs(_moveInput.x) > Mathf.Epsilon)
        {
            State = CharState.Move;
        }

        if (_isGrounded && Mathf.Abs(_moveInput.x) < Mathf.Epsilon)
            State = CharState.Idle;
    }

    private void Update()
    {
        if (transform.position.y < RespawnYThreshold)
        {
            ReceiveDamage();
            Respawn();
        }
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
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 0.1f);
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