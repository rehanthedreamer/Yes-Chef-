using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using TMPro;

public class EndScreen : MonoBehaviour
{
   
    public static event Action OnRestartGame;
    public Button startAgainButton;
    [SerializeField] private TextMeshProUGUI finalScoreText;
    [SerializeField] private TextMeshProUGUI finalTitleText;
    [SerializeField] private CanvasGroup canvasGroup;


    private void OnEnable() {
        startAgainButton.onClick.AddListener(RestartGame);
        StartScreen.OnStartGame += ScreenFadeOut;
        GameTimer.OnTimerComplete += ScreenFadeIn;
       
    }

    private void OnDisable() {
        startAgainButton.onClick.RemoveListener(RestartGame);
        GameTimer.OnTimerComplete -= ScreenFadeIn;
        StartScreen.OnStartGame -= ScreenFadeOut;
    }
    // Start is called before the first frame update
   
   void RestartGame() {
        StartCoroutine(FadeOut());
        OnRestartGame?.Invoke();
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
        transform.localScale = Vector3.one;
    }

    void ScreenFadeIn() {
        UpdateFinalScore();
        StartCoroutine(FadeIn());
    }

    void UpdateFinalScore() {
        finalTitleText.text = ScoreManager.Instance.GetCurruntScore() > ScoreManager.Instance.GetSavedScore() ? "New HighScore: " :  "Score: ";
       if(ScoreManager.Instance.GetCurruntScore() > ScoreManager.Instance.GetSavedScore())
        {
             ScoreManager.Instance.SaveNewHighScore();
             finalScoreText.text = ScoreManager.Instance.GetSavedScore().ToString();
        }
        else
        finalScoreText.text = ScoreManager.Instance.GetCurruntScore().ToString();
       
    }
     void ScreenFadeOut() {
        StartCoroutine(FadeOut());
    }
    
    IEnumerator FadeOut() {
         transform.localScale = Vector3.one;
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
