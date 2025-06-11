using UnityEngine;

public class PlantRequest : MonoBehaviour
{
    public SunRequirement sunRequirement;
    public int waterRequirement;
    public int difficultyRequirement;


    public PlantSO newPlantSO;

    private void Awake()
    {
        if(newPlantSO == null)
        {
            newPlantSO = PlantSO.FindAnyObjectByType<PlantSO>();
        }
    }

    public PlantSO GenerateRequest(PlantSO randomPlantSO)
    {
        newPlantSO.sunRequirement = randomPlantSO.sunRequirement;
        newPlantSO.waterRequirement = randomPlantSO.waterRequirement;
        newPlantSO.difficultyLevel = randomPlantSO.difficultyLevel;

        Debug.Log("Sun:  "  + newPlantSO.sunRequirement + "Water:    " + newPlantSO.waterRequirement + "diff:   " + newPlantSO.difficultyLevel);

        return newPlantSO;
    }
}
