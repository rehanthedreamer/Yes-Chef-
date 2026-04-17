using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowProcessTime : Singleton<ShowProcessTime>
{
    [SerializeField] CanvasGroup canvasGroup;
    [SerializeField] private Image fillImage; // Must be Image Type = Filled, Fill Method = Radial360
    private Coroutine runningCoroutine;


    void Start()
    {
        canvasGroup.alpha = 0f; // Hide the UI at the start
    }
    public IEnumerator StartFill(float duration, Transform atPosition)
    {
        ResetFill();
        transform.SetParent(atPosition);
        transform.localPosition = Vector3.zero;
        // Stop previous if running
        if (runningCoroutine != null)
            StopCoroutine(runningCoroutine);
        runningCoroutine = StartCoroutine(FillRoutine(duration));
        yield return runningCoroutine;
    }

    private IEnumerator FillRoutine(float duration)
    {
        canvasGroup.alpha = 1f;
        float time = 0f;
        fillImage.fillAmount = 0f;

        while (time < duration)
        {
            time += Time.deltaTime;
            fillImage.fillAmount = time / duration;

            yield return null;
        }

        fillImage.fillAmount = 1f;
        canvasGroup.alpha = 0f;
        runningCoroutine = null;
    }


    public void ResetFill()
    {
        if (runningCoroutine != null)
            StopCoroutine(runningCoroutine);
        fillImage.fillAmount = 0f;
    }
}
