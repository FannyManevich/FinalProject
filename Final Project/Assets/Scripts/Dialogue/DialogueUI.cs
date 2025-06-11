// Ignore Spelling: Dialogue npc

using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueUI : MonoBehaviour
{
    [SerializeField] GameObject playerDialoguePanel;
    [SerializeField] GameObject npcDialoguePanel;
    [SerializeField] Button cancelButton;

    private DialogueManager dm;
    public Input playerInput;

    public Image portraitImage;
    public TextMeshProUGUI nameText;

    public Sprite sunImage;
    public Sprite difficultyImage;
    public Sprite waterImage;

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
        dm = FindObjectOfType<DialogueManager>();
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
        if(randomPlantSO == null)
        {
            Debug.LogWarning("PlantRequest is null.");
            return;
        }

        int sunLevel = randomPlantSO.sunRequirement;
        int diffLevel = randomPlantSO.difficultyLevel;
        int waterLevel = randomPlantSO.waterRequirement;

        if((sunLevel < 0 && sunLevel >= 3 )|| (diffLevel < 0 && diffLevel >= 3) || (waterLevel < 0 && waterLevel >= 3))
        {
            sunLevel = 1;
            diffLevel = 1;
            waterLevel = 1;
        }
        else
        {
            sunImage = sunIcons[sunLevel];
            difficultyImage = diffIcons[diffLevel];
            waterImage = waterIcons[waterLevel];

            emptyPlant.difficultyLevel = diffLevel;
            emptyPlant.sunRequirement = sunLevel;
            emptyPlant.waterRequirement = waterLevel;
        }
    }
    
    public void DisplayPlayerDialogue()
    {
        playerDialoguePanel.SetActive(true);
    }

    public void DisplayNpcDialogue(PlantSO randomPlantSO)
    {
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

           // Debug.LogWarning("PlantRequest: Sun: " +  randomPlantSO.sunRequirement + " diff: "+ randomPlantSO.difficultyLevel + "water: " + randomPlantSO.waterRequirement);
            DisplayPlantRequest(randomPlantSO);
        }      
       // portraitImage.sprite = npc.NPC_portrait;
       // nameText.text = npc.NPC_name;
    }

    public void EndNpcDialogue()
    {
        Debug.Log("End NPC Dialogue");
        cancelButton.gameObject.SetActive(false);
        npcDialoguePanel.SetActive(false);
    }

    public void EndPlayerDialogue()
    {
        playerDialoguePanel.SetActive(false);
    }
}
