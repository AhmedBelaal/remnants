using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RespawnManager : MonoBehaviour
{
    public static RespawnManager instance;
    public Transform currentRespawnPoint;

    private void Awake()
    {
        instance = this;
    }

    public void SetRespawnPoint(Transform newPoint)
    {
        currentRespawnPoint = newPoint;
    }

    public void RespawnPlayer(GameObject player)
    {
        if (currentRespawnPoint != null)
        {
            player.transform.position = currentRespawnPoint.position;
        }
    }
}
