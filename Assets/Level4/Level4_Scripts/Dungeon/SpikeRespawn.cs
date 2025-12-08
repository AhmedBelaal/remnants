using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeRespawn : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        LevelManager levelManager = FindObjectOfType<LevelManager>();
        if (other.CompareTag("Player"))
        {
            {           
                other.transform.position = levelManager.CurrentCheckpoint.transform.position;
            }
        }
    }

}
