using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class NPCSpawner : MonoBehaviour
{    
    public static event Action<GameObject> OnCustomerSpawnedEvent;

    [Header("Prefabs and data:")]
    [SerializeField] private GameObject[] customerPrefabs;
    [SerializeField] private NPC_SO[] npcList;

    [Header("Spawn Timing in sec:")]
    [SerializeField] private float minSpawnTime = 3.0f;
    [SerializeField] private float maxSpawnTime = 7.0f;

    private float spawnTimer;
    private float timer;
    private int npcIndex = 0;
   
    private void Start()
    {
        SetNewSpawnTime();
    }
    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= spawnTimer)
        {
            SpawnCustomer();
            timer = 0;
            SetNewSpawnTime();
        }
    }
    private void SpawnCustomer()
    {
        if (customerPrefabs.Length == 0 || npcList.Length == 0)
        {
            Debug.LogError("Customer Prefabs or NPC List is not assigned in the inspector!");
            return;
        }

        int customerTypeIndex = Random.Range(0, customerPrefabs.Length);
        GameObject customerPrefab = customerPrefabs[customerTypeIndex];
        GameObject newCustomer = Instantiate(customerPrefab, transform.position, Quaternion.identity);

        NPC_SO currentNpcData = npcList[npcIndex];
        newCustomer.GetComponent<SpriteRenderer>().sprite = currentNpcData.NPC_portrait;
        npcIndex = (npcIndex + 1) % npcList.Length;

        OnCustomerSpawnedEvent?.Invoke(newCustomer);
    }

    private void SetNewSpawnTime()
    {
        spawnTimer = Random.Range(minSpawnTime, maxSpawnTime);
    }
}