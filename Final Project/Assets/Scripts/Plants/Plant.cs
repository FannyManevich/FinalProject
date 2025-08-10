using UnityEngine;

public class Plant : MonoBehaviour
{
    [SerializeField] private PlantSO[] PlantTypes;
    [SerializeField] public PlantSO currentPlantType { get; private set; }

    private SpriteRenderer spriteRenderer;
    void Start()
    {
        if (currentPlantType == null)
        {
            currentPlantType = PlantTypes[Random.Range(0,PlantTypes.Length)];
        }
        GetComponent<SpriteRenderer>().sprite = currentPlantType.Image;
    }
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    public void Initialize(PlantSO newPlantData)
    {
        currentPlantType = newPlantData;

        if (currentPlantType != null && spriteRenderer != null)
        {
            spriteRenderer.sprite = currentPlantType.Image;
        }
    }
}
