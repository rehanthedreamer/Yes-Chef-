using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CustomerWindow : MonoBehaviour
{

    public static event Action OnWindowCustomerServed;
    public static event Action<CustomerWindow> OnCustomerWindowEmpty;
    public Transform customerNode;
    public Transform orderServeNode;

    public CustomerPlayer customerPlayer;
    // Start is called before the first frame update

    private void OnEnable() {
    }

    private void OnDisable() {
        
    }

    void Start()
    {
        if(!IsCustomerPresent())
         OnCustomerWindowEmpty?.Invoke(this);
    }
   
   public bool IsCustomerPresent() {
    return customerPlayer;
   }
}
