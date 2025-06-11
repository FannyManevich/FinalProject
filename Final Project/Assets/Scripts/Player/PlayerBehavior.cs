using UnityEngine;
using UnityEngine.InputSystem;
using System.Linq;

public class PlayerBehavior : MonoBehaviour
{
    public PlayerSO playerData;
    public float moveSpeed = 5f;
    private Rigidbody2D rb;
    private Vector2 moveInput;

    [SerializeField] GameObject PlantHoldPos;
    [SerializeField] InputChannel inputChannel;

    public bool onRegister;
    public bool onPlant;
    public bool onRestock;
    public GameObject RestockStation;
    public GameObject PlantYouAreOn;
    public GameObject PlantYouAreHolding;

    void Start()
    {
        inputChannel.OnInteractEvent += Interact;
        rb = GetComponent<Rigidbody2D>();
        onRegister = false;
        onPlant = false;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.name == "CashRegister")
        {
            onRegister = true;
        }
        else if (collider.gameObject.tag == "Restock")
        {
            onRestock = true;
            RestockStation = collider.gameObject;
        }
        else if (collider.gameObject.tag == "Plant")
        {
            onPlant = true;
            PlantYouAreOn = collider.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.name == "CashRegister")
        {
            onRegister = false;
        }
        else if (collider.gameObject.tag == "Restock")
        {
            onRestock = false;
            RestockStation = null;
        }
        else if (collider.gameObject.tag == "Plant")
        {
            onPlant = false;
            PlantYouAreOn = null;
        }
    }

    public void Interact()
    {
        if (onRegister == true)
        {
            NPCEventManager.RegisterRealese();
        }
        else if (PlantYouAreHolding == null)
        {
            if (onPlant == true)
            {
                PlantYouAreHolding = PlantYouAreOn;
                //Fany added
                PlantYouAreHolding.GetComponents<BoxCollider2D>().FirstOrDefault(x => !x.isTrigger).enabled = false;
                //
                PlantYouAreHolding.GetComponent<PlantHolding>().FollowHolder(PlantHoldPos);

                //Fany added
                playerData.plantPicked = PlantYouAreHolding.GetComponent<Plant>().currentPlantType;
                //
            }
            else
            {
                if (onRestock)
                {
                    RestockStation.GetComponent<PlantRestock>().restock();
                }
            }
        }
        else
        {
            PlantYouAreHolding.transform.position = transform.position;
            PlantYouAreHolding.GetComponent<PlantHolding>().StopFollowHolder();
            //Fany added
            PlantYouAreHolding.GetComponents<BoxCollider2D>().FirstOrDefault(x => !x.isTrigger).enabled = true;
            //
            PlantYouAreHolding = null;
        }
    }
}
