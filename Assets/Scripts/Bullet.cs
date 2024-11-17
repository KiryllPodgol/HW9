using UnityEngine;

public class Bullet : MonoBehaviour
{
    private GameObject _parent;
    public GameObject Parent { set { _parent = value; } get { return _parent; } }

    private float _speed = 10.0F;
    private Vector3 _direction;
    public Vector3 Direction { set { _direction = value; } }

    public Color Color
    {
        set { _sprite.color = value; }
    }

    private SpriteRenderer _sprite;

    private void Awake()
    {
        _sprite = GetComponentInChildren<SpriteRenderer>();
    }

    private void Start()
    {
        Destroy(gameObject, 1.4F);
    }


    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, transform.position + _direction, _speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D boxCollider2D)
    {
        Unit unit = boxCollider2D.GetComponent<Unit>();

        if (unit && unit.gameObject != _parent)
        {
            Destroy(gameObject);
        }
}
}
