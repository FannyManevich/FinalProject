using UnityEngine;
using UnityEngine.InputSystem;
using System.Linq;
using Assets.Scripts.Managers;

public class PlayerBehavior : MonoBehaviour
{
    [Header("Player Data & speed:")]
    [SerializeField] private PlayerSO playerData;
    [SerializeField] private float moveSpeed = 5f;

    [Header("Input channel:")]
    [SerializeField] InputChannel inputChannel;

    [Header("Plant holding position:")]
    [SerializeField] private GameObject PlantHoldPos;

    [Header("Tags:")]
    [SerializeField] private string cashRegisterTag = "Register";
    [SerializeField] private string restockTag = "Restock";
    [SerializeField] private string plantTag = "Plant";

    [Header("Restock station:")]
    [SerializeField] private PlantRestock restockStation;

    public PlayerState CurrentState { get; private set; }
    public PlantSO HeldPlantData => playerData != null ? playerData.plantPicked : null;

    private GameObject PlantYouAreOn;
    private GameObject HoldingPlant;
    private Rigidbody2D rb;
    private Vector2 moveInput;
    private void Awake()
    {
        if (playerData != null)
        {
            playerData.plantPicked = null;
        }
        ChangeState(PlayerState.Moving);
    }
    private void FixedUpdate()
    {
        rb.velocity = new Vector2(moveInput.x * moveSpeed, moveInput.y * moveSpeed);
    }
        private void OnEnable()
    {
        if (inputChannel != null) inputChannel.OnInteractEvent += Interact;
    }
    private void OnDisable()
    {
        if (inputChannel != null) inputChannel.OnInteractEvent -= Interact;
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
                ChangeState(PlayerState.Moving);
            }
        }
        else if (other.CompareTag(restockTag))
        {
            if (CurrentState == PlayerState.InRestock)
            {
                ChangeState(PlayerState.Moving);
            }
        }
    }
    public void Interact()
    {
        switch (CurrentState)
        {
            case PlayerState.Moving:
                TryPickUpPlant();
                break;
            case PlayerState.HoldPlant:
                DropPlant();
                break;
            case PlayerState.InRegister:
                CashRegister.Instance.PlayerInteraction();
                break;
            case PlayerState.InRestock:
                if (restockStation != null)
                {
                    restockStation.RequestPlant();
                }
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
        if (PlantYouAreOn != null)
        {
            HoldingPlant = PlantYouAreOn;
            HoldingPlant.GetComponent<PlantHolding>().FollowHolder(PlantHoldPos);

            var plantComponent = HoldingPlant.GetComponent<Plant>();
            if (playerData != null && plantComponent != null)
            {
                playerData.plantPicked = plantComponent.currentPlantType;
            }
            ChangeState(PlayerState.HoldPlant);
        }
    }
    private void DropPlant()
    {
        if (HoldingPlant != null)
        {
            HoldingPlant.GetComponent<PlantHolding>().StopFollowHolder();
            HoldingPlant = null;

            if (playerData != null)
            {
                playerData.plantPicked = null;
            }
            ChangeState(PlayerState.Moving);
        }
    }
}