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
    // private PlantSO request;

    public Image portraitImage;
    public TextMeshProUGUI nameText;

    public Image[] waterDrops;
    public Sprite fullDrop;
    public Sprite emptyDrop;

    public Image[] difficultyIcons;
    public Sprite activeIcon;
    public Sprite inactiveIcon;

    public Image sunImage;
    public Sprite fullSun;
    public Sprite partialShade;
    public Sprite fullShade;

    private void OnEnable()
    {
        dm = FindObjectOfType<DialogueManager>();
        dm.OnPlayerDialogue += DisplayPlayerDialogue;
        dm.OnNpcDialogue += DisplayNpcDialogue;
        dm.EndPdialogue += EndPlayerDialogue;
        dm.EndNpcDialogue += EndNpcDialogue;
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

        int waterLevel = randomPlantSO.waterRequirement;
        int diffLevel = randomPlantSO.difficultyLevel;
        int sunLevel = randomPlantSO.sunRequirement;

        switch (sunLevel)
        {
            case 1:
                sunImage.sprite = fullSun;
                break;
            case 2:
                sunImage.sprite = partialShade;
                break;
            case 3:
                sunImage.sprite = fullShade;
                break;
        }

        SetDifficulty(diffLevel);
        SetWater(waterLevel);
    }

    private void SetDifficulty(int level)
    {
        for (int i = 0; i < difficultyIcons.Length; i++)
        {
            difficultyIcons[i].sprite = i < level ? activeIcon : inactiveIcon;
        }
    }

    private void SetWater(int level)
    {
        for (int i = 0; i < waterDrops.Length; i++)
        {
            waterDrops[i].sprite = i < level ? fullDrop : emptyDrop;
        }
    }

    public void DisplayPlayerDialogue()
    {
        playerDialoguePanel.SetActive(true);
    }

    public void DisplayNpcDialogue(PlantSO randomPlantSO)
    {       
        cancelButton.gameObject.SetActive(true);
        npcDialoguePanel.SetActive(true);

        Debug.LogWarning("PlantRequest: diff: "+ randomPlantSO.difficultyLevel + "water: " + randomPlantSO.waterRequirement);
        DisplayPlantRequest(randomPlantSO);

      //  portraitImage.sprite = npc.NPC_portrait;
       // nameText.text = npc.NPC_name;
    }

    public void EndNpcDialogue()
    {
        cancelButton.gameObject.SetActive(false);
        npcDialoguePanel.SetActive(false);
    }

    public void EndPlayerDialogue()
    {
        playerDialoguePanel.SetActive(false);
    }
}
