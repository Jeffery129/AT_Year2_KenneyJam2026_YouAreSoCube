using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleUIManager : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject titlePanel;
    public GameObject settingsPanel;

    [Header("Volume Sliders")]
    public Slider bgmSlider;
    public Slider seSlider;

    private void Start()
    {
        ShowTitleScreen();

        bgmSlider.onValueChanged.AddListener(OnBgmVolumeChanged);
        seSlider.onValueChanged.AddListener(OnSeVolumeChanged);
    }

    public void OnStartButtonClicked()
    {
        SceneManager.LoadScene("MainScene01");
    }

    public void OnSettingsButtonClicked()
    {
        titlePanel.SetActive(false);
        settingsPanel.SetActive(true);
    }

    public void OnBackButtonClicked()
    {
        ShowTitleScreen();
    }

    private void ShowTitleScreen()
    {
        titlePanel.SetActive(true);
        settingsPanel.SetActive(false);
    }

    private void OnBgmVolumeChanged(float volume)
    {
        SoundManager.Instance.SetBGMVolume(volume);
    }

    private void OnSeVolumeChanged(float volume)
    {
        SoundManager.Instance.SetSEVolume(volume);
    }
}
