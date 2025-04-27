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
        NPCSpawnerScript.OnCustomerSpawnedEvent += () => ReadNPCData(newNPC);
    }

    private void OnDisable()
    {
        NPCSpawnerScript.OnCustomerSpawnedEvent -= () => ReadNPCData(newNPC);
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
            playerInZone = true;

        if (other.CompareTag("NPC"))       
            npcInZone = true;     

        if (playerInZone && npcInZone)
        {
            dialogueManager.StartPlayerDialogue();
            Time.timeScale = 3f;

            dialogueManager.EndPlayerDialogue();

            if (randomP >= 0 && randomP < 6)
            {
                randomP = UnityEngine.Random.Range(0, 5);
                randomPlantSO = plantSO[randomP];
                Debug.Log("In CashRegister:  randomPlantSO : " + randomPlantSO);

                dialogueManager.StartNpcDialogue(randomPlantSO);
            }        
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            playerInZone = false;

        if (other.CompareTag("NPC"))
            npcInZone = false;
    }

}