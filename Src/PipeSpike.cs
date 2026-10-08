using UnityEngine;
using System.Collections;

public class PipeSpike : MonoBehaviour, IOperatable
{
    public bool isOperating = true;

    [Header("ìÆÇ©Ç∑ûô")]
    [SerializeField] private Transform spike;

    [Header("ûôÇÃà íu")]
    [SerializeField] private Vector3 hiddenLocalPos;
    [SerializeField] private Vector3 exposedLocalPos;

    [Header("éûä‘ê›íË")]
    [SerializeField] private float interval = 3;
    [SerializeField] private float moveDuration = 1;

    private Coroutine spikeCoroutine;

    private void Start()
    {
        spike.localPosition = hiddenLocalPos;

        if (isOperating)
        {
            spikeCoroutine = StartCoroutine(SpikeRoutine());
        }
    }

    private IEnumerator SpikeRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(interval);

            yield return MoveSpike(
                    hiddenLocalPos,
                    exposedLocalPos
                );

            yield return new WaitForSeconds(moveDuration);

            yield return MoveSpike(
                exposedLocalPos,
                hiddenLocalPos
                );
        }
    }

    private IEnumerator MoveSpike(Vector3 start, Vector3 end)
    {
        float elapsedTime = 0.0f;

        while (elapsedTime < moveDuration)
        {
            elapsedTime += Time.deltaTime;

            float t = Mathf.Clamp01(elapsedTime / moveDuration);
            spike.localPosition = Vector3.Lerp(start, end, t);

            yield return null;
        }

        spike.localPosition = end;
    }


    public void SetOperating(bool state)
    {
        if (isOperating == state) return;

        isOperating = state;

        if (isOperating)
        {
            if (spikeCoroutine != null) StopCoroutine(spikeCoroutine);
            spikeCoroutine = StartCoroutine(SpikeRoutine());
        }
        else
        {
            if (spikeCoroutine != null) StopCoroutine(spikeCoroutine);
            spikeCoroutine = StartCoroutine(MoveSpike(spike.localPosition, hiddenLocalPos));
        }
    }

    public void ToggleOperating()
    {
        SetOperating(!isOperating);
    }
}
