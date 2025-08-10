using UnityEngine;
using System.Collections.Generic;

public class PlantRestock : MonoBehaviour
{
    [Header("Configuration")]
    [SerializeField] private string plantTag = "Plant";

    private List<Plant> plants = new List<Plant>();

    void Start()
    {
        RestockPlants();
    }

    private void RestockPlants()
    {
        plants.Clear();
        GameObject[] plantObjects = GameObject.FindGameObjectsWithTag(plantTag);

        foreach (GameObject plantGO in plantObjects)
        {
            if (plantGO.TryGetComponent<Plant>(out Plant plantComponent))
            {
                plants.Add(plantComponent);
                plantGO.SetActive(false);
            }
        }
    }

    public GameObject RequestPlant()
    {
        PlantSO plantDataToAssign = InventoryManager.Instance.GetRandomPlant();
        if (plantDataToAssign == null)
        {
            Debug.LogWarning("In PlantRestock: InventoryManager has no plants in stock.");
            return null;
        }

        foreach (Plant plant in plants)
        {
            if (!plant.gameObject.activeSelf)
            {
                plant.Initialize(plantDataToAssign);
                plant.gameObject.SetActive(true);
                return plant.gameObject;
            }
        }
        Debug.LogWarning("In PlantRestock: No plants available to restock.");
        return null;
    }
}