using System.Collections;
using UnityEngine;

public class CashRegister : MonoBehaviour
{
    [Header("Component References")]
    [SerializeField] private DialogueManager dialogueManager;
    [SerializeField] private GameStateManager instance;
    [SerializeField] private MoneyManager cash;

    [Header("Player & NPC")]
    public PlayerSO activePlayer;
    public PlayerBehavior pb;
 
    [Header("Plant & Request Data")]
    [SerializeField] private PlantSO[] allPlants;
    public PlantSO playerPickedPlant;

    [SerializeField] private float waitForPlayerDialogue = 3f;

    private bool playerInZone = false;
    private bool npcInZone = false;
    private bool playerDialogueShown = false;
    private bool waitingForPlant = false;
    private PlantSO currentRequest = null;
    private PlantSO deliveredPlant = null;

    void Start()
    {
        if (pb == null) pb = FindObjectOfType<PlayerBehavior>();
        if (dialogueManager == null) dialogueManager = FindObjectOfType<DialogueManager>();
        if (instance == null) instance = FindObjectOfType<GameStateManager>();

        if (instance != null)
        {
            activePlayer = instance.selectedPlayerSO;
        }
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
        }

        if (other.CompareTag("Boris"))
        {
            npcInZone = true;
        }
        if (playerInZone && npcInZone && !playerDialogueShown && currentRequest == null)
        {
            Debug.Log("In CashRegister: player and NPC r in cash register");
            playerDialogueShown = true;

            dialogueManager.StartPlayerDialogue();

            if (pb.PlantYouAreHolding != null && waitingForPlant)
            {
                DeliverPlant();
                //dialogueManager.EndPlayerDialogue();
            }
            else if (pb.PlantYouAreHolding == null && !waitingForPlant && currentRequest == null)
            {
                dialogueManager.StartPlayerDialogue();
                StartCoroutine(GenerateNewRequestRoutine());
            }
            //else {
            //    StartCoroutine(WaitForPlayerDialogue()); 
            //}
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            playerInZone = false;

        if (other.CompareTag("Boris"))
        {
            Debug.Log("NPC has left the register zone. Resetting state.");
            npcInZone = false;
            ResetState();
        }
    }
    IEnumerator WaitForPlayerDialogue()
    {

        yield return new WaitForSeconds(waitForPlayerDialogue);
        dialogueManager.EndPlayerDialogue();

        ////Create random plant request

        //currentRequest = GetRandomPlant();
        //emptyPlant = currentRequest;

        //dialogueManager.StartNpcDialogue(emptyPlant);
        //waitingForPlant = true;

        //ShowRequestIcon();
    }
    private IEnumerator GenerateNewRequestRoutine()
    {
        if (waitingForPlant)
        {
            yield break;
        }

        yield return new WaitForSeconds(waitForPlayerDialogue);
        dialogueManager.EndPlayerDialogue();

        currentRequest = GetRandomPlant();
        Debug.Log("In CashRegister: Random plant request: " + currentRequest.name);
        if (currentRequest != null)
        {
            dialogueManager.StartNpcDialogue(currentRequest);
            waitingForPlant = true;
        }
        else
        {
            Debug.LogError("CashRegister: Failed to get a random plant. Check allPlants array in the Inspector");
        }
    }
    
    //player puts plant at cash reg
    public void DeliverPlant()
    {
        if (!waitingForPlant || pb.PlantYouAreHolding == null) return;
      
        deliveredPlant = pb.PlantYouAreHolding.GetComponent<Plant>().currentPlantType;
        //waitingForPlant = false;

        bool correct = PlantMatching.CheckPlantMatch(deliveredPlant, currentRequest);

        if (correct)
        {
            Debug.Log("In CashRegister: Correct plant! Tip added.");
            cash.AddMoney(deliveredPlant.Price);
            cash.AddTip(5);
        }
        else
        {
            Debug.Log("Wrong plant! -10$");
            cash.SubtractMoney(10);
        }

        Destroy(pb.PlantYouAreHolding);
        pb.PlantYouAreHolding = null;

        ResetState();
    }
    public void CompleteTransaction()
    {
        //Debug.Log("In CashRegister: Transaction complete. Ready for new request.");
        ResetState();
        //waitingForPlant = false;
    }
    private PlantSO GetRandomPlant()
    {
        if (allPlants == null || allPlants.Length == 0)
        {
            Debug.LogWarning("In CashRegister: allPlants array is empty or null.");
            return null;
        }
        int randomIndex = Random.Range(0, allPlants.Length);
        return allPlants[randomIndex];
    }
    private void ResetState()
    {
        //Debug.Log("--- RESETTING CASH REGISTER STATE ---");
        //var plantToDestory = pb.PlantYouAreHolding;
        //pb.PlantYouAreHolding = null;

        //GameObject plantToDestroy = pb.PlantYouAreHolding;
        //if (plantToDestroy != null)
        //{
        //    plantToDestroy = null;
        //    Destroy(plantToDestroy);           
        //}

        //pb.PlantYouAreHolding = null;
        deliveredPlant = null;
        currentRequest = null;
        waitingForPlant = false;
        playerDialogueShown = false;
    }
}