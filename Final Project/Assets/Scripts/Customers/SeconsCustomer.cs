using Assets.Scripts.Managers;
using System.Collections;
using System.Collections.Generic;
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
    public int WaitTime = 500;
    float step;

    Vector2 PlantOffset;
    public GameObject[] Plants;

    bool onPlant;
    GameObject PlantYouPick;

    GameObject UiManager;

    NPC_LineManagement LineManager;
    public int CurrentCustomerNumber = 0;

    public NPC_State CurrentState;


    // Start is called before the first frame update
    void Start()
    {
        if (UiManager == null)
        {
            UiManager = GameObject.Find("UiManager");
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
        CurrentState = NPC_State.WalkToLine;

    }

    // Update is called once per frame
    void Update()
    {
        switch (CurrentState)
        {
            case NPC_State.WalkToLine:
                MoveToPointYFirst(RegisterPoint + LineOffset);
                WaitTime = 5000;
                break;
            case NPC_State.InLine:
                TimerBar.gameObject.SetActive(true);
                if (new Vector2(transform.position.x, transform.position.y) != RegisterPoint + LineOffset)
                {
                    MoveToPointYFirst(RegisterPoint + LineOffset);
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
        if (CurrentState == NPC_State.WalkToLine)
        {
            CurrentState = NPC_State.InLine;
        }
        else if (CurrentState == NPC_State.InLine)
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
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(PointToWalkTo.x, transform.position.y, 0), step);
        }
        else if (transform.position.y != PointToWalkTo.y)
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, PointToWalkTo.y, 0), step);
        }
        else
        {
            NextState();
        }
    }

    public void MoveToPointYFirst(Vector2 PointToWalkTo)
    {
        if (transform.position.y != PointToWalkTo.y)
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, PointToWalkTo.y, 0), step);
        }
        else if (transform.position.x != PointToWalkTo.x)
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(PointToWalkTo.x, transform.position.y, 0), step);
        }
        else
        {
            NextState();
        }
    }

    public void RegisterInteraction()
    {
        if (CurrentState == NPC_State.InLine && CurrentCustomerNumber == 1)
        {
            UiManager.GetComponent<MoneyManager>().addMoney(PlantYouPick.GetComponent<Plant>().currentPlantType.Price);
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