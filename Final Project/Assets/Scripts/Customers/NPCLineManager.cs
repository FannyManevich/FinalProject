using UnityEngine;

public class NPCLineManager : MonoBehaviour
{
    public int customerNumber = 0;

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
        customerNumber++;
    }

    private void CustomerLeaveLine()
    {
        customerNumber--;
    }
}
