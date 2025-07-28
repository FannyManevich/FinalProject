using Assets.Scripts.Managers;
using UnityEngine;
using UnityEngine.UI;

public class SeconsCustomer : MonoBehaviour
{
    Vector2 FlowerPoint;
    Vector2 StartPoint;
    Vector2 RegisterPoint;

    public Vector2 LineOffset = new Vector2(0, 0);
    [SerializeField] Slider TimerBar;
    private float minX, minY, maxX, maxY;
    public int CurrentWaitTimer = 0;
    public int WaitTime = 2500;
    float step;

    Vector2 PlantOffset;
    public GameObject[] Plants;

    //bool onPlant;
    GameObject PlantYouPick;

    GameObject UiManager;

    NPC_LineManagement LineManager;
    public int CurrentCustomerNumber = 0;

    public Animator animator;
    public SpriteRenderer sr;

    public NPC_State CurrentState;

    //Fany
    public PlantSO requestedPlant;
    public MoneyManager cash;
    public int impatiencePenalty = 10;
    private CashRegister cashRegister;
    //

    // Start is called before the first frame update
    void Start()
    {
        if (UiManager == null)
        {
            UiManager = GameObject.Find("UiManager");
        }

        sr = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        cash = FindObjectOfType<MoneyManager>();
        //Fany
        cashRegister = FindObjectOfType<CashRegister>();
        //

        if (cash == null)
        {
            Debug.LogError("In SeconsCustomer: Could not find an active MoneyManager in the scene ");
        }

        step = 4.0f * Time.deltaTime;
        TimerBar.gameObject.SetActive(false);
        StartPoint = new Vector2(11, -4.5f);
        RegisterPoint = new Vector2(0, 1.3f);
        transform.position.Set(StartPoint.x, StartPoint.y, 0);
        SetMinMax();

        Plants = GameObject.FindGameObjectsWithTag("Plant");

        //line
        LineManager = GameObject.FindGameObjectWithTag("LineManager").GetComponent<NPC_LineManagement>();
        NPCEventManager.LineLeaveEvent += ProgInLine;
        NPCEventManager.RegisterReleaseEvent += RegisterInteraction;

        NPCEventManager.EnterLine();
        CurrentCustomerNumber = LineManager.CustomerNumber;
        LineOffset = new Vector2(0 - CurrentCustomerNumber, 0);
        CurrentState = NPC_State.WalkToLine;
    }
    // Update is called once per frame
    void Update()
    {
        switch (CurrentState)
        {
            case NPC_State.WalkToLine:
                MoveToPointXFirst(RegisterPoint + LineOffset);
                WaitTime = 5000;
                break;
            case NPC_State.InLine:
                TimerBar.gameObject.SetActive(true);
                if (new Vector2(transform.position.x, transform.position.y) != RegisterPoint + LineOffset)
                {
                    MoveToPointXFirst(RegisterPoint + LineOffset);
                }
                if (CurrentWaitTimer < WaitTime)
                {
                    CurrentWaitTimer += 1;
                    TimerBar.value = (1 - (float)CurrentWaitTimer / (float)WaitTime);
                }
                else
                {
                    CurrentWaitTimer = 0;
                    //Fany
                    Debug.Log("in SeconsCustomer: Customer left impatiently! Applying penalty.");
                    if (cash != null) cash.SubtractMoney(impatiencePenalty);
                    if (cashRegister != null) cashRegister.CompleteTransaction();
                    //
                    NextState();
                }
                break;
            case NPC_State.Exit:
                MoveToPointYFirst(StartPoint);
                if (transform.position == new Vector3(StartPoint.x, StartPoint.y, 0))
                {
                    Destroy(this.gameObject);
                }
                break;
        }

    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Plant")
        {
            //onPlant = true;
            PlantYouPick = collider.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Plant")
        {
            //onPlant = false;
        }
    }

    public void OnDestroy()
    {
        NPCEventManager.LineLeaveEvent -= ProgInLine;
      //  NPCEventManager.RegisterReleaseEvent -= RegisterInteraction;
    }

    public void NextState()
    {
        if (CurrentState == NPC_State.WalkToLine)
        {
            CurrentState = NPC_State.InLine;
        }
        else if (CurrentState == NPC_State.InLine)
        {//Fany
            //if (cashRegister != null)
            //{
            //    cashRegister.CompleteTransaction();
            //}
            //if (cash != null)
            //{
            //    Debug.Log("Customer left impatiently! Applying penalty.");
            //    cash.SubtractMoney(impatiencePenalty);
            //}
            TimerBar.gameObject.SetActive(false);
            NPCEventManager.LeaveLine();
            CurrentState = NPC_State.Exit;
        }
        else
        {
            NPCEventManager.LeaveLine();
        }
        //
    }
    public void SetMinMax()
    {
        minX = -10;
        minY = -5;
        maxX = 10;
        maxY = 5;
    }
    public void MoveToPointXFirst(Vector2 PointToWalkTo)
    {
        if (transform.position.x != PointToWalkTo.x)
        {
            animator.SetBool("isWalking", true);
            animator.SetBool("isWalkingSide", true);
            if (transform.position.x - PointToWalkTo.x > 0)
            {
                sr.flipX = true;
            }
            else
            {
                sr.flipX = false;
            }
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(PointToWalkTo.x, transform.position.y, 0), step);
        }
        else if (transform.position.y != PointToWalkTo.y)
        {
            sr.flipX = false;
            animator.SetBool("isWalkingSide", false);
            animator.SetBool("isWalking", true);
            if (transform.position.y - PointToWalkTo.y < 0)
            {
                animator.SetBool("isWalkingDown", true);
            }
            else
            {
                animator.SetBool("isWalkingDown", false);
            }
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

    public void MoveToPointYFirst(Vector2 PointToWalkTo)
    {
        if (transform.position.y != PointToWalkTo.y)
        {
            animator.SetBool("isWalking", true);
            if (transform.position.y - PointToWalkTo.y < 0)
            {
                animator.SetBool("isWalkingDown", true);
            }
            else
            {
                animator.SetBool("isWalkingDown", false);
            }
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, PointToWalkTo.y, 0), step);
        }
        else if (transform.position.x != PointToWalkTo.x)
        {
            animator.SetBool("isWalkingDown", false);
            animator.SetBool("isWalking", true);
            animator.SetBool("isWalkingSide", true);
            if (transform.position.x - PointToWalkTo.x > 0)
            {
                sr.flipX = true;
            }
            else
            {
                sr.flipX = false;
            }
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
    public void RegisterInteraction()
    {
        if (CurrentState != NPC_State.InLine || CurrentCustomerNumber != 1)
        {
            return;
        }
           int x;
        if (PlantYouPick != null)
        {
            if (CurrentState == NPC_State.InLine && CurrentCustomerNumber == 1)
            {
                //Fany
                //if((PlantMatching.CheckPlantMatch(PlantYouPick, requestedPlant))
                //{
                    x = PlantYouPick.GetComponent<Plant>().currentPlantType.Price;
                    cash.AddMoney(x);
                    Destroy(PlantYouPick);
               // }
                if (cashRegister != null)
                {
                    cashRegister.CompleteTransaction();
                }
                //
                NextState();
            }
            else
            {
                Debug.LogWarning("Plant component or PlantType is missing!");
                return;
            }
        }
    }
    public void ProgInLine()
    {
        if (CurrentCustomerNumber > 1)
        {
            CurrentCustomerNumber -= 1;
        }
        LineOffset = new Vector2(0 - CurrentCustomerNumber, 0);
    }
    public void SetRandomPlantToWalkTo()
    {
        if (Plants.Length == 0)
        {
            if (transform.position.x == StartPoint.x && transform.position.y == StartPoint.y)
            {
                SetRandomWalkToPoint();
            }
            else
            {
                CurrentState = NPC_State.Exit;
            }
        }
        else if (CheckPlants(Plants))
        {
            GameObject RandPlant = Plants[Random.Range(0, Plants.Length)];
            FlowerPoint = new Vector2(RandPlant.transform.position.x + PlantOffset.x, RandPlant.transform.position.y + PlantOffset.y);
        }
        else
        {
            CurrentState = NPC_State.Exit;
        }
    }
    public bool CheckPlants(GameObject[] Plants)
    {
        foreach (GameObject Plant in Plants)
        {
            if (Plant.activeSelf == true)
            {
                return true;
            }
        }
        return false;
    }
    public void SetRandomWalkToPoint()
    {
        FlowerPoint = new Vector2(Random.Range(minX, maxX), Random.Range(minY, maxY));
    }
}