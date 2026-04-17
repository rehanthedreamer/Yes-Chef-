using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEngine;

public class KitchenWindow : MonoBehaviour
{

    public static event System.Action<CustomerWindow> ServeThisCustomerWindow;
    public List<CustomerWindow> customerWindows = new List<CustomerWindow>();
    // Start is called before the first frame update

    private void OnEnable() {
        Chef.OnCustermberServed += ServeAnyCustomerWindow;
         StartScreen.OnStartGame += StartWithDelay;
         EndScreen.OnRestartGame += StartWithDelay;
    }

    private void OnDisable() {
        Chef.OnCustermberServed -= ServeAnyCustomerWindow;
        StartScreen.OnStartGame -= StartWithDelay;
        EndScreen.OnRestartGame -= StartWithDelay;
       
    }

    void StartWithDelay()
    {
        StartCoroutine(ServeAnyCustomerWindowWithDelay());
    }

    IEnumerator ServeAnyCustomerWindowWithDelay() {
        yield return new WaitUntil(() => GameManager.isGameStarted);
        yield return new WaitForSeconds(2f);
        var customerWindowToServe = customerWindows.Find(window => window.IsCustomerPresent());
        if(customerWindowToServe != null) {
            ServeThisCustomerWindow?.Invoke(customerWindowToServe);
        }
    }

    void ServeAnyCustomerWindow() {

        Debug.Log("Chef served a customer, checking for next customer to serve...");
        var customerWindowToServe = customerWindows
            .Where(window => window.IsCustomerPresent())
            .OrderByDescending(window => window.customerPlayer.GetCurrentOrderTime())
            .FirstOrDefault();
        if(customerWindowToServe != null) {
            ServeThisCustomerWindow?.Invoke(customerWindowToServe);
             Debug.Log("Invoked ServeThisCustomerWindow event for customer window: " + customerWindowToServe.name);
        }
    }
 
}
