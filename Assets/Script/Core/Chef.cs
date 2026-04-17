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
    Coroutine processOrderCoroutine;
    // Start is called before the first frame update

    private void OnEnable() {
        
        KitchenWindow.ServeThisCustomerWindow += ServeCustomerWindow;
        GameTimer.OnTimerComplete += ResetData;
        EndScreen.OnRestartGame += ServeNewCustomer;
    }

    private void OnDisable() {
        KitchenWindow.ServeThisCustomerWindow -= ServeCustomerWindow;
        GameTimer.OnTimerComplete -= ResetData;
        EndScreen.OnRestartGame -= ServeNewCustomer;
    }
    void Start()
    {
       
    }
    void ServeCustomerWindow( CustomerWindow customerWindow) {
        curruntCustomerWindowServing = customerWindow;
        // Process the order and serve the customer
        StopProcessOrderCoroutine();
        processOrderCoroutine =  StartCoroutine(ProcessOrder());
    }

    IEnumerator ProcessOrder()
    {
       table.FreePlate();
       yield return new WaitUntil(() => GameManager.isGameStarted);
        foreach (var ingredient in curruntCustomerWindowServing.customerPlayer.GetOrderIngredients()) {
            Debug.Log("Pick` up ingredient from fridge: " + ingredient);
           /// Move to fridge and pick up the ingredient
            yield return StartCoroutine(MoveToNode(fridge.chefNode));
            /// Pick up the ingredient and wait for the pick up time
            yield return StartCoroutine(ShowProcessTime.Instance.StartFill(pickUpTime, processTimeCanvas));
        
            // chek if ingredient needs to be cooked or chopped and move to stove or table accordingly
             if (ingredient == IngredientType.Meat) {
                Debug.Log("Cook ingredient on stove: " + ingredient);
                var ingredientObj = OrderManager.Instance.GetOrderIngredientType(ingredient);
                ingredientObj.PlaceOnChefHand(platePoint);
                ingredientsCount.Add(ingredientObj);

                yield return StartCoroutine(MoveToNode(stove.chefNode));
                // Get the ingredient object on the stove and wait for the cook time
                var burner = stove.GetAvailableBurner();
                ingredientObj.SetParentAndPosiion(burner.UseBurner(), burner.UseBurner().position);
                // Wait for the cooking time to finish
                yield return StartCoroutine(ShowProcessTime.Instance.StartFill(ingredientObj.GetIngredientProcessTime(), ingredientObj.transform));
                /// After cooking is done, move the ingredient to the table
                ingredientObj.SetParentAndPosiion(table.GetAvailablePlate(), table.GetAvailablePlate().position);
                burner.FreeBurner();
            } else if (ingredient == IngredientType.Vegitable) {
                Debug.Log("Chop ingredient on table: " + ingredient);
                /// Move to table and chop the ingredient
                 var ingredientObj = OrderManager.Instance.GetOrderIngredientType(ingredient);
                  ingredientObj.PlaceOnChefHand(platePoint);
                ingredientsCount.Add(ingredientObj);
                /// Wait for the chopping time to finish
                yield return StartCoroutine(MoveToNode(table.chefNode));
                ingredientObj.SetParentAndPosiion(table.GetAvailablePlate(), table.GetAvailablePlate().position);
                /// Wait for the chopping time to finish
                yield return StartCoroutine( ShowProcessTime.Instance.StartFill(ingredientObj.GetIngredientProcessTime(), ingredientObj.transform)); 
            } else {
                 var ingredientObj = OrderManager.Instance.GetOrderIngredientType(ingredient);
                   ingredientObj.PlaceOnChefHand(platePoint);
                ingredientsCount.Add(ingredientObj);
                yield return StartCoroutine(MoveToNode(table.chefNode));
                ingredientObj.SetParentAndPosiion(table.GetAvailablePlate(), table.GetAvailablePlate().position);
                 ShowProcessTime.Instance.StartFill(ingredientObj.GetIngredientProcessTime(), ingredientObj.transform);
                yield return StartCoroutine(ShowProcessTime.Instance.StartFill(ingredientObj.GetIngredientProcessTime(), ingredientObj.transform));
            }

             yield return new WaitUntil(() => GameManager.isGameStarted);
        }

        // After processing all ingredients, serve the customer
        Debug.Log("Serve customer at window: " + curruntCustomerWindowServing.name);
        CarryPlateToCustomer();
        yield return StartCoroutine(MoveToNode(curruntCustomerWindowServing.orderServeNode));
        yield return StartCoroutine(ShowProcessTime.Instance.StartFill(pickUpTime, processTimeCanvas));
         curruntCustomerWindowServing.CustomerServed(CalculateScore());
         yield return new WaitForSeconds(1f);
        ServeNewCustomer();
    }

    void ServeNewCustomer() {
        curruntCustomerWindowServing = null;
         table.FreePlate();
         RestoreIngredient();
         Debug.Log("Invoking OnCustomerServed event");
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
        score += item.GetSoreVale();
       }
        return score + (-(int)curruntCustomerWindowServing.customerPlayer.GetCurrentOrderTime());
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

    void ResetData() {
        curruntCustomerWindowServing = null;
        table.FreePlate();
        RestoreIngredient();
        StopProcessOrderCoroutine();
    }

    void StopProcessOrderCoroutine() {
         if(processOrderCoroutine != null) {
                    StopCoroutine(processOrderCoroutine);
                    processOrderCoroutine = null;
                }
    }
}
