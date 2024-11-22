using UnityEngine;

public class Game�ontroller : MonoBehaviour
{
    [SerializeField] private GameObject characterPrefab; // ������ ���������
    [SerializeField] private Transform playerSpawnPoint; // ������� ������ ���������
    [SerializeField] private HealthBar healthBar; // HP ���
    [SerializeField] private Transform respawnPoint; // Respawn �����

    private void Awake()
    {
        // �������� �� null
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


        // �������� ���������
        GameObject character = Instantiate(characterPrefab, playerSpawnPoint.position, Quaternion.identity);
        Character characterScript = character.GetComponent<Character>();

        // ������������� HP ���� � respawn �����
        characterScript.healthBar = healthBar;
        characterScript.respawnPoint = respawnPoint;


    }
}
