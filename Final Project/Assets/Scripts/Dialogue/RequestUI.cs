using TMPro;
using UnityEngine;

public class RequestUI : MonoBehaviour
{
    public GameObject requestPanel;
    public TextMeshProUGUI requestText;
    private PlantSO currentPlant;
    public void SetRequest(PlantSO plant)
    {
        currentPlant = plant;
        requestText.text = $" Sun: {plant.sunRequirement}\n Water: {plant.waterRequirement}\n Difficulty: {plant.difficultyLevel}";
    }

    public void ToggleRequestPanel()
    {
        requestPanel.SetActive(!requestPanel.activeSelf);
    }
}