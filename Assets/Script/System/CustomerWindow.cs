using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class CustomerWindow : MonoBehaviour
{

    public static event Action OnWindowCustomerServed;
    public static event Action<CustomerWindow> OnCustomerWindowEmpty;
    public Transform customerNode;
    public List<Transform> orderServeNode;

    public CustomerPlayer customerPlayer;
    // Start is called before the first frame update

    private void OnEnable() {
    }

    private void OnDisable() {
        
    }

    void Start()
    {
        StartCoroutine(SpawnCustomer());
    }
    IEnumerator SpawnCustomer() {
        yield return new WaitUntil(() => !IsCustomerPresent() && GameManager.isGameStarted);
        yield return new WaitForSeconds(2f);
        OnCustomerWindowEmpty?.Invoke(this);
    }

    public void CustomerServed(int score) {
        if (customerPlayer) {
            customerPlayer.StopOrderTimer();
            customerPlayer.ShowScoreFloater(score);
            ScoreManager.Instance.AddScore(score);
            StartCoroutine(NewCustomerDelay());
        }
    }
    
    IEnumerator NewCustomerDelay() {
        yield return new WaitForSeconds(1.5f);
         CustomerManager.Instance.ReturnObject(customerPlayer.gameObject);
        customerPlayer = null;
        yield return new WaitForSeconds(3.5f);
        OnCustomerWindowEmpty?.Invoke(this);
    }
   
   public bool IsCustomerPresent() {
    return customerPlayer != null;
   }
}
