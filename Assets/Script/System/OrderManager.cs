using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderManager : MonoBehaviour
{
    public IngredientData ingredientData;
    public int poolSize = 10;

    private List<GameObject> pool;

    void Awake()
    {
       PoolInit();
    }






/// <summary>
/// Pool Systerm for Ingredients. Pre-instantiates a set number of ingredient objects and manages their active state to optimize performance.
/// </summary>
    void PoolInit()
    {
         pool = new List<GameObject>();

        for (int j = 0; j < ingredientData.ingredients.Count; j++)
        {
              for (int i = 0; i < poolSize; i++)
            {
                GameObject obj = Instantiate(ingredientData.ingredients[j].prefab, transform);
                obj.SetActive(false);
                pool.Add(obj);
            }
        }
      
    }

    public GameObject GetCustomer(Vector3 position, Quaternion rotation)
    {
        foreach (var obj in pool)
        {
            if (!obj.activeInHierarchy)
            {
                obj.transform.position = position;
                obj.transform.rotation = rotation;
                obj.SetActive(true);
                return obj;
            }
        }

        // If all are active → create new one (optional)
        GameObject newObj = Instantiate(ingredientData.ingredients[Random.Range(0, ingredientData.ingredients.Count)].prefab, position, rotation, transform);
        pool.Add(newObj);
        return newObj;
    }

    public void ReturnObject(GameObject obj)
    {
        obj.SetActive(false);
    }
}
