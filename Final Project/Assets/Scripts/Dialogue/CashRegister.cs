using UnityEngine;

public class CashRegister : MonoBehaviour
{
    private bool playerInZone = false;
    private bool npcInZone = false;

    public DialogueManager dialogueManager;
    public GameStateManager instance;
    
    public PlayerSelector selectedPlayer;
    public PlayerSO activePlayer;
    public NPC_SO[] npcList;
    public GameObject newNPC;

    public PlantSO[] plantSO;
    public PlantSO randomPlantSO;
    private int randomP;

    public DialogueUI dialogueUI;
    public PlantSO emptyPlant;
    public PlayerSO playerPickedPlant;
    public MoneyManager cash;
    public Plant plant;
    public PlantRequest pr;
    private int pcount = 0;

    void Start()
    {
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
            pcount++;
        }
            

        if (other.CompareTag("Boris"))       
            npcInZone = true;

        if (playerInZone && npcInZone)
        {
            Debug.Log("In CashRegister: player and NPC r in cash register");
 
            dialogueManager.StartPlayerDialogue();

            dialogueManager.EndPlayerDialogue();

            //Create random plant request

            pr.GenerateRequest(emptyPlant);
            dialogueManager.StartNpcDialogue(randomPlantSO);
        }
        // Check how many times player came 2 cach regis
        if(pcount < 0 || pcount >= 2)
        {
            pcount = 0;
        }
    }
    public void Compare(PlantSO emptyPlant, GameObject player)
    {
        if (emptyPlant == player.GetComponent<Plant>().currentPlantType)
        {
            cash.currentMoney += plant.currentPlantType.Price;
        }
        else
        {
            cash.currentMoney -= 10;
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            playerInZone = false;

        if (other.CompareTag("Boris"))
        {
            npcInZone = false;
            pcount--;
        }
    }
}