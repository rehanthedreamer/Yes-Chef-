using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System;
using System.Collections;

public class StartScreen : MonoBehaviour
{

    public static event Action OnStartGame;
    public Button startButton;
    [SerializeField] private CanvasGroup canvasGroup;


    private void OnEnable() {
        startButton.onClick.AddListener(StartGame);
        EndScreen.OnRestartGame += ScreenFadeOut;
    }

    private void OnDisable() {
        startButton.onClick.RemoveListener(StartGame);
        EndScreen.OnRestartGame -= ScreenFadeOut;
    }
    // Start is called before the first frame update
   
   void StartGame() {
        StartCoroutine(FadeOut());
        OnStartGame?.Invoke();
        // Invoke("LoadGame", 1f);
    }

    void ScreenFadeOut() {
        StartCoroutine(FadeOut());
    }
    void ScreenFadeIn() {
        StartCoroutine(FadeIn());
    }
    IEnumerator FadeIn() {
        float duration = 1f;
        float time = 0f;

        while (time < duration) {
            time += Time.deltaTime;
            canvasGroup.alpha = UnityEngine.Mathf.Lerp(0f, 1f, time / duration);
            yield return null;
        }
         transform.localScale = Vector3.one;
    }

    IEnumerator FadeOut() {
        float duration = 1f;
        float time = 0f;

        while (time < duration) {
            time += Time.deltaTime;
            canvasGroup.alpha = UnityEngine.Mathf.Lerp(1f, 0f, time / duration);
            yield return null;
        }
         transform.localScale = Vector3.zero;
    }
}
