using System.Collections;
using UnityEngine;
using Assets.Scripts.Managers;
using System;
using System.Collections.Generic;

public class CashRegister : MonoBehaviour
{
    public static CashRegister Instance { get; private set; }
    public static event Action<PlantSO> OnPlantRequestGenerated;
    public static event Action OnInteractionEnded;

    [Header("Component References:")]
    [SerializeField] private DialogueEvents dialogueManager;
    [SerializeField] private MoneyManager cash;
    [SerializeField] private ShiftManager shiftManager;
    private PlayerBehavior playerBehavior;

    [Header("State:")]
    [SerializeField] private RegisterState currentState = RegisterState.Free;

    [Header("Settings:")]
    [SerializeField] private string playerTag = "Player";
    [SerializeField] private float dialogueDelay = 2.0f;
    [SerializeField] int tip = 10;
    [SerializeField] int penalty = 20;

    private PlantSO currentPlantRequest;
    [SerializeField] private CustomerBehavior currentCustomer;
    private bool playerInZone = false;
    [SerializeField] Queue<CustomerBehavior> npcQueue = new();
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }  
    }
    private void Update()
    {
        if (playerBehavior == null)
        {
           // Debug.LogError("In CashRegister: cant find PlayerBehavior in the scene.");
            playerBehavior = FindObjectOfType<PlayerBehavior>();
        }
    }
    private void Start()
    {
        SetState(RegisterState.Free);
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(playerTag))
        {
            playerInZone = true;
            if (currentState == RegisterState.WaitingForPlayer)
            {
                StartCoroutine(GenerateRequestRoutine());
            }
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag(playerTag))
            playerInZone = false;
    }
    private IEnumerator GenerateRequestRoutine()
    {
        SetState(RegisterState.Processing);
        dialogueManager.StartPlayerDialogue();

        yield return new WaitForSeconds(dialogueDelay);

        dialogueManager.EndPlayerDialogue();

        currentPlantRequest = InventoryManager.Instance.GetRandomPlant();
        Debug.Log("In CashRegister: Random plant request: " + currentPlantRequest.name);

        if (currentPlantRequest != null)
        {
            dialogueManager.StartNpcDialogue(currentPlantRequest);
            OnPlantRequestGenerated?.Invoke(currentPlantRequest);
            SetState(RegisterState.WaitingForPlant);
            Debug.Log($"In CashRegister: new request generated: {currentPlantRequest.name}");
        }
        else
        {
            Debug.LogError("In CashRegister: failed to get a random plant. Check allPlants array in the Inspector");
            ResetCashRegister();
        }
    }
    public void HandleNpcArrival(CustomerBehavior arrivingCustomer)
    {
        if (currentState != RegisterState.Free)
        {
            npcQueue.Enqueue(arrivingCustomer);
            return;
        }

        currentCustomer = arrivingCustomer;

        if (currentState == RegisterState.Free && arrivingCustomer.GetCustomerType() == CustomerBehavior.CustomerType.AskForHelp)
        {
            Debug.Log("In CashRegister:  Boris has arrived. Waiting for Player.");
            SetState(RegisterState.WaitingForPlayer);

            if (playerInZone)
            {
                StartCoroutine(GenerateRequestRoutine());
            }
        }
        else
        {
            Debug.Log("In CashRegister:  Roaming NPC has arrived. Waiting for Player.");
            SetState(RegisterState.Processing);
        }
    }
    public void PlayerInteraction()
    {
        if (!playerInZone) return;

        switch (currentState)
        {
            case RegisterState.WaitingForPlayer:
                StartCoroutine(GenerateRequestRoutine());
                break;
            case RegisterState.WaitingForPlant:
                if (currentCustomer.GetCustomerType() == CustomerBehavior.CustomerType.AskForHelp)
                {
                    CheckDeliveredPlant();
                }
                else
                {
                    PlantPayment();
                }
                break;
            case RegisterState.Processing:
                PlantPayment();
                break;
        }
    }
    private void PlantPayment()
    {
        GameObject plantPicked = currentCustomer.GetPickedPlant(); 
        if (plantPicked != null)
        {
            PlantSO plantData = plantPicked.GetComponent<Plant>().CurrentPlantType;

            Debug.Log($"In CashRegister: Processing payment for {plantPicked.name}.");

            cash.AddMoney(plantData.price);
            shiftManager.AddCustomerServed();
            shiftManager.AddRevenue(plantData.price);

            currentCustomer.ForceExit();
            ResetCashRegister();
        }
        else
        {
            Debug.LogWarning("NPC arrived without a plant.");
            currentCustomer.ForceExit();
            ResetCashRegister();
        }
    }
    public void CheckDeliveredPlant()
    {
        PlantSO deliveredPlant = playerBehavior.heldPlantData;

        if (deliveredPlant == null)
        {
            Debug.Log("Player has no plant.");
            return;
        }

        if (deliveredPlant.name == currentPlantRequest.name)
        {
            Debug.Log("In CashRegister: Correct plant! Tip added.");
            FindObjectOfType<PlayerBehavior>().ChoseRightPlantForCustomer();
            cash.AddMoney(deliveredPlant.price);
            cash.AddTip(tip);
            shiftManager.AddCustomerServed();
            shiftManager.AddRevenue(deliveredPlant.price);
            if (currentCustomer != null)
            {
                currentCustomer.ForceExit();
            } 
            ResetCashRegister();
        }
        else
        {
            Debug.Log($"Comparison failed. Requested: '{currentPlantRequest.name}', Delivered: '{deliveredPlant.name}'");
            ApplyLeavingPenalty();            
        }
         
    }
    public void ApplyLeavingPenalty()
    {     
        Debug.Log("Wrong plant!");
        cash.SubtractMoney(penalty);
        if (currentCustomer != null)
        {
            currentCustomer.ForceExit();
        }
        ResetCashRegister();
    }
    private void ResetCashRegister()
    {
        SetState(RegisterState.Free);
        dialogueManager.EndnpcDialogue(); 
        currentPlantRequest = null;
        currentCustomer = null;

        OnInteractionEnded?.Invoke();
        NPCLineEvents.LeaveLine();

        if(npcQueue.Count > 0)
        {
            HandleNpcArrival(npcQueue.Dequeue());
        }

        Debug.Log("In CashRegister: Cash Register Reset.");
    }
    private void SetState(RegisterState newState)
    {
        if (currentState == newState) return;
        currentState = newState;
    }
}