using UnityEngine;

public class PlantRestock : MonoBehaviour
{
    [Header("Configuration")]
    [SerializeField] private GameObject plantPrefab;
    public GameObject RequestPlant()
    {
        if (plantPrefab == null)
        {
            Debug.LogError("Generic Plant Prefab is not assigned on the PlantRestock component! Please assign it in the Inspector.");
            return null;
        }
        PlantSO plantData = InventoryManager.Instance.GetRandomPlant();
        if (plantData == null)
        {
            Debug.LogWarning("InventoryManager has no plants in stock.");
            return null;
        }
        GameObject newPlant = Instantiate(plantPrefab, transform.position, Quaternion.identity);
        newPlant.GetComponent<Plant>().Initialize(plantData);
        return newPlant;
    }

}