using System;
using UnityEngine;

public class PlantRequest : MonoBehaviour
{
    public SunRequirement sunRequirement;
    public int waterRequirement;
    public int difficultyRequirement;

    public int i;

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
        return newPlantSO;
    }
    //void Awake()
    //{
    //   // GenerateRandomRequest();
    //}

    //public void GenerateRandomRequest()
    //{
    //    //waterRequirement = Random.Range(1, 3);
    //    //difficultyRequirement = Random.Range(1, 3);
    //    //sunRequirement = (SunRequirement)Random.Range(0, 2);
    //}
}
