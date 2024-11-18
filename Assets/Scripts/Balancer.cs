using UnityEngine;

public class Balancer : MonoBehaviour
{
    public float force = 20.0f;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
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