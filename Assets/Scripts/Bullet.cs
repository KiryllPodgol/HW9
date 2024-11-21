using UnityEngine;

public class Bullet : MonoBehaviour
{
    private GameObject _parent;
    public GameObject Parent
    {
        set => _parent = value;
        get => _parent;
    }

    private float _speed = 10.0F;
    private Vector3 _direction;
    public Vector3 Direction
    {
        set => _direction = value.normalized;
    }

    public Color Color
    {
        set => _sprite.color = value;
    }

    private SpriteRenderer _sprite;
    private Rigidbody2D _rigidbody;
    private float _lifetime = 1.0F;

    private void Awake()
    {
        _sprite = GetComponentInChildren<SpriteRenderer>();
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        Destroy(gameObject, _lifetime);

      
        if (_rigidbody != null)
        {
            _rigidbody.linearVelocity = _direction * _speed; 
        }
        else
        {
            Debug.LogError("Rigidbody2D не найден на пуле.");
        }
    }

    private void OnTriggerEnter2D(Collider2D boxCollider2D)
    {
        Unit unit = boxCollider2D.GetComponent<Unit>();

        if (unit && unit.gameObject != _parent)
        {
            Destroy(gameObject);
            unit.ReceiveDamage();
        }
    }
}
