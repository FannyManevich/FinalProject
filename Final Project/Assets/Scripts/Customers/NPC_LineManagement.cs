using UnityEngine;

public class NPC_LineManagement : MonoBehaviour
{
    public int CustomerNumber = 0;

    // Start is called before the first frame update
    void Start()
    {
        NPCEventManager.LineEnterEvent += CustomerEnterLine;
        NPCEventManager.LineLeaveEvent += CustomerLeaveLine;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void CustomerEnterLine()
    {
        CustomerNumber++;
    }

    private void CustomerLeaveLine()
    {
        CustomerNumber--;
    }
}
