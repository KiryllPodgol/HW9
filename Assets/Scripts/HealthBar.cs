using System;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    public GameObject[] hearts;
    private int _maxHealth;
    private int _currentHealth;

    void Start()
    {
        _maxHealth = hearts.Length;
        _currentHealth = _maxHealth;
        UpdateHearts();
    }

    public void TakeDamage(int damage)
    {
        _currentHealth -= damage;
        _currentHealth = Mathf.Clamp(_currentHealth, 0, _maxHealth);
        UpdateHearts();
    }

    private void UpdateHearts()
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            hearts[i].SetActive(i < _currentHealth);
        }
    }

    public int CurrentHealth => _currentHealth;

    public void SetMaxHealth(int health)
    {
        _maxHealth = health;
        _currentHealth = Mathf.Min(health, _currentHealth);
        UpdateHearts();
    }

    public void AddHeart()
    {
        if (_maxHealth < hearts.Length - 1)
        {
            _maxHealth++;
            SetMaxHealth(_maxHealth);
  
        }
        else
        {

        }
    }
}