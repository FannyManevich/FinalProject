using UnityEngine;
using System.Collections.Generic;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }
    public List<PlantSO> plantsInStock { get; private set; }

    [Header("Plant Databases")]
    [SerializeField] private List<PlantSO> allPossiblePlants;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }
    private void Start()
    {
        UpdatePlantsInStock();
    }

    public void UpdatePlantsInStock()
    {
        plantsInStock = new List<PlantSO>(allPossiblePlants);
        Debug.Log($"Inventory Initialized. {plantsInStock.Count} plant types are in stock.");
    }
    public List<PlantSO> GetAvailablePlants()
    {
        return plantsInStock;
    }
    public PlantSO GetRandomPlant()
    {
        if (plantsInStock == null || plantsInStock.Count == 0)
        {
            Debug.LogWarning("In CashRegister: allPlants array is empty or null.");
            return null;
        }
        int randomIndex = Random.Range(0, plantsInStock.Count);
        return plantsInStock[randomIndex];
    }
}    