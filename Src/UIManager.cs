using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject hud;
    [SerializeField] private GameObject clearPanel;
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject settingsPanel;

    private void Start()
    {
        Time.timeScale = 1.0f;

        hud.SetActive(true);
        clearPanel.SetActive(false);
        pausePanel.SetActive(false);
        settingsPanel.SetActive(false);
    }

    public void OpenPause()
    {
        hud.SetActive(false);
        pausePanel.SetActive(true);
        Time.timeScale = 0.0f;
    }

    public void ClosePause()
    {
        hud.SetActive(true);
        pausePanel.SetActive(false);
        Time.timeScale = 1.0f;
    }

    public void OpenSettings()
    {
        pausePanel.SetActive(false);
        settingsPanel.SetActive(true);
    }

    public void CloseSettings()
    {
        settingsPanel.SetActive(false);
        pausePanel.SetActive(true);
    }

    public void OpenClear()
    {
        hud.SetActive(false);
        pausePanel.SetActive(false);
        clearPanel.SetActive(true);
    }
}
