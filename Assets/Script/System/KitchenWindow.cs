using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class KitchenWindow : MonoBehaviour
{

    public static event System.Action<CustomerWindow> ServeThisCustomerWindow;
    public List<CustomerWindow> customerWindows = new List<CustomerWindow>();
    // Start is called before the first frame update

    private void OnEnable() {
        Chef.OnCustermberServed += ServeAnyCustomerWindow;
    }

    private void OnDisable() {
        Chef.OnCustermberServed -= ServeAnyCustomerWindow;
    }

    void Start()
    {
        StartCoroutine(ServeAnyCustomerWindowWithDelay());
    }

    IEnumerator ServeAnyCustomerWindowWithDelay() {
        yield return new WaitUntil(() => GameManager.isGameStarted);
        yield return new WaitForSeconds(4f);
        var customerWindowToServe = customerWindows.Find(window => window.IsCustomerPresent());
        if(customerWindowToServe != null) {
            ServeThisCustomerWindow?.Invoke(customerWindowToServe);
        }
    }

    void ServeAnyCustomerWindow() {
        var customerWindowToServe = customerWindows.Find(window => window.IsCustomerPresent());
        if(customerWindowToServe != null) {
            ServeThisCustomerWindow?.Invoke(customerWindowToServe);
        }
    }
 
}
