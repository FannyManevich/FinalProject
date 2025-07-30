// Ignore Spelling: Prog
using Assets.Scripts.Managers;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CustomerMovement))]
public class CustomerInteractions : MonoBehaviour
{
    public enum CustomerType {Roaming,  AskForHelp}

    [Header("Behavior Setup:")]
    [SerializeField] private CustomerType customerType;
    [SerializeField] private int waitTime = 500;
    [SerializeField] private int impatiencePenalty = 10;

    [Header("UI:")]
    [SerializeField] private Slider timerBar;
    public NPC_State CurrentState { get; private set; }
    //private void OnArrival() => NextState();
    public int CurrentWaitTimer { get; private set; }

    private CustomerMovement movement;
    private MoneyManager cash;
    private CashRegister cashRegister;
    private NPC_LineManagement lineManager;
    private GameObject PlantYouPick;
    private int currentCustomerNumber = 0;

    private readonly Vector2 startPoint = new Vector2(11, -4.5f);
    private readonly Vector2 registerPoint = new Vector2(0, 1.3f);

    private void Awake()
    {
        movement = GetComponent<CustomerMovement>();
        cash = FindObjectOfType<MoneyManager>();
        cashRegister = FindObjectOfType<CashRegister>();
        lineManager = GameObject.FindGameObjectWithTag("LineManager").GetComponent<NPC_LineManagement>();
    }
    private void OnEnable() { movement.OnDestinationReached += OnArrival; }
    private void OnDisable() { movement.OnDestinationReached -= OnArrival; }

    private void Start()
    {
        if (timerBar) 
        {
            timerBar.gameObject.SetActive(false);
        }

        CurrentState = NPC_State.EnterScene;
        movement.MoveTo(startPoint);

        NPCEventManager.CustomerArrived();

        //if (customerType == CustomerType.Roaming)
        //{
        //    SetRandomPlantToWalkTo();
        //}           
        //else GoToLine();
    }

    private void Update()
    {
        if (CurrentState == NPC_State.InLine)
        {
            InLineState();
        }
        else
        {
            if (CurrentState == NPC_State.Wait)
            {
                RoamingWaitState();
            }              
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Plant"))
        {
            PlantYouPick = other.gameObject;
        }           
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Plant") && other.gameObject == PlantYouPick)
            PlantYouPick = null;
    }

    void OnArrival()
    {
        if (CurrentState == NPC_State.EnterScene)
        {
            InitializeBehavior();
            return;
        }

        NextState();
    }
    private void InitializeBehavior()
    {
        if (customerType == CustomerType.Roaming)
        {
            SetRandomPlantToWalkTo();
        }
        else
        {
            GoToLine();
        }
    }
    public void RegisterInteraction()
    {
        if (CurrentState != NPC_State.InLine || currentCustomerNumber != 1)
            return;
        if (customerType == CustomerType.AskForHelp && PlantYouPick != null)
        {
            cash.AddMoney(PlantYouPick.GetComponent<Plant>().currentPlantType.Price);
            Destroy(PlantYouPick);
        }
        if (cashRegister != null) cashRegister.CompleteTransaction();
        NextState();
    }
    public void ProgInLine()
    {
        if (currentCustomerNumber > 0)
        {
            currentCustomerNumber -= 1;
        }

        Vector2 lineOffset = new Vector2(0 - currentCustomerNumber, 0);

        if (CurrentState == NPC_State.InLine)
        {
            movement.MoveTo(registerPoint + lineOffset);
        }       
    }
    private void NextState()
    {
        if (customerType == CustomerType.Roaming && CurrentState == NPC_State.WalkToFlower)
            CurrentState = NPC_State.Wait;
        else if (customerType == CustomerType.Roaming && CurrentState == NPC_State.Wait)
        {
            if (PlantYouPick) 
                PlantYouPick.SetActive(false);
            if (timerBar)
                timerBar.gameObject.SetActive(false);
            GoToLine();
        }
        else if (CurrentState == NPC_State.WalkToLine) CurrentState = NPC_State.InLine;
        else if (CurrentState == NPC_State.InLine)
        {
            if (timerBar) timerBar.gameObject.SetActive(false);
            NPCEventManager.LeaveLine();
            CurrentState = NPC_State.Exit;
            movement.MoveTo(startPoint);
        }
        else if (CurrentState == NPC_State.Exit)
                Destroy(gameObject);
    }
    private void GoToLine()
    {
        currentCustomerNumber = lineManager.CustomerNumber;
        NPCEventManager.EnterLine();
        
        Vector2 lineOffset = new Vector2(0 - currentCustomerNumber, 0);
        CurrentState = NPC_State.WalkToLine;
        movement.MoveTo(registerPoint + lineOffset);
    }
    private void InLineState()
    {
        if (timerBar) timerBar.gameObject.SetActive(true);
        if (CurrentWaitTimer < waitTime)
        {
            CurrentWaitTimer++;
            if (timerBar) timerBar.value = (1 - (float)CurrentWaitTimer / waitTime);
        }
        else
        {
            CurrentWaitTimer = 0;
            if (cash != null) cash.SubtractMoney(impatiencePenalty);
            if (cashRegister != null) cashRegister.CompleteTransaction();
            NPCEventManager.CustomerWalkout(impatiencePenalty);
            NextState();
        }
    }
    private void RoamingWaitState()
    {
        if (PlantYouPick == null) { NextState(); return; }

        if (timerBar) timerBar.gameObject.SetActive(true);
        if (CurrentWaitTimer < waitTime)
        {
            CurrentWaitTimer++;
            if (timerBar) timerBar.value = (1 - (float)CurrentWaitTimer / waitTime);
        }
        else
        { 
            CurrentWaitTimer = 0;
            NextState(); 
        }
    }
    private void SetRandomPlantToWalkTo()
    {
        GameObject[] plants = GameObject.FindGameObjectsWithTag("Plant");

        if (plants.Length > 0)
        {
            GameObject randPlant = plants[Random.Range(0, plants.Length)];
            Vector2 flowerPoint = randPlant.transform.position;

            CurrentState = NPC_State.WalkToFlower;
            movement.MoveTo(flowerPoint);

            //-------Kinematic-----
            //Collider2D plantCollider = randPlant.GetComponent<Collider2D>();
            //if (plantCollider != null)
            //{
            //    Vector2 flowerPoint = new Vector2(plantCollider.bounds.center.x - 1.0f, plantCollider.bounds.center.y);
            //    CurrentState = NPC_State.WalkToFlower;
            //    movement.MoveTo(flowerPoint);
            //}
            //else
            //{
            //    GoToLine();
            //}
            //Vector2 flowerPoint = new Vector2(randPlant.transform.position.x + 1, randPlant.transform.position.y);              
        }
        else GoToLine();
    }
}