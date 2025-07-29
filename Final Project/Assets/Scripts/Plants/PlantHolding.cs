using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantHolding : MonoBehaviour
{
    public bool follow = false;
    public GameObject holder = null;


    // Update is called once per frame
    void Update()
    {
        if (follow == true)
        {
            this.GetComponent<BoxCollider2D>().enabled = false;
            transform.position = holder.transform.position;
        }
        else
        {
            this.GetComponent<BoxCollider2D>().enabled = true;
        }
    }

    public void FollowHolder(GameObject Holding)
    {
        holder = Holding;
        follow = true;
    }

    public void StopFollowHolder()
    {
        follow = false;
        holder = null;
    }
}
