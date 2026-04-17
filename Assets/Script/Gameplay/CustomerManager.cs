using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerManager : Singleton<CustomerManager>
{
    public CustomerData customerData;
    public int poolSize = 10;
    private List<GameObject> pool;
    

    void Awake()
    {
       PoolInit();
      
    }

    private void OnEnable() {
        CustomerWindow.OnCustomerWindowEmpty += SpawnCustomer;
        GameTimer.OnTimerComplete += ResetData;
    }
    private void OnDisable() {
        CustomerWindow.OnCustomerWindowEmpty -= SpawnCustomer;
        GameTimer.OnTimerComplete -= ResetData;
    }

    
    void SpawnCustomer(CustomerWindow window)
    {
        window.customerPlayer = GetCustomer(window.customerNode.position, window.customerNode.rotation).GetComponent<CustomerPlayer>();
        window.customerPlayer. InitOrder();
    }



/// <summary>
/// Pool Systerm for Customers. Pre-instantiates a set number of customer objects and manages their active state to optimize performance.
/// </summary>
    void PoolInit()
    {
         pool = new List<GameObject>();

        for (int j = 0; j < customerData.customers.Count; j ++)
        {
            for (int i = 0; i < poolSize; i++)
            {
                GameObject obj = Instantiate(customerData.customers[j].prefab, transform);
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
        GameObject newObj = Instantiate(customerData.customers[Random.Range(0, customerData.customers.Count)].prefab, position, rotation, transform);
        pool.Add(newObj);
        return newObj;
    }

    public void ReturnObject(GameObject obj)
    {
        obj.SetActive(false);
    }

    public void ResetData() {
        foreach (var obj in pool) {
            obj.SetActive(false);
        }
    }
}
