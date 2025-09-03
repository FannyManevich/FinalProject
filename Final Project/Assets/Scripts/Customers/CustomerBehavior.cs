using Assets.Scripts.Managers;
using UnityEngine;
using UnityEngine.UI;
public class CustomerBehavior : MonoBehaviour
{
    public enum CustomerType { Roaming, AskForHelp }

    [Header("UI:")]
    [SerializeField] private Slider TimerBar;
    [SerializeField] private int WaitTimePlant;
    [SerializeField] private int WaitTimeLine;

    [Header("Customer:")]
    [SerializeField] private NPC_State CurrentState;
    [SerializeField] private float Speed;
    [SerializeField] private CustomerType customerType;

    private int WaitTime;

    private GameObject FlowerPoint;
    private GameObject StartPoint;
    private GameObject RegisterPoint;

    private Vector3 LineOffset = Vector3.zero;
    private int CurrentWaitTimer = 0;
    private float step;

    private Animator animator;
    private SpriteRenderer sr;

    private GameObject[] Plants;

    private bool onPlant;
    private GameObject PlantYouPick;

    private GameObject UiManager;

    private NPC_LineManagement LineManager;
    private int CurrentCustomerNumber = 0;


    // Start is called before the first frame update
    void Start()
    {
        if (UiManager == null)
        {
            UiManager = GameObject.FindGameObjectWithTag("UIManager");
        }

        Plants = GameObject.FindGameObjectsWithTag("Plant");

        if (customerType == CustomerType.Roaming)
        {
            SetRandomPlantToWalkTo();
            CurrentState = NPC_State.WalkToFlower;
        }
        else
        {
            CurrentState = NPC_State.WalkToLine;
        }

        StartPoint = GameObject.FindGameObjectWithTag("NPC");
        RegisterPoint = GameObject.FindGameObjectWithTag("Register");

        sr = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        step = Speed * Time.deltaTime;
        TimerBar.gameObject.SetActive(false);
        transform.position.Set(StartPoint.transform.position.x, StartPoint.transform.position.y, 0);

        //line
        LineManager = GameObject.FindGameObjectWithTag("LineManager").GetComponent<NPC_LineManagement>();
        NPCLineManager.LineLeaveEvent += ProgInLine;
        NPCLineManager.RegisterReleaseEvent += RegisterInteraction;
    }

    // Update is called once per frame
    void Update()
    {
        switch (CurrentState)
        {
            case NPC_State.WalkToFlower:
                MoveToPointXFirst(FlowerPoint.transform.position);
                WaitTime = WaitTimePlant;
                break;
            case NPC_State.Wait:
                if (onPlant)
                {
                    WaitOnPlant();
                }
                else
                {
                    CurrentState = NPC_State.WalkToFlower;
                    SetRandomPlantToWalkTo();
                    TimerBar.gameObject.SetActive(false);
                    CurrentWaitTimer = 0;
                }
                break;
            case NPC_State.WalkToLine:
                MoveToPointYFirst(RegisterPoint.transform.position + LineOffset);
                WaitTime = WaitTimeLine;
                break;
            case NPC_State.InLine:
                TimerBar.gameObject.SetActive(true);
                if (new Vector3(transform.position.x,transform.position.y,0) != RegisterPoint.transform.position + LineOffset)
                {
                    MoveToPointYFirst(RegisterPoint.transform.position + LineOffset);
                }
                else
                {
                    animator.SetBool("isWalking", false);
                }
                if (CurrentWaitTimer < WaitTime)
                {
                    CurrentWaitTimer += 1;
                    TimerBar.value = (1 - (float)CurrentWaitTimer / (float)WaitTime);
                }
                else
                {
                    CurrentWaitTimer = 0;
                    NextState();
                }
                break;
            case NPC_State.Exit:
                MoveToPointXFirst(StartPoint.transform.position);
                if (transform.position == StartPoint.transform.position)
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
            PlantYouPick = collider.gameObject;
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
        NPCLineManager.LineLeaveEvent -= ProgInLine;
        NPCLineManager.RegisterReleaseEvent -= RegisterInteraction;
    }
    private void NextState()
    {
        switch (CurrentState)
        {
            case NPC_State.WalkToFlower:
                CurrentState = NPC_State.Wait;
                break;
            case NPC_State.WalkToLine:
                CurrentState = NPC_State.InLine;
                break;
            case NPC_State.InLine:
                TimerBar.gameObject.SetActive(false);
                NPCLineManager.LeaveLine();
                CurrentState = NPC_State.Exit;
                break;
            case NPC_State.Wait:
                PlantYouPick.SetActive(false);
                TimerBar.gameObject.SetActive(false);
                NPCLineManager.EnterLine();
                CurrentCustomerNumber = LineManager.CustomerNumber;
                LineOffset = new Vector2(0 - CurrentCustomerNumber, 0);
                CurrentState = NPC_State.WalkToLine;
                break;
        }
    }

    private void MoveToPointXFirst(Vector2 PointToWalkTo)
    {
        if (transform.position.x != PointToWalkTo.x)
        {
            animator.SetBool("isWalking", true);
            animator.SetBool("isWalkingSide", true);
            sr.flipX = (transform.position.x - PointToWalkTo.x > 0);
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(PointToWalkTo.x, transform.position.y, 0), step);
        }
        else if (transform.position.y != PointToWalkTo.y)
        {
            sr.flipX = false;
            animator.SetBool("isWalkingSide", false);
            animator.SetBool("isWalking", true);
            animator.SetBool("isWalkingDown", (transform.position.y - PointToWalkTo.y < 0));
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, PointToWalkTo.y, 0), step);
        }
        else
        {
            animator.SetBool("isWalking", false);
            animator.SetBool("isWalkingDown", false);
            animator.SetBool("isWalkingSide", false);
            NextState();
        }
    }
    private void MoveToPointYFirst(Vector2 PointToWalkTo)
    {
        if (transform.position.y != PointToWalkTo.y)
        {
            animator.SetBool("isWalking", true);
            animator.SetBool("isWalkingDown", (transform.position.y - PointToWalkTo.y < 0));
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, PointToWalkTo.y, 0), step);
        }
        else if (transform.position.x != PointToWalkTo.x)
        {
            animator.SetBool("isWalkingDown", false);
            animator.SetBool("isWalking", true);
            animator.SetBool("isWalkingSide", true);
            sr.flipX = (transform.position.x - PointToWalkTo.x > 0);
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(PointToWalkTo.x, transform.position.y, 0), step);
        }
        else
        {
            sr.flipX = false;
            animator.SetBool("isWalking", false);
            animator.SetBool("isWalkingDown", false);
            animator.SetBool("isWalkingSide", false);
            NextState();
        }
    }
    private void RegisterInteraction()
    {
        if (CurrentState == NPC_State.InLine && CurrentCustomerNumber == 1)
        {
            UiManager.GetComponent<MoneyManager>().AddMoney(PlantYouPick.GetComponent<Plant>().currentPlantType.Price); 
            NextState();
        }
    }

    private void ProgInLine()
    {
        if (CurrentCustomerNumber > 1)
        {
            CurrentCustomerNumber -= 1;
        }
        LineOffset = new Vector2(-1*CurrentCustomerNumber, 0);
    }
    private void SetRandomPlantToWalkTo()
    {
        if (Plants.Length == 0)
        {
            CurrentState = NPC_State.Exit;
        }
        else if (CheckPlants(Plants))
        {
            FlowerPoint = Plants[Random.Range(0, Plants.Length)];
        }
        else
        {
            CurrentState = NPC_State.Exit;
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
        TimerBar.gameObject.SetActive(true);
        if (CurrentWaitTimer < WaitTime)
        {
            CurrentWaitTimer += 1;
            TimerBar.value = (1 - (float)CurrentWaitTimer / (float)WaitTime);
        }
        else
        {
            CurrentWaitTimer = 0;
            NextState();
        }
    }
}