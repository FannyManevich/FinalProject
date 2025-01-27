using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UI;

public class CustomerBehaveior : MonoBehaviour
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

    LineManagment LineManager;
    public int CurrentCustomerNumber = 0;

    public enum StateMachine
    {
        WalkToFlower,
        WalkToLine,
        Wait,
        InLine,
        Exit
    }

    public StateMachine CurrentState;


    // Start is called before the first frame update
    void Start()
    {
        step = 4.0f * Time.deltaTime;
        TimerBar.gameObject.SetActive(false);
        StartPoint = new Vector2(-11, 0);
        RegisterPoint = new Vector2(10, 0);
        transform.position.Set(StartPoint.x, StartPoint.y, 0);
        SetMinMax();

        //plants as fixed points
        PlantOffset = new Vector2 (1, 0);
        Plants = GameObject.FindGameObjectsWithTag("Plant");
        SetRandomPlantToWalkTo();

        //line
        LineManager = GameObject.FindGameObjectWithTag("LineManager").GetComponent<LineManagment>();
        EventManager.LineLeaveEvent += ProgInLine;
        EventManager.RegisterRealeseEvent += RegisterInteraction;
    }

    // Update is called once per frame
    void Update()
    {
        switch (CurrentState)
        {
            case StateMachine.WalkToFlower:
                MoveToPointXFirst(FlowerPoint);
                break;
            case StateMachine.Wait:
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
                    CurrentState = StateMachine.WalkToFlower;
                    SetRandomPlantToWalkTo();
                    TimerBar.gameObject.SetActive(false);
                    CurrentWaitTimer = 0;
                }
                break;
            case StateMachine.WalkToLine:
                MoveToPointYFirst(RegisterPoint+LineOffset);
                WaitTime = 5000;
                break;
            case StateMachine.InLine:
                TimerBar.gameObject.SetActive(true);
                if (new Vector2(transform.position.x,transform.position.y) != RegisterPoint + LineOffset)
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
            case StateMachine.Exit:
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
        EventManager.LineLeaveEvent -= ProgInLine;
        EventManager.RegisterRealeseEvent -= RegisterInteraction;
    }

    public void NextState()
    {
        if (CurrentState == StateMachine.WalkToFlower)
        {
            CurrentState = StateMachine.Wait;
        }
        else if (CurrentState == StateMachine.WalkToLine)
        {
            CurrentState = StateMachine.InLine;
        }
        else if (CurrentState == StateMachine.Wait)
        {
            PlantYouPick.SetActive(false);
            TimerBar.gameObject.SetActive(false);
            EventManager.EnterLine();
            CurrentCustomerNumber = LineManager.CustomerNumber;
            LineOffset = new Vector2(0- CurrentCustomerNumber, 0);
            CurrentState = StateMachine.WalkToLine;
        }else if (CurrentState == StateMachine.InLine)
        {
            TimerBar.gameObject.SetActive(false);
            EventManager.LeaveLine();
            CurrentState = StateMachine.Exit;
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
        if (CurrentState == StateMachine.InLine && CurrentCustomerNumber == 1)
        {
            NextState();
        }
    }

    public void ProgInLine()
    {
        CurrentCustomerNumber -= 1;
        LineOffset = new Vector2(LineOffset.x + 1, 0);
    }

    public void SetRandomPlantToWalkTo()
    {
        if (Plants.Length==0)
        {
            if (transform.position.x == StartPoint.x && transform.position.y == StartPoint.y)
            {
                SetRandomWalkToPoint();
            }
            else
            {
                CurrentState = StateMachine.Exit;
            }
        }
        else
        {
            GameObject RandPlant = Plants[Random.Range(0, Plants.Length)];
            FlowerPoint = new Vector2(RandPlant.transform.position.x + PlantOffset.x, RandPlant.transform.position.y + PlantOffset.y);
        }
    }

    public void SetRandomWalkToPoint()
    {
        FlowerPoint = new Vector2(Random.Range(minX, maxX), Random.Range(minY, maxY));
    }
}
