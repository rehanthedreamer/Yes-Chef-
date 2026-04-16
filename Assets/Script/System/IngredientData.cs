using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;


[CreateAssetMenu(fileName = "IngredientData", menuName = "YesChef!/IngredientData")]
public class IngredientData : ScriptableObject 
{
    public List<IngredientSData> ingredients;
    // Start is called before the first frame update

}

[System.Serializable]
public class IngredientSData
{
    public IngredientDetail ingredientDetail;
   public GameObject prefab;
}

[System.Serializable]
public class IngredientDetail
{
    public IngredientType type;
    public int scoreValue;
    public bool needChopping;
    public float cookTime;
}

public enum IngredientType {
    Meat,
    Cheese,
    Vegitable
}
