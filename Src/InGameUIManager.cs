using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class InGameUIManager : MonoBehaviour
{
    public static InGameUIManager Instance { get; private set; }

    [Header("Stage Data")]
    [SerializeField] private int nowRemainChance = 6;
    [SerializeField] private int maxChangeChance = 6;
    [SerializeField] private TMP_Text changeText;

    [Header("UI Panels")]
    public GameObject inGamePanel;
    public GameObject pausePanel;
    public GameObject settingsPanel;

    [Header("Volume Sliders")]
    public Slider bgmSlider;
    public Slider seSlider;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        inGamePanel.SetActive(true);
        pausePanel.SetActive(false);
        settingsPanel.SetActive(false);

        nowRemainChance = Mathf.Clamp(nowRemainChance, 0, maxChangeChance);
        UpdateChangeText();

        InitVolumeSliders();
    }

    public bool CanScaleChange()
    {
        return nowRemainChance > 0;
    }

    public bool UseScaleChangeChance()
    {
        if (nowRemainChance <= 0)
        {
            return false;
        }

        nowRemainChance--;
        UpdateChangeText();

        return true;
    }

    private void UpdateChangeText()
    {
        if (changeText == null) return;

        changeText.text = nowRemainChance + "/" + maxChangeChance;
    }

    private void InitVolumeSliders()
    {
        if (SoundManager.Instance == null) return;

        if (bgmSlider != null)
        {
            bgmSlider.minValue = 0f;
            bgmSlider.maxValue = 1f;
            bgmSlider.wholeNumbers = false;
            bgmSlider.SetValueWithoutNotify(SoundManager.Instance.BGMVolume);
            bgmSlider.onValueChanged.AddListener(OnBgmVolumeChanged);
        }

        if (seSlider != null)
        {
            seSlider.minValue = 0f;
            seSlider.maxValue = 1f;
            seSlider.wholeNumbers = false;
            seSlider.SetValueWithoutNotify(SoundManager.Instance.SEVolume);
            seSlider.onValueChanged.AddListener(OnSeVolumeChanged);
        }
    }

    public void OnPauseButtonClicked()
    {
        Time.timeScale = 0.0f;
        inGamePanel.SetActive(false);
        pausePanel.SetActive(true);
        settingsPanel.SetActive(false);
    }

    public void OnBackButtonInPauseClicked()
    {
        Time.timeScale = 1.0f;
        inGamePanel.SetActive(true);
        pausePanel.SetActive(false);
        settingsPanel.SetActive(false);
    }

    public void OnBackButtonInSettingClicked()
    {
        inGamePanel.SetActive(false);
        pausePanel.SetActive(true);
        settingsPanel.SetActive(false);
    }

    public void OnSettingButtonInPauseClicked()
    {
        inGamePanel.SetActive(false);
        pausePanel.SetActive(false);
        settingsPanel.SetActive(true);
    }

    public void OnHomeButtonInPauseClicked()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene("MainMenu");
    }

    private void OnBgmVolumeChanged(float volume)
    {
        if (SoundManager.Instance == null) return;

        SoundManager.Instance.SetBGMVolume(volume);
    }

    private void OnSeVolumeChanged(float volume)
    {
        if (SoundManager.Instance == null) return;

        SoundManager.Instance.SetSEVolume(volume);
    }
}