using UnityEngine;

public class HealthBar : MonoBehaviour
{
    public GameObject[] hearts; // ������ ��������
    private int maxHealth; // ������������ ��������
    private int currentHealth; // ������� ��������

    void Start()
    {
        maxHealth = hearts.Length; // ������������� ������������ �������� ������ ���������� ��������
        currentHealth = maxHealth; // �������������� ������� �������� �� ������������
        UpdateHearts(); // ��������� ����������� �������� ��� ������
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage; // ��������� ������� ��������
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // ������������ �������� �� 0 �� maxHealth

        UpdateHearts(); // ��������� ����������� ��������
    }

    private void UpdateHearts()
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            hearts[i].SetActive(i < currentHealth); // ���������� ��� ������������ �������� � ����������� �� �������� ��������
        }
    }

    public int CurrentHealth => currentHealth; // �������� ��� ��������� �������� ��������

    public void SetMaxHealth(int health)
    {
        maxHealth = health;
        currentHealth = health;
        UpdateHearts(); // ��������� ����������� �������� ��� ��������� ������������� ��������
    }

    public void AddHeart() // ����� ��� ���������� ������
    {
        if (maxHealth < hearts.Length) // ���������, �� ��������� �� ������������ ���������� ������ ���������� ��������
        {
            maxHealth++; // ����������� ������������ ��������
            SetMaxHealth(maxHealth); // ������������� ����� ������������ ��������
            Debug.Log($"Added a heart! Current max health: {maxHealth}");
        }
        else
        {
            Debug.Log("Max health reached. Cannot add more hearts.");
        }
    }
}