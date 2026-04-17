using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : Singleton<ScoreManager>
{
    public static event System.Action<int> OnScoreUpdate;
    // Start is called before the first frame update
    private int score = 0;

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

    public int GetFinalScore() {
        return PlayerPrefs.GetInt("FinalScore", 0);
    }

    public void SetFinalScore() {

        if(GetCurruntScore() > GetFinalScore()) PlayerPrefs.SetInt("FinalScore", GetCurruntScore());
       
    }
    
}
