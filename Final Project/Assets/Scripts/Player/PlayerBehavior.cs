using UnityEngine;
using Assets.Scripts.Managers;

public class PlayerBehavior : MonoBehaviour
{
    [Header("Player Data & speed:")]
    [SerializeField] private PlayerSO playerData;

    [Header("Beacon:")]
    [SerializeField] BeaconSO beacon;

    [Header("Plant Prefab & Holding Position:")]
    [SerializeField] private GameObject PlantHoldPos;
    [SerializeField] private GameObject plantPrefab;

    [Header("Tags:")]
    [SerializeField] private string cashRegisterTag = "Register";
    [SerializeField] private string restockTag = "Restock";
    [SerializeField] private string plantTag = "Plant";

    public PlayerState CurrentState { get; private set; }
    public PlantSO HeldPlantData;
    private GameObject PlantYouAreOn;
    private GameObject HoldingPlant;

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
            PlantYouAreOn = other.gameObject;
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
        if (other.gameObject == PlantYouAreOn)
        {
            PlantYouAreOn = null;
        }
        else if (other.CompareTag(cashRegisterTag))
        {
            if (CurrentState == PlayerState.InRegister)
            {
                if (HoldingPlant != null)
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
                if (HoldingPlant != null)
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
        if (PlantYouAreOn != null && HoldingPlant == null)
        {
            HoldingPlant = PlantYouAreOn;
            HeldPlantData = PlantYouAreOn.GetComponent<Plant>().currentPlantType;
            HoldingPlant.transform.SetParent(PlantHoldPos.transform);
            HoldingPlant.transform.localPosition = Vector3.zero;
            HoldingPlant.GetComponent<Collider2D>().enabled = false;

            PlantYouAreOn = null;         
            ChangeState(PlayerState.HoldPlant);
        }
    }
    private void DropPlant()
    {
        if (HoldingPlant != null)
        {
            HoldingPlant.transform.SetParent(null); 
            HoldingPlant.transform.position = this.transform.position;

            HoldingPlant.GetComponent<Collider2D>().enabled = true;

            HoldingPlant = null;        
            ChangeState(PlayerState.Moving);
        }
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
            HoldingPlant = Instantiate(plantPrefab, PlantHoldPos.transform.position, Quaternion.identity);
            HoldingPlant.GetComponent<Plant>().Initialize(plantData);

            HoldingPlant.transform.SetParent(PlantHoldPos.transform);
            HoldingPlant.transform.localPosition = Vector3.zero;
            HoldingPlant.GetComponent<Collider2D>().enabled = false;
        }           
        ChangeState(PlayerState.HoldPlant);
    }
}