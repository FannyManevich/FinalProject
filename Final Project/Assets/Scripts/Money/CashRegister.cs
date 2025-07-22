using System.Collections;
using UnityEngine;

public class CashRegister : MonoBehaviour
{
    [Header("Component References")]
    [SerializeField] private DialogueManager dialogueManager;
    [SerializeField] private GameStateManager instance;
    [SerializeField] private RequestUI requestUI;
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
        if (requestUI == null) requestUI = FindObjectOfType<RequestUI>(true);

        if (requestUI != null)
        {
            requestUI.gameObject.SetActive(false);
        }

        if (instance != null)
        {
            activePlayer = instance.selectedPlayerSO;
        }
    }
    private void OnEnable()
    {
        if (dialogueManager != null)
        {
            NPCSpawnerScript.OnCustomerSpawnedEvent += ReadNPCData;           
        }
    }
    private void OnDisable()
    {
        if (dialogueManager != null)
        {
            NPCSpawnerScript.OnCustomerSpawnedEvent -= ReadNPCData;
        }
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
        if (playerInZone && npcInZone && !playerDialogueShown)
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
        yield return new WaitForSeconds(waitForPlayerDialogue);
        dialogueManager.EndPlayerDialogue();

        currentRequest = GetRandomPlant();
        Debug.Log("CashRegister: Random plant request: " + currentRequest);
        if (currentRequest != null)
        {
            dialogueManager.StartNpcDialogue(currentRequest);
            waitingForPlant = true;
            ShowRequestIcon();
        }
        else
        {
            Debug.LogError("Failed to get a random plant. Check allPlants array in the Inspector");
        }
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

        bool correct = PlantMatching.CheckPlantMatch(deliveredPlant, currentRequest);

        if (correct)
        {
            Debug.Log("Correct plant! Tip added.");
            cash.AddMoney(deliveredPlant.Price);
            cash.AddTip(5);
        }
        else
        {
            Debug.Log("Wrong plant! -10$");
            cash.SubtractMoney(10);
        }
        ResetState();
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
    void ShowRequestIcon()
    {
        if (requestUI != null && currentRequest != null)
        {
            requestUI.gameObject.SetActive(true);
            requestUI.SetRequest(currentRequest);
        }
    }
    private void ResetState()
    {
        if (requestUI != null)
        {
            requestUI.gameObject.SetActive(false);
        }
        //deliveredPlant = null;
        //var plantToDestory = pb.PlantYouAreHolding;
        //pb.PlantYouAreHolding = null;

        GameObject plantToDestroy = pb.PlantYouAreHolding;
        if (plantToDestroy != null)
        {
            plantToDestroy = null;
            Destroy(plantToDestroy);           
        }

        pb.PlantYouAreHolding = null;
        deliveredPlant = null;
        currentRequest = null;
        waitingForPlant = false;
    }
}