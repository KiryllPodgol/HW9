using UnityEngine;
using System.Collections;

public class Obstacle : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collider)
    {
        Character character = collider.gameObject.GetComponent<Character>();
        if (character != null)
        {
            character.ReceiveDamage();
        }
        else
        {
          
        }
    }
}