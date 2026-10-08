using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    [SerializeField] private int lastStageBuildIndex = 7;

    public void RestartStage()
    {
        Time.timeScale = 1.0f;

        int clearedStage =
            PlayerPrefs.GetInt("ClearedStageIndex", 1);

        SceneManager.LoadScene(clearedStage);
    }

    public void RestartCurrentStage()
    {
        Time.timeScale = 1.0f;

        SceneManager.LoadScene(
            SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadNextStage()
    {
        Time.timeScale = 1.0f;

        int clearedStage =
            PlayerPrefs.GetInt("ClearedStageIndex", 1);

        int nextStage = clearedStage + 1;

        if(nextStage <= lastStageBuildIndex)
        {
            SceneManager.LoadScene(nextStage);
        }
        else
        {
            LoadMainMenu();
        }
    }
    public void LoadMainMenu()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene("MainMenu");
    }

    public void PlayGame()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene("MainScene01");
    }

    public void ShouResult()
    {
        Time.timeScale = 1.0f;

        int currentStage =
            SceneManager.GetActiveScene().buildIndex;

        PlayerPrefs.SetInt(
            "ClearedStageIndex",
            currentStage);

        PlayerPrefs.Save();

        SceneManager.LoadScene("Result");
    }
}
