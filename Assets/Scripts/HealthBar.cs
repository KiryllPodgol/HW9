using UnityEngine;

public class HealthBar : MonoBehaviour
{
    public GameObject[] hearts; // ћассив сердечек
    private int maxHealth; // ћаксимальное здоровье
    private int currentHealth; // “екущее здоровье

    void Start()
    {
        maxHealth = hearts.Length; // ”станавливаем максимальное здоровье равным количеству сердечек
        currentHealth = maxHealth; // »нициализируем текущее здоровье на максимальное
        UpdateHearts(); // ќбновл€ем отображение сердечек при старте
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage; // ”меньшаем текущее здоровье
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // ќграничиваем здоровье от 0 до maxHealth

        UpdateHearts(); // ќбновл€ем отображение сердечек
    }

    private void UpdateHearts()
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            hearts[i].SetActive(i < currentHealth); // јктивируем или деактивируем сердечки в зависимости от текущего здоровь€
        }
    }

    public int CurrentHealth => currentHealth; // —войство дл€ получени€ текущего здоровь€

    public void SetMaxHealth(int health)
    {
        maxHealth = health;
        currentHealth = health;
        UpdateHearts(); // ќбновл€ем отображение сердечек при установке максимального здоровь€
    }

    public void AddHeart() // ћетод дл€ добавлени€ сердца
    {
        if (maxHealth < hearts.Length) // ѕровер€ем, не превышает ли максимальное количество жизней количество сердечек
        {
            maxHealth++; // ”величиваем максимальное здоровье
            SetMaxHealth(maxHealth); // ”станавливаем новое максимальное здоровье
            Debug.Log($"Added a heart! Current max health: {maxHealth}");
        }
        else
        {
            Debug.Log("Max health reached. Cannot add more hearts.");
        }
    }
}