// Ignore Spelling: Dialogue npc diff

using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueUI : MonoBehaviour
{
    [Header("Canvas & Panels:")]
    [SerializeField] GameObject dialogueCanvas;
    [SerializeField] GameObject playerDialoguePanel;
    [SerializeField] GameObject npcDialoguePanel;

    [Header("Buttons:")]    
    [SerializeField] Button cancelButton;

    [Header("Managers:")]
    [SerializeField] public DialogueManager dm;
    public Input playerInput;

    [Header("NPC ID:")]
    public Image portraitImage;
    public TextMeshProUGUI nameText;

    [Header("Plant Request Icons:")]
    public Image sunImage;
    public Image difficultyImage;
    public Image waterImage;

    [SerializeField] public Sprite[] sunIcons;
    [SerializeField] public Sprite[] diffIcons;
    [SerializeField] public Sprite[] waterIcons;

    public PlantSO emptyPlant;

    private void Start()
    {
        cancelButton.onClick.AddListener(EndNpcDialogue);
    }

    private void OnEnable()
    {
        //dm = FindObjectOfType<DialogueManager>();
        dm.OnPlayerDialogue += DisplayPlayerDialogue;
        dm.OnNpcDialogue += DisplayNpcDialogue;
        dm.EndPdialogue += EndPlayerDialogue;
        dm.EndNpcDialogue += EndNpcDialogue;
        playerInput = new();
    }

    private void OnDisable()
    {
        if (dm != null)
        {
            dm.OnPlayerDialogue -= DisplayPlayerDialogue;
            dm.OnNpcDialogue -= DisplayNpcDialogue;
            dm.EndPdialogue -= EndPlayerDialogue;
            dm.EndNpcDialogue -= EndNpcDialogue;
        }
    }

    public void DisplayPlantRequest(PlantSO randomPlantSO)
    {
        if (randomPlantSO == null)
       // if(requestedPlant == null)
        {
            Debug.LogWarning("In DialougeUI: PlantRequest is null.");
            return;
        }

        int sunIndex = (int)randomPlantSO.sunRequirement;
        Debug.Log("In DialougeUI:  sun: " + sunIndex);

        if (sunImage != null && sunIndex >= 0 && sunIndex < sunIcons.Length)
        {
            sunImage.sprite = sunIcons[sunIndex];
            Debug.Log($"In DialougeUI: Setting sun icon: {sunIcons[sunIndex].name}");
        }
        else
        {
            Debug.LogError($"In DialougeUI: Invalid sunIndex: {sunIndex} or sunIcons array is not set up correctly.");
        }

        int difficultyIndex = randomPlantSO.difficultyLevel;
        if (difficultyImage != null && difficultyIndex >= 0 && difficultyIndex < diffIcons.Length)
        {
            difficultyImage.sprite = diffIcons[difficultyIndex];
            Debug.Log($"In DialougeUI: Setting difficulty icon: {diffIcons[difficultyIndex].name}");
        }
        else
        {
            Debug.LogError($"In DialougeUI: Invalid difficultyIndex: {difficultyIndex} or diffIcons array is not set up correctly.");
        }

        int waterIndex = randomPlantSO.waterRequirement;
        if (waterIcons != null && waterIndex >= 0 && waterIndex < waterIcons.Length)
        {
            waterImage.sprite = waterIcons[waterIndex];
            Debug.Log($"In DialougeUI: Setting water icon: {waterIcons[waterIndex].name}");
        }
        else
        {
            Debug.LogError($"In DialougeUI: Invalid waterIndex: {waterIndex} or waterIcons array is not set up correctly.");
        }
    }
    //sunImage = sunIcons[sunLevel];
    //difficultyImage = diffIcons[diffLevel];
    //waterImage = waterIcons[waterLevel];

    //emptyPlant.difficultyLevel = diffLevel;
    //    //emptyPlant.sunRequirement = sunLevel;
    //    emptyPlant.waterRequirement = waterLevel;
    //}
    public void DisplayPlayerDialogue()
    {
       // Debug.Log("In DialougeUI: DisplayPlayerDialogue");
        dialogueCanvas.SetActive(true);
        playerDialoguePanel.SetActive(true);
    }
    public void DisplayNpcDialogue(PlantSO randomPlantSO)
    {
        dialogueCanvas.SetActive(!npcDialoguePanel.activeInHierarchy);
        if (npcDialoguePanel.activeInHierarchy)
        {
            cancelButton.gameObject.SetActive(false);
            npcDialoguePanel.SetActive(false);
            playerInput.Player.Enable();
            playerInput.UI.Disable();
        }
        if (!npcDialoguePanel.activeInHierarchy)
        {
            playerInput.Player.Disable();
            playerInput.UI.Enable();

            cancelButton.gameObject.SetActive(true);
            npcDialoguePanel.SetActive(true);

            //Debug.Log("In DialogueUI: PlantRequest: Sun: " +  randomPlantSO.sunRequirement + " diff:  "+ randomPlantSO.difficultyLevel + " water: " + randomPlantSO.waterRequirement);
            DisplayPlantRequest(randomPlantSO);
        }
        // portraitImage.sprite = npc.NPC_portrait;
        // nameText.text = npc.NPC_name;
    }
    public void EndNpcDialogue()
    {
        //Debug.Log("End NPC Dialogue");
        cancelButton.gameObject.SetActive(false);
        npcDialoguePanel.SetActive(false);
        dialogueCanvas.SetActive(false);
    }
    public void EndPlayerDialogue()
    {
        dialogueCanvas.SetActive(false);
        playerDialoguePanel.SetActive(false);
    }
}