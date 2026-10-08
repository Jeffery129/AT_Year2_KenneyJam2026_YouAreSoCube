using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResetManager : MonoBehaviour
{
    [SerializeField] private FadeManager fadeManager;

    //private static bool _wasResetting; // �ق��̃V�[�����炭��Ƃ�fade in���Ȃ��悤�ɂ���
    private bool _isResetting;

    /*
    private void Start()
    {
        if (fadeManager == null)
        {
            return;
        }

        if (_wasResetting) // �ق��̃V�[�����炭��Ƃ�fade in���Ȃ��悤�ɂ���
        {
            fadeManager.SetFadeAlpha(1f);

            _wasResetting = false;

            StartCoroutine(fadeManager.FadeIn());
        }
        else
        {
            fadeManager.SetFadeAlpha(1f);
        }
    }
    */

    public void ResetCurrentScene()
    {
        if (_isResetting) return;

        StartCoroutine(ResetSceneCoroutine());
    }

    private IEnumerator ResetSceneCoroutine()
    {
        _isResetting = true;

        if (fadeManager != null)
        {
            yield return StartCoroutine(fadeManager.FadeOut());
        }

        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void PlaySeVer2(AudioClip seClip)
    {
        SoundManager.Instance.PlaySE(seClip);
    }

    /*

    private IEnumerator FadeOut()
    {
        if (!HasFadeCanvasGroup()) yield break;

        SetFadeBlocksRaycasts(true);

        while (GetMaxFadeAlpha() < 1f)
        {
            AddFadeAlpha(Time.unscaledDeltaTime * fadeSpeed);
            yield return null;
        }

        SetFadeAlpha(1f);
    }

    private IEnumerator FadeIn()
    {
        if (!HasFadeCanvasGroup()) yield break;

        SetFadeBlocksRaycasts(true);

        while (GetMaxFadeAlpha() > 0f)
        {
            AddFadeAlpha(-Time.unscaledDeltaTime * fadeSpeed);
            yield return null;
        }

        SetFadeAlpha(0f);
        SetFadeBlocksRaycasts(false);
    }

    private bool HasFadeCanvasGroup()
    {
        return fadeCanvasGroup != null;
    }

    private void SetFadeAlpha(float alpha)
    {
        if (fadeCanvasGroup != null)
        {
            fadeCanvasGroup.alpha = alpha;
        }
    }

    private void AddFadeAlpha(float value)
    {
        if (fadeCanvasGroup != null)
        {
            fadeCanvasGroup.alpha = Mathf.Clamp01(fadeCanvasGroup.alpha + value);
        }
    }

    private float GetMaxFadeAlpha()
    {
        var alpha = 0f;

        if (fadeCanvasGroup != null)
        {
            alpha = Mathf.Max(alpha, fadeCanvasGroup.alpha);
        }

        return alpha;
    }

    private void SetFadeBlocksRaycasts(bool blocksRaycasts)
    {
        if (fadeCanvasGroup != null)
        {
            fadeCanvasGroup.blocksRaycasts = blocksRaycasts;
        }
    }
    */
}