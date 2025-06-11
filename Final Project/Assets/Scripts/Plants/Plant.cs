using UnityEngine;

public class Plant : MonoBehaviour
{
    [SerializeField] PlantSO[] PlantTypes;
    [SerializeField] public PlantSO currentPlantType;
    // Start is called before the first frame update
    void Start()
    {
        if (currentPlantType == null)
        {
            currentPlantType = PlantTypes[Random.Range(0,PlantTypes.Length)];
        }
        GetComponent<SpriteRenderer>().sprite = currentPlantType.Image;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
