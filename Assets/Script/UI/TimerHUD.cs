using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Security.Cryptography;
using UnityEngine.UI;

public class TimerHUD : MonoBehaviour
{

    public static event System.Action OnGammePause;
    [SerializeField] Button pauseBtn;
    [SerializeField] private TextMeshProUGUI timerText;
    // Start is called before the first frame update
   private void OnEnable() {
    pauseBtn.onClick.AddListener(PauseGame);
    GameTimer.OnTimerUpdate += UpdateTimer;
   }

   private void OnDisable() {
    pauseBtn.onClick.RemoveListener(PauseGame);
    GameTimer.OnTimerUpdate -= UpdateTimer;
   }

    void PauseGame() {
       
        OnGammePause?.Invoke();
        GameManager.isGameStarted = false;
    }
    // Update is called once per frame
    void UpdateTimer(float time)
    {
        timerText.text =  Utills.FormatTime(time);
    }
}
