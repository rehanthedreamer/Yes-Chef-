using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : Singleton<ScoreManager>
{
    public static event System.Action<int> OnScoreUpdate;
    // Start is called before the first frame update
    private int score = 0;

    void OnEnable()
    {
       GameTimer.OnTimerComplete += ResetData;
    }
    void OnDisable()
    {
        GameTimer.OnTimerComplete -= ResetData;
    }

    void Start()
    {
        OnScoreUpdate?.Invoke(score);
    }
    public void AddScore(int amount) {
        score += amount;
        OnScoreUpdate?.Invoke(score);
    }

    public int GetCurruntScore() {
        return score;
    }

    public void SaveNewHighScore() {
        PlayerPrefs.SetInt("FinalScore", GetCurruntScore());
    }

    public int GetSavedScore()
    {
        return PlayerPrefs.GetInt("FinalScore", 0);
    }


    public void ResetData() {
        score = 0;
        OnScoreUpdate?.Invoke(score);
    }
    
}
