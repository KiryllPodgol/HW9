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
        set => _direction = value.normalized; // ����������� �����������
    }

    public Color Color
    {
        set => _sprite.color = value;
    }

    private SpriteRenderer _sprite;
    private Rigidbody2D _rigidbody;

    private void Awake()
    {
        _sprite = GetComponentInChildren<SpriteRenderer>();
        _rigidbody = GetComponent<Rigidbody2D>(); // �������� ��������� Rigidbody2D
    }

    private void Start()
    {
        Destroy(gameObject, 1.4F);

        // ������������� �������� ���� ��� ������
        if (_rigidbody != null)
        {
            _rigidbody.linearVelocity = _direction * _speed; // ��������� �������� � Rigidbody
        }
    }

    private void OnTriggerEnter2D(Collider2D boxCollider2D)
    {
        Unit unit = boxCollider2D.GetComponent<Unit>();

        if (unit && unit.gameObject != _parent)
        {
            Destroy(gameObject);
            // ����� ����� �������� ������ ��� ��������� �����
            unit.ReceiveDamage(); // ��������������, ��� � ������ Unit ���� ����� TakeDamage()
        }
    }
}
