using System.Collections;
using UnityEngine;
using Assets.Scripts.Managers;
using System;

public class CashRegister : MonoBehaviour
{
    public static CashRegister Instance { get; private set; }
    public static event Action<PlantSO> OnPlantRequestGenerated;
    public static event Action OnInteractionEnded;

    [Header("Component References:")]
    [SerializeField] private DialogueManager dialogueManager;
    [SerializeField] private MoneyManager cash;
    [SerializeField] private ShiftManager shiftManager;
    private PlayerBehavior playerBehavior;

    [Header("State:")]
    [SerializeField] private RegisterState currentState = RegisterState.Free;

    [Header("Player & NPC:")]

    [Header("Settings:")]
    [SerializeField] private string playerTag = "Player";
    [SerializeField] private float dialogueDelay = 2.0f;

    private PlantSO currentPlantRequest;
    private bool playerInZone = false;
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
    private void OnEnable()
    {
        NPCLineManager.LineEnterEvent += HandleNpcArrival;
      //  NPCSpawner.OnCustomerSpawnedEvent += ReadNPCData;           
    }
    private void OnDisable()
    {
        NPCLineManager.LineEnterEvent -= HandleNpcArrival;
       // NPCSpawner.OnCustomerSpawnedEvent -= ReadNPCData;       
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(playerTag))
        {
            playerInZone = true;
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
    private void HandleNpcArrival()
    {
        if (currentState == RegisterState.Free)
        {
            Debug.Log("In CashRegister:  NPC has arrived. Waiting for Player.");
            SetState(RegisterState.WaitingForPlayer);           
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
                CheckDeliveredPlant();
                break;
        }
    }
    public void CheckDeliveredPlant()
    {
        PlantSO deliveredPlant = playerBehavior.HeldPlantData;

        if (deliveredPlant == null)
        {
            Debug.Log("Player has no plant.");
            return;
        }

        if (deliveredPlant == currentPlantRequest)
        {
            Debug.Log("In CashRegister: Correct plant! Tip added.");
            cash.AddMoney(deliveredPlant.Price);
            cash.AddTip(5);
            shiftManager.AddCustomerServed();
            shiftManager.AddRevenue(deliveredPlant.Price);
        }
        else
        {
            Debug.Log("Wrong plant! -10$");
            cash.SubtractMoney(10);
        }      
        ResetCashRegister();
    }
    private void ResetCashRegister()
    {
        dialogueManager.EndnpcDialogue(); 
        currentPlantRequest = null;
        OnInteractionEnded?.Invoke();
        SetState(RegisterState.Free);
        Debug.Log("In CashRegister: Cash Register Reset.");
    }
    private void SetState(RegisterState newState)
    {
        if (currentState == newState) return;
        currentState = newState;
    }
}