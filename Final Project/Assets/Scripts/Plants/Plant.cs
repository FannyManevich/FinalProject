using UnityEngine;

public class Plant : MonoBehaviour
{
    [SerializeField] private PlantSO[] plantTypes;
    [SerializeField] public PlantSO CurrentPlantType { get; private set; }

    private SpriteRenderer spriteRenderer;
    void Start()
    {
        if (CurrentPlantType == null)
        {
            CurrentPlantType = plantTypes[Random.Range(0,plantTypes.Length)];
        }
        GetComponent<SpriteRenderer>().sprite = CurrentPlantType.image;
    }
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    public void Initialize(PlantSO newPlantData)
    {
        CurrentPlantType = newPlantData;

        if (CurrentPlantType != null && spriteRenderer != null)
        {
            spriteRenderer.sprite = CurrentPlantType.image;
        }
    }
}
