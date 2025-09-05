using UnityEngine;

public class NPCLineManager : MonoBehaviour
{
    public int CustomerNumber = 0;

    // Start is called before the first frame update
    void Start()
    {
        NPCLineEvents.LineEnterEvent += CustomerEnterLine;
        NPCLineEvents.LineLeaveEvent += CustomerLeaveLine;
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
