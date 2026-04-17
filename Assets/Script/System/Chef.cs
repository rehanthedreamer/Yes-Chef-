using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chef : Singleton<Chef>
{

    public float speed = 2f;
    [SerializeField] private Animator animator;
    [SerializeField] private Fridge fridge;
     [SerializeField] private Stove stove;
      [SerializeField] private Table table;
      public Transform platePoint;
    CustomerWindow curruntCustomerWindowServing;
    List<Ingredient> ingredientsCount = new List<Ingredient>();

    public static event System.Action OnCustermberServed;
    float pickUpTime = 1f;
    public Transform processTimeCanvas;
    // Start is called before the first frame update

    private void OnEnable() {
        
        KitchenWindow.ServeThisCustomerWindow += ServeCustomerWindow;
    }

    private void OnDisable() {
        KitchenWindow.ServeThisCustomerWindow -= ServeCustomerWindow;
    }
    void Start()
    {
       
    }
    void ServeCustomerWindow( CustomerWindow customerWindow) {
        curruntCustomerWindowServing = customerWindow;
        // Process the order and serve the customer
        StartCoroutine(ProcessOrder());
    }

    IEnumerator ProcessOrder()
    {
       table.FreePlate();
        foreach (var ingredient in curruntCustomerWindowServing.customerPlayer.GetOrderIngredients()) {
            Debug.Log("Pick up ingredient from fridge: " + ingredient);
           
            yield return StartCoroutine(MoveToNode(fridge.chefNode));
            yield return StartCoroutine(ShowProcessTime.Instance.StartFill(pickUpTime, processTimeCanvas));
        
            // chek if ingredient needs to be cooked or chopped and move to stove or table accordingly
             if (ingredient == IngredientType.Meat) {
                Debug.Log("Cook ingredient on stove: " + ingredient);
                yield return StartCoroutine(MoveToNode(stove.chefNode));
                // Get the ingredient object on the stove and wait for the cook time
                var burner = stove.GetAvailableBurner();
                var ingredientObj = OrderManager.Instance.GetOrderIngredientType(ingredient, burner.UseBurner().position);
                ingredientObj.SetParentAndPosiion(burner.UseBurner(), burner.UseBurner().position);
                ingredientsCount.Add(ingredientObj);
                // Wait for the cooking time to finish
                yield return StartCoroutine(ShowProcessTime.Instance.StartFill(ingredientObj.GetIngredientProcessTime(), ingredientObj.transform));

                ingredientObj.SetParentAndPosiion(table.GetAvailablePlate(), table.GetAvailablePlate().position);
                burner.FreeBurner();
            } else if (ingredient == IngredientType.Vegitable) {
                Debug.Log("Chop ingredient on table: " + ingredient);
                yield return StartCoroutine(MoveToNode(table.chefNode));
                var ingredientObj = OrderManager.Instance.GetOrderIngredientType(ingredient, table.GetAvailablePlate().position);
                    ingredientObj.SetParentAndPosiion(table.GetAvailablePlate(), table.GetAvailablePlate().position);
                ingredientsCount.Add(ingredientObj);
                yield return StartCoroutine( ShowProcessTime.Instance.StartFill(ingredientObj.GetIngredientProcessTime(), ingredientObj.transform)); 
            } else {
                yield return StartCoroutine(MoveToNode(table.chefNode));
                var ingredientObj = OrderManager.Instance.GetOrderIngredientType(ingredient, table.GetAvailablePlate().position);
                ingredientObj.SetParentAndPosiion(table.GetAvailablePlate(), table.GetAvailablePlate().position);
                ingredientsCount.Add(ingredientObj);
                 ShowProcessTime.Instance.StartFill(ingredientObj.GetIngredientProcessTime(), ingredientObj.transform);
                yield return StartCoroutine(ShowProcessTime.Instance.StartFill(ingredientObj.GetIngredientProcessTime(), ingredientObj.transform));
            }
        }

        // After processing all ingredients, serve the customer
        Debug.Log("Serve customer at window: " + curruntCustomerWindowServing.name);
        CarryPlateToCustomer();
        yield return StartCoroutine(MoveToNode(curruntCustomerWindowServing.orderServeNode));
      
           yield return StartCoroutine(ShowProcessTime.Instance.StartFill(pickUpTime, processTimeCanvas));
         curruntCustomerWindowServing.CustomerServed();
         ScoreManager.Instance.AddScore(CalculateScore());
         curruntCustomerWindowServing = null;
         table.FreePlate();
         RestoreIngredient();
         OnCustermberServed?.Invoke();
    }

    void RestoreIngredient() {
       foreach (var item in ingredientsCount)
       {
         OrderManager.Instance.ReturnObject(item.gameObject);
       }
         ingredientsCount.Clear();
    }

    int CalculateScore() {

        int score = 0;
       foreach (var item in ingredientsCount)
       {
         item.GetSoreVale();
       }
        return score;
    }

     void MoveToStove() {
        StartCoroutine(MoveToNode(stove.chefNode));
    }

      void MoveToTable() {
        StartCoroutine(MoveToNode(table.chefNode));
    }

    private IEnumerator MoveToNode(List<Transform> target)
    {
        animator.SetBool("IsWalking", true);
        foreach (var nodeTarget in target)
        {
             while (Vector3.Distance(transform.position, nodeTarget.position) > 0.05f)
        {
            // Direction to target
            Vector3 direction = (nodeTarget.position - transform.position).normalized;

            // Rotate towards target
            if (direction != Vector3.zero)
            {
                Quaternion lookRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(
                    transform.rotation,
                    lookRotation,
                    10f * Time.deltaTime // rotation speed
                );
            }

            // Move forward
            transform.position = Vector3.MoveTowards(
                transform.position,
                nodeTarget.position,
                speed * Time.deltaTime
            );

            yield return null;
        }

    
        }
        transform.position = target[target.Count - 1].position;
        animator.SetBool("IsWalking", false);
       
    }

    void CarryPlateToCustomer() {
        table.GetAvailablePlate().SetParent(platePoint);
        table.CarryPlate(platePoint);
    }
    
    public CustomerWindow GetCurrentCustomerWindow() {
        return curruntCustomerWindowServing;
    }
}
