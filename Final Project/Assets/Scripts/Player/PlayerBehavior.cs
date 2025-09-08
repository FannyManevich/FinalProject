using UnityEngine;
using Assets.Scripts.Managers;

public class PlayerBehavior : MonoBehaviour
{
    [Header("Player Data & speed:")]
    [SerializeField] private PlayerSO playerData;

    [Header("Beacon:")]
    [SerializeField] BeaconSO beacon;

    [Header("Plant Prefab & Holding Position:")]
    [SerializeField] private GameObject plantHoldPos;
    [SerializeField] private GameObject plantPrefab;

    [Header("Tags:")]
    [SerializeField] private string cashRegisterTag = "Register";
    [SerializeField] private string restockTag = "Restock";
    [SerializeField] private string plantTag = "Plant";

    public PlayerState CurrentState { get; private set; }
    public PlantSO heldPlantData;
    private GameObject plantYouAreOn;
    private GameObject holdingPlant;

    private void Awake()
    {
        if (playerData != null)
        {
            playerData.plantPicked = null;
        }
        ChangeState(PlayerState.Moving);
    }
    private void OnEnable()
    {
        if (beacon.inputChannel != null) beacon.inputChannel.OnInteractEvent += Interact;
    }
    private void OnDisable()
    {
        if (beacon.inputChannel != null) beacon.inputChannel.OnInteractEvent -= Interact;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(plantTag))
        {
            plantYouAreOn = other.gameObject;
        }
        else if (other.CompareTag(cashRegisterTag))
        {
            ChangeState(PlayerState.InRegister);
        }
        else if (other.CompareTag(restockTag))
        {
            ChangeState(PlayerState.InRestock);
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject == plantYouAreOn)
        {
            plantYouAreOn = null;
        }
        else if (other.CompareTag(cashRegisterTag))
        {
            if (CurrentState == PlayerState.InRegister)
            {
                if (holdingPlant != null)
                {
                    ChangeState(PlayerState.HoldPlant);
                }
                else
                {
                    ChangeState(PlayerState.Moving);
                }
            }
        }
        else if (other.CompareTag(restockTag))
        {
            if (CurrentState == PlayerState.InRestock)
            {
                if (holdingPlant != null)
                {
                    ChangeState(PlayerState.HoldPlant);
                }
                else
                {
                    ChangeState(PlayerState.Moving);
                }
            }
        }
    }
    public void Interact()
    {
        if (CurrentState == PlayerState.HoldPlant)
        {
            DropPlant();
            return;
        }
        
        switch (CurrentState)
        {
            case PlayerState.Moving:
                TryPickUpPlant();
                break;
            case PlayerState.InRegister:
                CashRegister.Instance.PlayerInteraction();
                break;
            case PlayerState.InRestock:
                InventoryManager.Instance.Restock(this);
                break;
        }
    }
    private void ChangeState(PlayerState newState)
    {
        if (CurrentState == newState) return;

        CurrentState = newState;
    }
    private void TryPickUpPlant()
    {
        if (plantYouAreOn != null && holdingPlant == null)
        {
            holdingPlant = plantYouAreOn;
            heldPlantData = plantYouAreOn.GetComponent<Plant>().CurrentPlantType;
            holdingPlant.transform.SetParent(plantHoldPos.transform);
            holdingPlant.transform.localPosition = Vector3.zero;
            holdingPlant.GetComponent<Collider2D>().enabled = false;

            plantYouAreOn = null;         
            ChangeState(PlayerState.HoldPlant);
        }
    }
    private void DropPlant()
    {
        if (holdingPlant != null)
        {
            holdingPlant.transform.SetParent(null); 
            holdingPlant.transform.position = this.transform.position;

            holdingPlant.GetComponent<Collider2D>().enabled = true;

            holdingPlant = null;
            heldPlantData = null;
            ChangeState(PlayerState.Moving);
        }
    }

    public void ChoseRightPlantForCustomer()
    {
        Destroy(holdingPlant);
        holdingPlant = null;
        heldPlantData = null;
    }
    public void TakeFromStock(PlantSO plantData)
    {
        if (playerData.plantPicked != null)
        {
            Debug.Log("Your already holding a plant.");
            return;
        }

        if (plantData != null)
        {
            holdingPlant = Instantiate(plantPrefab, plantHoldPos.transform.position, Quaternion.identity);
            holdingPlant.GetComponent<Plant>().Initialize(plantData);

            holdingPlant.transform.SetParent(plantHoldPos.transform);
            holdingPlant.transform.localPosition = Vector3.zero;
            holdingPlant.GetComponent<Collider2D>().enabled = false;
        }           
        ChangeState(PlayerState.HoldPlant);
    }
}