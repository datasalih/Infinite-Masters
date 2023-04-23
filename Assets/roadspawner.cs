using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class roadspawner : MonoBehaviour
{
    public GameObject road;

    public Transform parent;

    void Start()
    {
        InvokeRepeating("spawnroad", 19f, 19f);
    }



    void spawnroad()
    {
        GameObject spawnroad = Instantiate(road);

        spawnroad.transform.SetParent(parent.transform);
        
    }

}
