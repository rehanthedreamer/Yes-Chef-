using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
public class CustomerPlayer : MonoBehaviour
{


    public static event Action OnCustomerOrderGenerate;

    public List<IngredientType> orderIngredients = new List<IngredientType>();
    [SerializeField] private List<TextMeshProUGUI>  orderIngredientTexts = new List<TextMeshProUGUI>();
    [SerializeField] private TextMeshProUGUI orderTimerText;
    [SerializeField] CanvasGroup scoreFloater;
    [SerializeField] TextMeshProUGUI scoreFloaterText;
    float currentTime;
    Coroutine orderTimerCoroutine;
    private void OnEnable() {
         GameTimer.OnTimerComplete += ResetData;
    }

    private void OnDisable() {
        GameTimer.OnTimerComplete -= ResetData;
    }
    // Start is called before the first frame update
    void Start()
    {
        // OnCustomerOrderGenerate?.Invoke();
       
      //  InitOrder();
       
    }

   public void InitOrder()
    {
         HideGUIOgers();
         int randomeIngredientCount = UnityEngine.Random.Range(2, 4); // Randomly decide if the order will have 2 or 3 ingredients
        for (int i = 0; i < randomeIngredientCount; i++) {
            IngredientType randomIngredient = 
            (IngredientType)UnityEngine.Random.Range(0, System.Enum.GetValues(typeof(IngredientType)).Length);
            orderIngredients.Add(randomIngredient);
            orderIngredientTexts[i].text = randomIngredient.ToString();
            orderIngredientTexts[i].gameObject.SetActive(true);
        }
        StartOrderTimer();
    }

    public List<IngredientType> GetOrderIngredients() {
        return orderIngredients;
    }

    void HideGUIOgers()
    {
        foreach (var text in orderIngredientTexts) {
            text.gameObject.SetActive(false);
        }
    }

    void StartOrderTimer() {
        currentTime = 0;
        if (orderTimerCoroutine != null) {
            StopCoroutine(orderTimerCoroutine);
            orderTimerCoroutine = null;
        }
        orderTimerCoroutine = StartCoroutine(OrderTimer());
    }
 private IEnumerator OrderTimer()
    {
        while (true) { 
            currentTime += Time.deltaTime;
            orderTimerText.text = Utills.FormatTime(currentTime).ToString();
            yield return null; 
        }
    }

    public void StopOrderTimer() {
        if (orderTimerCoroutine != null) {
            StopCoroutine(orderTimerCoroutine);
            orderTimerCoroutine = null;
        }
    }

    public float GetCurrentOrderTime() {
        return currentTime;
    }

    public void ShowScoreFloater(int score) {
        StartCoroutine(ScoreFloaterRoutine(score));
    }
    IEnumerator ScoreFloaterRoutine(int score) {
        scoreFloaterText.text = "+" + score.ToString();
        scoreFloater.alpha = 1f;
        Vector3 originalPosition = scoreFloater.transform.localPosition;
        Vector3 targetPosition = originalPosition + new Vector3(0, 2, 0); // Move up by 50 units
        float duration = 1f; // Duration of the animation
        float time = 0f;

        while (time < duration)
        {
            time += Time.deltaTime;
            float t = time / duration;
            scoreFloater.transform.localPosition = Vector3.Lerp(originalPosition, targetPosition, t);
            scoreFloater.alpha = Mathf.Lerp(1f, 0f, t); // Fade out
            yield return null;
        }

        scoreFloater.transform.localPosition = originalPosition;
        scoreFloater.alpha = 0f;
    }

    public void ResetData() {
        StopOrderTimer();
        orderIngredients.Clear();
        HideGUIOgers();
        scoreFloater.alpha = 0f;
    }
}
