using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverWindowManager : MonoBehaviour
{
    [SerializeField] private GameObject gameOverPanel;

    [SerializeField] private GameObject inGameUI;

    [SerializeField] private string titleSceneName = "TitleScene";

    private void Start()
    {
        Time.timeScale = 1f;

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }

        if (inGameUI != null)
        {
            inGameUI.SetActive(true);
        }
    }

    public void ShowGameOverWindow()
    {
        if (inGameUI != null)
        {
            inGameUI.SetActive(false);
        }

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);

            Time.timeScale = 0f;
        }
    }

    public void OnRetryButtonClicked()
    {
        Time.timeScale = 1f;
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }

    public void OnTitleButtonClicked()
    {
        Time.timeScale = 1f;
        if (!string.IsNullOrEmpty(titleSceneName))
        {
            SceneManager.LoadScene(titleSceneName);
        }
    }
}
