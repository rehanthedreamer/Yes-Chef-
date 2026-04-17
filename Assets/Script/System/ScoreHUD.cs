using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreHUD : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    // Start is called before the first frame update
   private void OnEnable() {
    ScoreManager.OnScoreUpdate += UpdateScore;
   }

   private void OnDisable() {
    ScoreManager.OnScoreUpdate -= UpdateScore;
   }

    // Update is called once per frame
    void UpdateScore(int score)
    {
       
        scoreText.text = "Score: " + score.ToString();
    }
    
}
