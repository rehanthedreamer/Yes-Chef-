using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "CustomerData", menuName = "YesChef!/CustomerData")]
public class CustomerData : ScriptableObject 
{
    public List<Customer> customers;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

[System.Serializable]
public class Customer
{
    public GameObject prefab;
}
