using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stove : MonoBehaviour
{
    public List<Transform> chefNode;
    public List<StoveBurner> stoveBurners = new List<StoveBurner>();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public StoveBurner GetAvailableBurner() {
        return stoveBurners.Find(burner => !burner.IsBussy());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
