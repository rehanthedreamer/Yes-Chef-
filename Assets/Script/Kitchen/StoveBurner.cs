using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveBurner : MonoBehaviour
{
    // Start is called before the first frame update
    bool isBussy = false;
    [SerializeField] Transform utinsils;


    void OnEnable()
    {
        GameTimer.OnTimerComplete += ResetData;
    }
    void OnDestroy()
    {
        GameTimer.OnTimerComplete -= ResetData;
    }

    void Start()
    {
        FreeBurner();
    }

    public bool IsBussy() {
        return isBussy;
    }

    public Transform UseBurner() {
        isBussy = true;
        utinsils.gameObject.SetActive(true);
        return utinsils;
    }

    public void FreeBurner() {
        isBussy = false;
        utinsils.gameObject.SetActive(false);
    }

    void ResetData() {
        isBussy = false;
        utinsils.gameObject.SetActive(false);
    }
}

