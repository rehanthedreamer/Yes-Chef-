using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public class EndScreen : MonoBehaviour
{
   
    public static event Action OnRestartGame;
    public Button startAgainButton;
    [SerializeField] private CanvasGroup canvasGroup;


    private void OnEnable() {
        startAgainButton.onClick.AddListener(StartGame);
        StartScreen.OnStartGame += ScreenFadeOut;
        GameTimer.OnTimerComplete += ScreenFadeIn;
       
    }

    private void OnDisable() {
        startAgainButton.onClick.RemoveListener(StartGame);
        GameTimer.OnTimerComplete -= ScreenFadeIn;
        StartScreen.OnStartGame -= ScreenFadeOut;
    }
    // Start is called before the first frame update
   
   void StartGame() {
        StartCoroutine(FadeOut());
        OnRestartGame?.Invoke();
        // Invoke("LoadGame", 1f);
    }

    IEnumerator FadeIn() {
        float duration = 1f;
        float time = 0f;

        while (time < duration) {
            time += Time.deltaTime;
            canvasGroup.alpha = UnityEngine.Mathf.Lerp(0f, 1f, time / duration);
            yield return null;
        }
    }

    void ScreenFadeIn() {
        StartCoroutine(FadeIn());
    }
     void ScreenFadeOut() {
        StartCoroutine(FadeOut());
    }
    
    IEnumerator FadeOut() {
        float duration = 1f;
        float time = 0f;

        while (time < duration) {
            time += Time.deltaTime;
            canvasGroup.alpha = UnityEngine.Mathf.Lerp(1f, 0f, time / duration);
            yield return null;
        }
    }
}
