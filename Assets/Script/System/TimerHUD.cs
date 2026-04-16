using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Security.Cryptography;

public class TimerHUD : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerText;
    // Start is called before the first frame update
   private void OnEnable() {
    GameTimer.OnTimerUpdate += UpdateTimer;
   }

   private void OnDisable() {
    GameTimer.OnTimerUpdate -= UpdateTimer;
   }

    // Update is called once per frame
    void UpdateTimer(float time)
    {
        timerText.text =  Utills.FormatTime(time);
    }
}
