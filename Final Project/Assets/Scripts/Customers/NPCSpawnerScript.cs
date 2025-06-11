using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class NPCSpawnerScript : MonoBehaviour
{
    public GameObject[] CustomerPrefab;
    public int CustomerType;
    public int CustomerTypeRNG;
    public int spawnTimer;
    public int customerSprite=0;   
    public int timer=0;
    public Sprite[] CustomerList;

    public NPC_SO[] npcList;
    public int npcIndex = 0;

    public int minSpawnTime = 1400;
    public int maxSpawnTime = 3400;

    public static event Action<GameObject> OnCustomerSpawnedEvent;

    private void Start()
    {
        spawnTimer = Random.Range(minSpawnTime, maxSpawnTime);
    }

    // Update is called once per frame
    void Update()
    {
        timer++;
        if (timer > spawnTimer)
        { 
            if(npcIndex == 3)
            {
                npcIndex = 0;
            }

            GameObject c = Instantiate(CustomerPrefab[CustomerType], transform.position, Quaternion.identity);
            if (CustomerPrefab != null)
            {
                c.GetComponent<SpriteRenderer>().sprite = npcList[npcIndex].NPC_portrait;
            }
            OnCustomerSpawnedEvent?.Invoke(c);
            npcIndex++;
            CustomerTypeRNG = Random.Range(0, 10);
            if (CustomerTypeRNG > 7)
            {
                CustomerType = 0;
            }
            else
            {
                CustomerType = 1;
            }
                spawnTimer = UnityEngine.Random.Range(minSpawnTime, maxSpawnTime);
            timer = 0;
        }
    }
}
