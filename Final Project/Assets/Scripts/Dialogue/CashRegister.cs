using System.Collections;
using UnityEngine;

public class CashRegister : MonoBehaviour
{
    private bool playerInZone = false;
    private bool npcInZone = false;
    private bool npcAtRegister = false;
    private bool waitingForPlant = false;

    [SerializeField] DialogueManager dialogueManager;
    [SerializeField] GameStateManager instance;

    public PlayerSelector selectedPlayer;
    public PlayerSO activePlayer;
    [SerializeField] NPC_SO[] npcList;
    public GameObject newNPC;

    [SerializeField] PlantSO[] plantSO;
    public PlantSO randomPlantSO;
    private int randomP;

    public DialogueUI dialogueUI;
    public PlantSO emptyPlant;
    public PlayerSO playerPickedPlant;
    public MoneyManager cash;
    public Plant plant;
    public PlantRequest pr;
    private int pcount = 0;
    public PlantSO[] allPlants;
    private PlantSO currentRequest;
    private PlantSO deliveredPlant;

    public PlayerBehavior pb;

    [SerializeField] float waitForPlayerDialogue = 3;
    void Start()
    {
        pb = FindObjectOfType<PlayerBehavior>();
        if (dialogueManager == null)
            dialogueManager = FindObjectOfType<DialogueManager>();

        if (instance == null)
            instance = FindObjectOfType<GameStateManager>();

        if (selectedPlayer == null)
            activePlayer = instance.selectedPlayerSO;
    }

    private void OnEnable()
    {
        NPCSpawnerScript.OnCustomerSpawnedEvent += ReadNPCData;
    }

    private void OnDisable()
    {
        NPCSpawnerScript.OnCustomerSpawnedEvent -= ReadNPCData;
    }

    private void ReadNPCData(GameObject newNPC)
    {
        if (newNPC == null)
        {
            Debug.LogError("newNPC is null in ReadNPCData!");
            return;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInZone = true;
           // pcount++;
            Debug.Log(pcount);
        }

        if (other.CompareTag("Boris"))
            npcInZone = true;

        if (playerInZone && npcInZone)
        {
            Debug.Log("In CashRegister: player and NPC r in cash register");
            pcount++;
            Debug.Log(pcount);
            dialogueManager.StartPlayerDialogue();
            if (pb.PlantYouAreHolding != null)
            {
                DeliverPlant();
                dialogueManager.EndPlayerDialogue();
            }
            else
                StartCoroutine(WaitForPlayerDialogue());

        }
        // Check how many times player came 2 cach regis
        if (pcount < 0 || pcount >= 2)
        {
            pcount = 0;
        }
    }

    IEnumerator WaitForPlayerDialogue()
    {

        yield return new WaitForSeconds(waitForPlayerDialogue);
        dialogueManager.EndPlayerDialogue();

        //Create random plant request

        emptyPlant = GetRandomPlant();

        dialogueManager.StartNpcDialogue(emptyPlant);
        waitingForPlant = true;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            playerInZone = false;

        if (other.CompareTag("Boris"))
        {
            npcInZone = false;
        }
    }

    //player puts plant at cash reg
    public void DeliverPlant()
    {
        if (!waitingForPlant || pb.PlantYouAreHolding == null) return;
      
        deliveredPlant = pb.PlantYouAreHolding.GetComponent<Plant>().currentPlantType;
        waitingForPlant = false;

        bool correct = CheckPlantMatch(deliveredPlant, emptyPlant);

        if (correct)
        {
            Debug.Log("Correct plant! Tip added.");
            cash.currentMoney += deliveredPlant.Price;
        }
        else
        {
            Debug.Log("Wrong plant! Penalty.");
            cash.currentMoney -= 10;
        }

        ResetState();
    }

    private bool CheckPlantMatch(PlantSO a, PlantSO b)
    {
        return a.sunRequirement == b.sunRequirement &&
               a.waterRequirement == b.waterRequirement &&
               a.difficultyLevel == b.difficultyLevel;
    }

    private PlantSO GetRandomPlant()
    {
        return plantSO[Random.Range(0, allPlants.Length - 1)];
    }

    private void ResetState()
    {
        npcAtRegister = false;
        deliveredPlant = null;
        currentRequest = null;
        var plantToDestory = pb.PlantYouAreHolding;
        pb.PlantYouAreHolding = null;
        Destroy(plantToDestory);
    }
}