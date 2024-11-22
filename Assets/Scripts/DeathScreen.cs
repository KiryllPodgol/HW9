using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DeathScreen : MonoBehaviour
{
    public static DeathScreen Instance { get; private set; }

    public GameObject deathPanel;
    public Button restartButton;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        deathPanel.SetActive(false);
        restartButton.onClick.AddListener(RestartGame);
    }

    private void OnDestroy()
    {
        restartButton.onClick.RemoveListener(RestartGame);
    }

    public void PlayerDied()
    {
        deathPanel.SetActive(true);
        Time.timeScale = 0;
    }

    public void RestartGame()
    {
        deathPanel.SetActive(false);
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
