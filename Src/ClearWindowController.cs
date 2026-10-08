using UnityEngine;
using UnityEngine.SceneManagement;

public class ClearWindowController : MonoBehaviour
{
    [SerializeField] private GameObject clearPanel;

    [SerializeField] private GameObject inGameUI;

    [SerializeField] private string nextStageName;

    [SerializeField] private string titleSceneName = "TitleScene";

    private void Start()
    {
        Time.timeScale = 1f;

        if (clearPanel != null)
        {
            clearPanel.SetActive(false);
        }

        if (inGameUI != null)
        {
            inGameUI.SetActive(true);
        }
    }

    public void ShowClearWindow()
    {
        if (inGameUI != null)
        {
            inGameUI.SetActive(false);
        }

        if (clearPanel != null)
        {
            clearPanel.SetActive(true);

            Time.timeScale = 0f;
        }
    }

    public void OnNextStageButtonClicked()
    {
        if (!string.IsNullOrEmpty(nextStageName))
        {
            SceneManager.LoadScene(nextStageName);
        }
    }

    public void OnRetryButtonClicked()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }

    public void OnTitleButtonClicked()
    {
        if (!string.IsNullOrEmpty(titleSceneName))
        {
            SceneManager.LoadScene(titleSceneName);
        }
    }
}