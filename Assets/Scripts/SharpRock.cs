using UnityEngine;
using UnityEngine.TextCore.Text;

public class SharpRock : MonoBehaviour
{
     
    public float knockbackForce = 5f; // ���� ������������

    private void OnTriggerEnter2D(Collider2D other)
    {
        // ���������, �������� �� ������ ����������
        if (other.CompareTag("Player"))
        {
            // �������� ��������� Character (��� ��� ����� ���������)
            CharacterControll character = other.GetComponent<CharacterControll>();
            if (character != null)
            {
                // ������� ���� ���������
                character.TakeDamage(1);

                // ���������� ����������� ������������
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