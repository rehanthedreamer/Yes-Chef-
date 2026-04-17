using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderManager : Singleton<OrderManager>
{
    public IngredientData ingredientData;
    public int poolSize = 10;
    private List<Ingredient> pool;


    void Awake()
    {
       PoolInit();
    }






/// <summary>
/// Pool Systerm for Ingredients. Pre-instantiates a set number of ingredient objects and manages their active state to optimize performance.
/// </summary>
    void PoolInit()
    {
         pool = new List<Ingredient>();

        for (int j = 0; j < ingredientData.ingredients.Count; j++)
        {
              for (int i = 0; i < poolSize; i++)
            {
                Ingredient obj = Instantiate(ingredientData.ingredients[j].prefab, transform).GetComponent<Ingredient>();
                obj.gameObject.SetActive(false);
                pool.Add(obj);
            }
        }
      
    }

    public Ingredient GetOrderIngredientType(IngredientType ingredientType, Vector3 position)
    {
     // Ingredient obj =  pool.Find(obj => obj.GetIngredientType() == ingredientType);
       
       foreach (var ingredient in pool)
       {
         if (ingredient.GetIngredientType() == ingredientType && !ingredient.gameObject.activeInHierarchy)
            {
                ingredient.transform.position = position;
                ingredient.gameObject.SetActive(true);
                return ingredient;
            }
       }
           
     

        // If all are active → create new one (optional)
       var thisIngredientData = ingredientData.ingredients.Find(data => data.ingredientDetail.type == ingredientType);
        Ingredient newObj = Instantiate(thisIngredientData.prefab, position, Quaternion.identity, transform).GetComponent<Ingredient>();
        pool.Add(   newObj);
        return newObj;
    }

    public void ReturnObject(GameObject obj)
    {
        obj.SetActive(false);
        obj.transform.SetParent(transform);
    }
}
