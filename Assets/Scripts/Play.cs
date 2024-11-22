using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Play : MonoBehaviour
{
    public Button playButton;

    void Awake()
    {
        
        playButton.onClick?.AddListener(LoadGameScene);
    }

    private void OnDestroy()
    {
       
        playButton.onClick?.RemoveListener(LoadGameScene);
    }


    void LoadGameScene()
    {
        SceneManager.LoadScene("GameScene"); 
    }
}
