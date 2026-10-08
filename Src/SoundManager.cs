using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    public float BGMVolume = 1.0f;
    public float SEVolume = 1.0f;

    [SerializeField] private AudioSource _bgmAudioSource;
    [SerializeField] private AudioClip _bgmAudioClip;
    [SerializeField] private AudioSource _seAudioSource;

    [SerializeField] private AudioClip _buttonClickSE;


    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void Start()
    {
        BGMVolume = PlayerPrefs.GetFloat("BGMVolume", 1f);
        SEVolume = PlayerPrefs.GetFloat("SEVolume", 1f);

        if (!_bgmAudioSource) return;
        if (!_seAudioSource) return;
        _bgmAudioSource.volume = BGMVolume;
        _seAudioSource.volume = SEVolume;

        PlayBGM(_bgmAudioClip);
    }

    public void SetBGMVolume(float volume)
    {
        BGMVolume = volume;
        _bgmAudioSource.volume = BGMVolume;

        PlayerPrefs.SetFloat("BGMVolume", BGMVolume);
        PlayerPrefs.Save();
    }

    public void SetSEVolume(float volume)
    {
        SEVolume = volume;
        _seAudioSource.volume = SEVolume;

        PlayerPrefs.SetFloat("SEVolume", SEVolume);
        PlayerPrefs.Save();
    }

    public void PlayBGM(AudioClip bgmClip)
    {
        if(bgmClip == null)
        {
            return;
        }

        _bgmAudioSource.clip = bgmClip;
        _bgmAudioSource.loop = true;
        _bgmAudioSource.Play();
    }

    public void StopBGM()
    {
        _bgmAudioSource.Stop();
    }

    public void PlaySE(AudioClip seClip)
    {
        if(seClip == null)
        {
            return;
        }

        _seAudioSource.PlayOneShot(seClip);
    }

    private void OnSceneLoaded(
        Scene scene,
        LoadSceneMode loadSceneMode)
    {
        ButtonClickSounds();
    }

    private void ButtonClickSounds()
    {
        Button[] buttons = FindObjectsByType<Button>(
            FindObjectsInactive.Include,
            FindObjectsSortMode.None);

        foreach(Button button in buttons)
        {
            button.onClick.RemoveListener(PlayButtonClickSound);
            button.onClick.AddListener(PlayButtonClickSound);
        }
    }

    void PlayButtonClickSound()
    {
        PlaySE(_buttonClickSE);
    }

    private void OnDestroy()
    {
        if(Instance == this)
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }
}
