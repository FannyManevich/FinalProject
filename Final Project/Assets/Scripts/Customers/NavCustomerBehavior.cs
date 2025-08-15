using UnityEngine;
using UnityEngine.AI;

public class NavCustomerBehavior : MonoBehaviour
{
    GameObject UiManager;
    public Animator animator;
    public SpriteRenderer sr;

    public GameObject[] Plants;

    Transform target;

    NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;

        sr = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        if (UiManager == null)
        {
            UiManager = GameObject.Find("UIManager");
        }

        Plants = GameObject.FindGameObjectsWithTag("Plant");
        SetRandomPlantToWalkTo();
    }
    private void SetRandomPlantToWalkTo()
    {
        target = Plants[Random.Range(0, Plants.Length)].transform;
    }
}