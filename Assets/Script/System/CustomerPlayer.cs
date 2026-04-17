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
    private void OnEnable() {
        
    }

    private void OnDisable() {
        
    }
    // Start is called before the first frame update
    void Start()
    {
        // OnCustomerOrderGenerate?.Invoke();
        HideGUIOgers();
        InitOrder();
       
    }

    void InitOrder()
    {
         int randomeIngredientCount = UnityEngine.Random.Range(1, 4);
        for (int i = 0; i < randomeIngredientCount; i++) {
            IngredientType randomIngredient = 
            (IngredientType)UnityEngine.Random.Range(0, System.Enum.GetValues(typeof(IngredientType)).Length);
            orderIngredients.Add(randomIngredient);
            orderIngredientTexts[i].text = randomIngredient.ToString();
            orderIngredientTexts[i].gameObject.SetActive(true);
        }
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

    // Update is called once per frame
    void Update()
    {
        
    }
}
