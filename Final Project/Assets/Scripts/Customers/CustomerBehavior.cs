using Assets.Scripts.Managers;

using UnityEngine;
using UnityEngine.UI;

public class CustomerBehavior : MonoBehaviour
{
    Vector2 FlowerPoint;
    Vector2 StartPoint;
    Vector2 RegisterPoint;
    
    public Vector2 LineOffset = new Vector2(0,0);
    [SerializeField] Slider TimerBar;
    private float minX,minY,maxX,maxY;
    public int CurrentWaitTimer = 0;
    public int WaitTime = 500;
    float step;

    Vector2 PlantOffset;
    public GameObject[] Plants;

    bool onPlant;
    GameObject PlantYouPick;

    GameObject UiManager;

    NPC_LineManagement LineManager;
    public int CurrentCustomerNumber = 0;

    public Animator animator;
    public SpriteRenderer sr;

    public NPC_State CurrentState;


    // Start is called before the first frame update
    void Start()
    {
        if (UiManager == null)
        {
            UiManager = GameObject.Find("UIManager");
        }

        sr = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        step = 4.0f * Time.deltaTime;
        TimerBar.gameObject.SetActive(false);
        StartPoint = new Vector2(11, -4.5f);
        RegisterPoint = new Vector2(0, 1.3f);
        transform.position.Set(StartPoint.x, StartPoint.y, 0);
        SetMinMax();

        //plants as fixed points
        PlantOffset = new Vector2 (1, 0);
        Plants = GameObject.FindGameObjectsWithTag("Plant");
        SetRandomPlantToWalkTo();

        //line
        LineManager = GameObject.FindGameObjectWithTag("LineManager").GetComponent<NPC_LineManagement>();
        NPCEventManager.LineLeaveEvent += ProgInLine;
        NPCEventManager.RegisterReleaseEvent += RegisterInteraction;

    }

    // Update is called once per frame
    void Update()
    {

        switch (CurrentState)
        {
            case NPC_State.WalkToFlower:
                MoveToPointXFirst(FlowerPoint);
                break;
            case NPC_State.Wait:
                if (onPlant == true)
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
                else
                {
                    CurrentState = NPC_State.WalkToFlower;
                    SetRandomPlantToWalkTo();
                    TimerBar.gameObject.SetActive(false);
                    CurrentWaitTimer = 0;
                }
                break;
            case NPC_State.WalkToLine:
                MoveToPointYFirst(RegisterPoint+LineOffset);
                WaitTime = 5000;
                break;
            case NPC_State.InLine:
                TimerBar.gameObject.SetActive(true);
                if (new Vector2(transform.position.x,transform.position.y) != RegisterPoint + LineOffset)
                {
                    MoveToPointYFirst(RegisterPoint + LineOffset);
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
                MoveToPointXFirst(StartPoint);
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
            onPlant = true;
            PlantYouPick = collider.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Plant")
        {
            onPlant = false;
        }
    }

    public void OnDestroy()
    {
        NPCEventManager.LineLeaveEvent -= ProgInLine;
        NPCEventManager.RegisterReleaseEvent -= RegisterInteraction;
    }

    public void NextState()
    {
        if (CurrentState == NPC_State.WalkToFlower)
        {
            CurrentState = NPC_State.Wait;
        }
        else if (CurrentState == NPC_State.WalkToLine)
        {
            CurrentState = NPC_State.InLine;
        }
        else if (CurrentState == NPC_State.Wait)
        {
            PlantYouPick.SetActive(false);
            TimerBar.gameObject.SetActive(false);
            NPCEventManager.EnterLine();
            CurrentCustomerNumber = LineManager.CustomerNumber;
            LineOffset = new Vector2(0- CurrentCustomerNumber, 0);
            CurrentState = NPC_State.WalkToLine;
        }else if (CurrentState == NPC_State.InLine)
        {
            TimerBar.gameObject.SetActive(false);
            NPCEventManager.LeaveLine();
            CurrentState = NPC_State.Exit;
        }
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
            if(transform.position.y - PointToWalkTo.y < 0)
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
        if (CurrentState == NPC_State.InLine && CurrentCustomerNumber == 1)
        {
            UiManager.GetComponent<MoneyManager>().AddMoney(PlantYouPick.GetComponent<Plant>().currentPlantType.Price); 
            NextState();
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
