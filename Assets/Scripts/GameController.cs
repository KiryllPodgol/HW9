using UnityEngine;

public class GameСontroller : MonoBehaviour
{
    [SerializeField] private GameObject characterPrefab; // Префаб персонажа
    [SerializeField] private Transform playerSpawnPoint; // Позиция спавна персонажа
    [SerializeField] private HealthBar healthBar; // HP бар
    [SerializeField] private Transform respawnPoint; // Respawn точка

    private void Awake()
    {
        // Проверка на null
        if (characterPrefab == null)
        {
            Debug.Log("Character prefab is null");
            return;
        }

        if (playerSpawnPoint == null)
        {
            Debug.Log("Player spawn point is null");
            return;
        }

        if (healthBar == null)
        {
            Debug.Log("Health bar is null");
            return;
        }

        if (respawnPoint == null)
        {
            Debug.Log("Respawn point is null");
            return;
        }


        // Создание персонажа
        GameObject character = Instantiate(characterPrefab, playerSpawnPoint.position, Quaternion.identity);
        Character characterScript = character.GetComponent<Character>();

        // Инициализация HP бара и respawn точки
        characterScript.healthBar = healthBar;
        characterScript.respawnPoint = respawnPoint;


    }
}
