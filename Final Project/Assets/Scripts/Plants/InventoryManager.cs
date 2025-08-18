using UnityEngine;
using System.Collections.Generic;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }
    public List<PlantSO> PlantsInStock { get; private set; }

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
        PlantsInStock = new List<PlantSO>(allPossiblePlants);
       // Debug.Log($"In InventoryManager: Inventory Initialized. {plantsInStock.Count} plant types are in stock.");
    }
    public List<PlantSO> GetAvailablePlants()
    {
        return PlantsInStock;
    }
    public PlantSO GetRandomPlant()
    {
        if (PlantsInStock == null || PlantsInStock.Count == 0)
        {
            Debug.LogWarning("In InventoryManager: allPlants array is empty or null.");
            return null;
        }
        int randomIndex = Random.Range(0, PlantsInStock.Count);
        return PlantsInStock[randomIndex];
    }

    public void Restock(PlayerBehavior player)
    {
        if (player.HeldPlantData != null)
        {
            Debug.Log("In PlantRestock: player is already holding a plant.");
            return;
        }

        PlantSO randomPlant = GetRandomPlant();

        if (randomPlant == null)
        {
            Debug.LogWarning("In PlantRestock: inventory manager  out of stock.");
            return;
        }

        Debug.Log("In PlantRestock: giving " + randomPlant.name + " to the player.");
        player.TakeFromStock(randomPlant);
    }
}    