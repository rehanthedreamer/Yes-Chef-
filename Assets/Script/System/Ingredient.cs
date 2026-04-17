using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ingredient : MonoBehaviour
{
    [SerializeField] private IngredientDetail ingredientDetail;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public IngredientType GetIngredientType() {
        return ingredientDetail.type;
    }

    public float GetIngredientProcessTime() {
        return ingredientDetail.cookTime;
    }

        public int GetSoreVale() {
        return ingredientDetail.scoreValue;
    }


    public void SetParentAndPosiion(Transform parent, Vector3 position) {
        transform.SetParent(parent);
        transform.localPosition = position;
        if(ingredientDetail.type == IngredientType.Cheese) {
            transform.localScale = Vector3.one; 
        } else
            transform.localScale = Vector3.one*15; 
    }
}
