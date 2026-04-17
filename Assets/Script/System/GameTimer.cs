
using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System;

public class GameTimer : MonoBehaviour
{
    public static event Action<float> OnTimerUpdate;
    public static event Action OnTimerComplete;
    [Header("Timer Settings")]
    public float duration = 60f;

    private float currentTime;
    private Coroutine timerRoutine;

    private void OnEnable() {
        StartScreen.OnStartGame += StartTimer;
        EndScreen.OnRestartGame += StartTimer;
    }
    private void OnDisable() {
        StartScreen.OnStartGame -= StartTimer;
        EndScreen.OnRestartGame -= StartTimer;
    }

    public void StartTimer()
    {
        StopTimer();
        currentTime = duration;
        timerRoutine = StartCoroutine(RunTimer());
        GameManager.isGameStarted = true;
    }

    public void StopTimer()
    {
        if (timerRoutine != null)
        {
            StopCoroutine(timerRoutine);
            timerRoutine = null;
        }
    }


    private IEnumerator RunTimer()
    {
        while (currentTime > 0f)
        {
            currentTime -= Time.deltaTime;
            OnTimerUpdate?.Invoke(currentTime);
            yield return null; // next frame
        }

        currentTime = 0f;
        OnTimerUpdate?.Invoke(currentTime);
        OnTimerComplete?.Invoke();
        GameManager.isGameStarted = false;
        timerRoutine = null;

    }
    public bool IsRunning()
    {
        return timerRoutine != null;
    }
}
