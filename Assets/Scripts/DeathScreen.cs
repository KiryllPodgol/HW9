using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; 

public class DeathScreen : MonoBehaviour
{
    public GameObject deathPanel;
    public Button restartButton; 

    private void Start()
    {
        deathPanel.SetActive(false);
        restartButton.onClick?.AddListener(RestartGame);
      
            if (Time.timeScale == 0)
            {
                Time.timeScale = 1;
            }
        }

    private void OnDestroy()
    {
        restartButton.onClick?.RemoveListener(RestartGame);
    }

    public void PlayerDied()
    {
        deathPanel.SetActive(true);
        Time.timeScale = 0;
      
    }

    public void RestartGame()
    {
        
        Time.timeScale = 1;
       
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Исправлено
    }
}
