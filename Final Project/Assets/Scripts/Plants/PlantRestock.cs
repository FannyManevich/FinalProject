using UnityEngine;

public class PlantRestock : MonoBehaviour
{
    bool restocked;
    public GameObject[] Plants;
    GameObject PlantHoldPos;
    GameObject Player;

    [SerializeField] GameObject PlantParent;
    [SerializeField] GameObject PlantPrefab;


    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        PlantHoldPos = GameObject.Find("PlayerPlantHoldPos");
        Plants = GameObject.FindGameObjectsWithTag("Plant");
    }

    public void restock()
    {
        if (Player == null)
        {
            Player = GameObject.FindGameObjectWithTag("Player");
            PlantHoldPos = GameObject.Find("PlayerPlantHoldPos");
        }
        restocked = false;
        foreach (GameObject Plant in Plants)
        {
            if (Plant.activeSelf == false)
            {
                Plant.SetActive(true);
                Player.GetComponent<PlayerBehavior>().PlantYouAreHolding = Plant;
                Player.GetComponent<PlayerBehavior>().PlantYouAreHolding.GetComponent<PlantHolding>().FollowHolder(PlantHoldPos);
                restocked = true;
                break;
            }
        }
        if (!restocked)
        {
            //create new plant
            Player.GetComponent<PlayerBehavior>().PlantYouAreHolding = Instantiate(PlantPrefab, PlantHoldPos.transform.position, Quaternion.identity);
            Player.GetComponent<PlayerBehavior>().PlantYouAreHolding.GetComponent<PlantHolding>().FollowHolder(PlantHoldPos);
        }
    }
}
