using Assets.Scripts.Managers;
using UnityEngine;
using UnityEngine.UI;
public class CustomerBehavior : MonoBehaviour
{
    public enum CustomerType { Roaming, AskForHelp }

    [Header("UI:")]
    [SerializeField] private Slider timerBar;
    [SerializeField] private int WaitTimePlant;
    [SerializeField] private int WaitTimeLine;

    [Header("Customer:")]
    [SerializeField] private NPC_State currentState;
    [SerializeField] private CustomerType customerType;

    private Animator animator;
    private SpriteRenderer sr;
    private int waitTime;

    private GameObject flowerPoint;
    private GameObject startPoint;
    private GameObject registerPoint;

    private Vector3 lineOffset = Vector3.zero;
    private int currentWaitTimer = 0;

    private GameObject[] plants;

    private bool onPlant;
    private GameObject plantYouPick;

    private GameObject uiManager;
    private CustomerMovement customerMovement;

    private NPCLineManager lineManager;
    private int currentCustomerNumber = 0;


    // Start is called before the first frame update
    void Start()
    {
        if (uiManager == null)
        {
            uiManager = GameObject.FindGameObjectWithTag("UIManager");
        }

        customerMovement = gameObject.GetComponent<CustomerMovement>();

        plants = GameObject.FindGameObjectsWithTag("Plant");

        if (customerType == CustomerType.Roaming)
        {
            SetRandomPlantToWalkTo();
            currentState = NPC_State.WalkToFlower;
        }
        else
        {
            currentState = NPC_State.WalkToLine;
        }

        sr = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        startPoint = GameObject.FindGameObjectWithTag("NPC");
        registerPoint = GameObject.FindGameObjectWithTag("Register");

        timerBar.gameObject.SetActive(false);
        transform.position.Set(startPoint.transform.position.x, startPoint.transform.position.y, 0);

        //line
        lineManager = GameObject.FindGameObjectWithTag("LineManager").GetComponent<NPCLineManager>();
        NPCLineEvents.LineLeaveEvent += ProgInLine;
        NPCLineEvents.RegisterReleaseEvent += RegisterInteraction;
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentState)
        {
            case NPC_State.WalkToFlower:
                customerMovement.MoveToPointXFirst(flowerPoint.transform.position, true);
                break;
            case NPC_State.Wait:
                if (onPlant)
                {
                    WaitOnPlant();
                }
                else
                {
                    currentState = NPC_State.WalkToFlower;
                    SetRandomPlantToWalkTo();
                    timerBar.gameObject.SetActive(false);
                    currentWaitTimer = 0;
                }
                break;
            case NPC_State.WalkToLine:
                customerMovement.MoveToPointYFirst(registerPoint.transform.position + lineOffset,true);
                break;
            case NPC_State.InLine:
                WalkInLine();
                break;
            case NPC_State.Exit:
                customerMovement.MoveToPointXFirst(startPoint.transform.position, false);
                if (transform.position == startPoint.transform.position)
                {
                    Destroy(this.gameObject);
                }
                break;
        }
    }
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Plant"))
        {
            onPlant = true;
            plantYouPick = collider.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Plant"))
        {
            onPlant = false;
        }
    }

    public void OnDestroy()
    {
        NPCLineEvents.LineLeaveEvent -= ProgInLine;
        NPCLineEvents.RegisterReleaseEvent -= RegisterInteraction;
    }
    public void NextState()
    {
        switch (currentState)
        {
            case NPC_State.WalkToFlower:
                waitTime = WaitTimePlant;
                currentState = NPC_State.Wait;
                break;
            case NPC_State.WalkToLine:
                waitTime = WaitTimeLine;
                currentState = NPC_State.InLine;
                break;
            case NPC_State.InLine:
                timerBar.gameObject.SetActive(false);
                NPCLineEvents.LeaveLine();
                currentState = NPC_State.Exit;
                break;
            case NPC_State.Wait:
                plantYouPick.SetActive(false);
                timerBar.gameObject.SetActive(false);
                NPCLineEvents.EnterLine();
                currentCustomerNumber = lineManager.CustomerNumber;
                lineOffset = new Vector2(0 - currentCustomerNumber, 0);
                currentState = NPC_State.WalkToLine;
                break;
        }
    }

    private void RegisterInteraction()
    {
        if (currentState == NPC_State.InLine && currentCustomerNumber == 1)
        {
            uiManager.GetComponent<MoneyManager>().AddMoney(plantYouPick.GetComponent<Plant>().CurrentPlantType.price); 
            NextState();
        }
    }

    private void ProgInLine()
    {
        if (currentCustomerNumber > 1)
        {
            currentCustomerNumber -= 1;
        }
        lineOffset = new Vector2(-1*currentCustomerNumber, 0);
    }
    private void SetRandomPlantToWalkTo()
    {
        if (plants.Length == 0)
        {
            currentState = NPC_State.Exit;
        }
        else if (CheckPlants(plants))
        {
            flowerPoint = plants[Random.Range(0, plants.Length)];
        }
        else
        {
            currentState = NPC_State.Exit;
        }
    }
    private bool CheckPlants(GameObject[] Plants)
    {
        foreach (GameObject Plant in Plants)
        {
            if (Plant.activeSelf)
            {
                return true;
            }
        }
        return false;
    }

    private void WaitOnPlant()
    {
        timerBar.gameObject.SetActive(true);
        if (currentWaitTimer < waitTime)
        {
            currentWaitTimer += 1;
            timerBar.value = (1 - (float)currentWaitTimer / (float)waitTime);
        }
        else
        {
            currentWaitTimer = 0;
            NextState();
        }
    }

    private void WalkInLine()
    {
        timerBar.gameObject.SetActive(true);
        if (new Vector3(transform.position.x, transform.position.y, 0) != registerPoint.transform.position + lineOffset)
        {
            customerMovement.MoveToPointYFirst(registerPoint.transform.position + lineOffset,false);
        }
        else
        {
            animator.SetBool("isWalking", false);
        }
        if (currentWaitTimer < waitTime)
        {
            currentWaitTimer += 1;
            timerBar.value = (1 - (float)currentWaitTimer / (float)waitTime);
        }
        else
        {
            currentWaitTimer = 0;
            NextState();
        }
    }
}