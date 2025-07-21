using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CashRegisterInteraction : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject playerDialoguePanel;
    public GameObject npcDialoguePanel;
    public Button cancelButton;

    [Header("NPC & Plant Data")]
    public PlantSO[] allPlants;
    private PlantSO currentRequest;
    private PlantSO deliveredPlant;

    [Header("UI Elements")]
    public Image portraitImage;
    public TextMeshProUGUI nameText;

    private bool npcAtRegister = false;
    private bool waitingForPlant = false;

    private void Start()
    {
        cancelButton.onClick.AddListener(CloseNpcPanel);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("NPC") && !npcAtRegister)
        {
            npcAtRegister = true;
            StartDialogue();
        }
    }

    private void StartDialogue()
    {
        Debug.Log("Player says hello");
        playerDialoguePanel.SetActive(true);

        Invoke(nameof(StartNpcDialogue), 1.5f);
    }

    private void StartNpcDialogue()
    {
        playerDialoguePanel.SetActive(false);

        currentRequest = GetRandomPlant();
        npcDialoguePanel.SetActive(true);

        portraitImage.sprite = currentRequest.Image;
        nameText.text = $"I want: {currentRequest.name}";

        waitingForPlant = true;
    }

    private void CloseNpcPanel()
    {
        npcDialoguePanel.SetActive(false);
    }

    // Call this externally when player brings a plant to register
    public void DeliverPlant(PlantSO plant)
    {
        if (!waitingForPlant) return;

        deliveredPlant = plant;
        waitingForPlant = false;

        bool correct = CheckPlantMatch(deliveredPlant, currentRequest);

        if (correct)
        {
            Debug.Log("Correct plant! Tip added.");

        }
        else
        {
            Debug.Log("Wrong plant! Penalty.");

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
        return allPlants[Random.Range(0, allPlants.Length)];
    }

    private void ResetState()
    {
        npcAtRegister = false;
        deliveredPlant = null;
        currentRequest = null;
    }
}
