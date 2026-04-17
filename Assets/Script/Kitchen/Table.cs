using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Table : MonoBehaviour
{
    public List<Transform> chefNode;
    public Transform plate;
    [SerializeField] private Transform platePosOnTable;
    // Start is called before the first frame update
    void Start()
    {
        FreePlate();
    }
   public Transform GetAvailablePlate() {
        plate.gameObject.SetActive(true);
        plate.position = platePosOnTable.position;
        return plate; 
    }

    public void CarryPlate(Transform parent) {
        
        plate.transform.localPosition = Vector3.zero;
    }

    public void FreePlate() {
        plate.gameObject.SetActive(false);
        plate.SetParent(transform);
        plate.position = platePosOnTable.position;
    }
}
