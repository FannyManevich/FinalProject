using UnityEngine;
public static class PlantMatching
{
    public static bool CheckPlantMatch(PlantSO a, PlantSO b)
    {
        return a.sunRequirement == b.sunRequirement &&
               a.waterRequirement == b.waterRequirement &&
               a.difficultyLevel == b.difficultyLevel;
    }
}