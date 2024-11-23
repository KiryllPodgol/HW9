using UnityEngine;

public class Balancer : MonoBehaviour
{
    public float force = 40.0f; // Сила отталкивания
    public float Speed = 100.0f; // Угловая скорость
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.angularVelocity = Speed;
    }

    void Update()
    {
        
        if (Mathf.Abs(rb.angularVelocity) < Speed)
        {
            rb.angularVelocity = Speed * Mathf.Sign(rb.angularVelocity);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Character character = collision.gameObject.GetComponent<Character>();

        if (character != null)
        {
            Rigidbody2D characterRb = character.GetComponent<Rigidbody2D>();

            if (characterRb != null)
            {
              
                Vector2 direction = (transform.position - collision.transform.position).normalized;

              
                characterRb.AddForce(direction * force, ForceMode2D.Impulse);

           
                character.ReceiveDamage();
            }
        }
    }
}