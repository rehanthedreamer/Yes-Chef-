using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.SceneManagement;

public class PauseScreen : MonoBehaviour
{
    public Button resumeBtn;
    public Button quitBtn;
    [SerializeField] private CanvasGroup canvasGroup;


    private void OnEnable() {
        quitBtn.onClick.AddListener(QuitGame);
        resumeBtn.onClick.AddListener(GameResume);
        TimerHUD.OnGammePause += ScreenFadeIn;
       
    }

    private void OnDisable() {
        quitBtn.onClick.RemoveListener(QuitGame);
        resumeBtn.onClick.RemoveListener(GameResume);
        TimerHUD.OnGammePause -= ScreenFadeIn;
        
    }
    // Start is called before the first frame update
   
   void GameResume() {
        Time.timeScale = 1f;
        StartCoroutine(FadeOut());
        GameManager.isGameStarted = true;
    }

    IEnumerator FadeIn() {
         transform.localScale = Vector3.one;
        float duration = 1f;
        float time = 0f;

        while (time < duration) {
            time += Time.deltaTime;
            canvasGroup.alpha = UnityEngine.Mathf.Lerp(0f, 1f, time / duration);
            yield return null;
        }
         Time.timeScale = 0f;
        transform.localScale = Vector3.one;
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
         transform.localScale = Vector3.zero;
    }

    void QuitGame() {
       SceneManager.LoadScene("Gameplay");
    }
}
