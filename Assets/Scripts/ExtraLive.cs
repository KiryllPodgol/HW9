using UnityEngine;

public class ExtraLife : MonoBehaviour
{
    public int lifeAmount = 1; 

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Проверяем, является ли объект персонажем
        CharacterControll character = other.GetComponent<CharacterControll>();
        if (character != null)
        {
            
            character.AddLives(lifeAmount);

            
            if (character.healthBar != null)
            {
                character.healthBar.AddHeart(); 
            }

         
            Destroy(gameObject);
        }
    }
}