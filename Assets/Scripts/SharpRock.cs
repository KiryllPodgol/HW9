using UnityEngine;
using UnityEngine.TextCore.Text;

public class SharpRock : MonoBehaviour
{
     
    public float knockbackForce = 5f; // Сила отбрасывания

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Проверяем, является ли объект персонажем
        if (other.CompareTag("Player"))
        {
            // Получаем компонент Character (или ваш класс персонажа)
            CharacterControll character = other.GetComponent<CharacterControll>();
            if (character != null)
            {
                // Наносим урон персонажу
                character.TakeDamage(1);

                // Определяем направление отбрасывания
                Vector2 knockbackDirection = (other.transform.position - transform.position).normalized;
                Rigidbody2D rb = other.GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    rb.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);
                }
            }
        }
    }
}