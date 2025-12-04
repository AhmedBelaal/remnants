using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Looping : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Transform teleportToPoint; 

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Teleport the player back to the start
            // We keep the Y position to prevent snapping into the ground
            other.transform.position = new Vector3(teleportToPoint.position.x, other.transform.position.y, other.transform.position.z);
        }
    }
}
