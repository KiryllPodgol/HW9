using UnityEngine;

public class Game—ontroller : MonoBehaviour
{
    [SerializeField] private GameObject characterPrefab;
    [SerializeField] private Transform playerSpawnPoint; 
    [SerializeField] private HealthBar healthBar; 
    [SerializeField] private Transform respawnPoint; 

    private void Awake()
    {
       
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


        HealthBar healthBarInstantiated = Instantiate(healthBar, Vector3.zero, Quaternion.identity);

        GameObject character = Instantiate(characterPrefab, playerSpawnPoint.position, Quaternion.identity);
        Character characterScript = character.GetComponent<Character>();

        characterScript.healthBar = healthBarInstantiated;
        characterScript.respawnPoint = respawnPoint;
 
        CameraControl cameraControl = Camera.main.GetComponent<CameraControl>();
        if (cameraControl != null)
        {
            cameraControl.target = character.transform;
        }
        else
        {
            Debug.LogError("CameraControl script not found on the Main Camera!");
        }
    }
}